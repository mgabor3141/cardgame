using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private static float CLICK_TIME = 0.15f;
    private static float DELAYED_DRAG_TIME = 0.5f;
    private static float DRAG_START_DISTANCE = 10;

    private NetworkInstanceId grabbed = new NetworkInstanceId();

    private float elapsedTime = 0;
    private Entity hovering = null; 

    // In case of failure, we drop it back here
    private Vector3 startingMousePosition; // Screen space

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        // Timer
        elapsedTime += Time.deltaTime;

        // What is under the cursor/finger?
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100, 1 << 8); // Entity Layer
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        // Mouse clicked, start the clock
        if (Input.GetMouseButtonDown(0) && hits.Length > 0)
        {
            elapsedTime = 0;

            startingMousePosition = Input.mousePosition;
        }

        // Click (on quick mouse release)
        // Topmost object gets clicked
        if (Input.GetMouseButtonUp(0) && grabbed.IsEmpty() && hits.Length > 0 &&
            elapsedTime < CLICK_TIME)
        {
            RaycastHit hit = hits[0];
            CmdClick(hit.point, hit.collider.GetComponent<Entity>().netId);
        }

        // StartDelayedDrag (mouse down and hasn't moved for a while)
        // Topmost object gets dragged
        if (Input.GetMouseButton(0) && grabbed.IsEmpty() && hits.Length > 0 &&
            elapsedTime >= DELAYED_DRAG_TIME)
        {
            RaycastHit hit = hits[0];
            CmdStartDelayedDrag(hit.point, gameObject, hit.collider.GetComponent<Entity>().netId);
            Entity target = hit.collider.GetComponent<Entity>().DelayedDragTarget(hit.point);
            if (target != null)
            {
                grabbed = target.netId;
                ClientScene.FindLocalObject(grabbed).gameObject.layer = 9;
            }

        }

        // StartDrag (movement)
        // Topmost object gets dragged
        if (Input.GetMouseButton(0) && grabbed.IsEmpty() && hits.Length > 0 &&
            (Input.mousePosition - startingMousePosition).magnitude > DRAG_START_DISTANCE)
        {

            RaycastHit hit = hits[0];
            CmdStartDrag(hit.point, gameObject, hit.collider.GetComponent<Entity>().netId);
            Entity target = hit.collider.GetComponent<Entity>().DragTarget(hit.point);
            if (target != null)
            {
                grabbed = target.netId;
                ClientScene.FindLocalObject(grabbed).gameObject.layer = 9;
            }
        }

        // Hover (holding something over something else)
        // HoverOff for when we stop hovering it
        // We hover the topmost entity that responds (so not cards for example)
        if (Input.GetMouseButton(0) && !grabbed.IsEmpty())
        {
            Entity newhovering = null;
            foreach (RaycastHit hit in hits)
            {
                CmdHover(grabbed, hit.point, gameObject, hit.collider.GetComponent<Entity>().netId);
                if (hit.collider.GetComponent<Entity>().HoverAnswered(ClientScene.FindLocalObject(grabbed).GetComponent<Entity>(), hit.point))
                {
                    newhovering = hit.collider.GetComponent<Entity>();
                    break;
                }
            }

            if (newhovering != hovering && hovering != null)
                CmdHoverOff(hovering.netId);

            hovering = newhovering;
        }

        // Drop (release something over something)
        // We try from the topmost object, if nothing takes the object we put it back
        if (Input.GetMouseButtonUp(0) && !grabbed.IsEmpty() && hits.Length > 0)
        {
            ClientScene.FindLocalObject(grabbed).gameObject.layer = 8;

            bool dropSuccess = false;
            foreach (RaycastHit hit in hits)
            {
                CmdDrop(grabbed, hit.point, gameObject, hit.collider.GetComponent<Entity>().netId);
                dropSuccess = hit.collider.GetComponent<Entity>().DropAccepted(ClientScene.FindLocalObject(grabbed).GetComponent<Entity>(), hit.point);
                if (dropSuccess)
                    break;
            }

            if (!dropSuccess)
            {
                // If not we put it back to where it came from
                Ray ray2 = Camera.main.ScreenPointToRay(startingMousePosition);
                RaycastHit[] hits2 = Physics.RaycastAll(ray2, 100, 1 << 8); // Entity Layer
                foreach (RaycastHit hit2 in hits2)
                {
                    CmdDrop(grabbed, hit2.point, gameObject, hit2.collider.GetComponent<Entity>().netId);
                    dropSuccess = hit2.collider.GetComponent<Entity>().DropAccepted(ClientScene.FindLocalObject(grabbed).GetComponent<Entity>(), hit2.point);
                    if (dropSuccess)
                        break;
                }
            }
            grabbed = new NetworkInstanceId();
        }
    }

    // Entity Commands

    [Command]
    private void CmdClick(Vector3 hitPos, NetworkInstanceId target)
    {
        NetworkServer.FindLocalObject(target).GetComponent<Entity>().Click(hitPos);
    }

    [Command]
    private void CmdStartDrag(Vector3 hitPos, GameObject caller, NetworkInstanceId target)
    {
        NetworkServer.FindLocalObject(target).GetComponent<Entity>().StartDrag(hitPos);
    }

    [Command]
    private void CmdStartDelayedDrag(Vector3 hitPos, GameObject caller, NetworkInstanceId target)
    {
        NetworkServer.FindLocalObject(target).GetComponent<Entity>().StartDelayedDrag(hitPos);
    }

    [Command]
    private void CmdHover(NetworkInstanceId netId, Vector3 hitPos, GameObject caller, NetworkInstanceId target)
    {
        NetworkServer.FindLocalObject(target).GetComponent<Entity>().Hover(NetworkServer.FindLocalObject(netId).GetComponent<Entity>(), hitPos);
    }

    [Command]
    private void CmdHoverOff(NetworkInstanceId target)
    {
        NetworkServer.FindLocalObject(target).GetComponent<Entity>().HoverOff();
    }

    [Command]
    private void CmdDrop(NetworkInstanceId netId, Vector3 hitPos, GameObject caller, NetworkInstanceId target)
    {
        NetworkServer.FindLocalObject(target).GetComponent<Entity>().Drop(NetworkServer.FindLocalObject(netId).GetComponent<Entity>(), hitPos);
    }
}
