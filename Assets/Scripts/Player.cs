using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private static float CLICK_TIME = 0.15f;
    private static float DELAYED_DRAG_TIME = 0.5f;
    private static float DRAG_START_DISTANCE = 10;

    private float elapsedTime = 0;
    private Entity grabbed = null;
    Entity hovering = null;

    // In case of failure, we drop it back here
    private Vector3 startingMousePosition; // Screen space

    public override void OnStartLocalPlayer()
    {
        GameObject.Find("StartingObjects").gameObject.transform.parent = transform;
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

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
        if (Input.GetMouseButtonUp(0) && grabbed == null && hits.Length > 0 &&
            elapsedTime < CLICK_TIME)
        {
            RaycastHit hit = hits[0];
            hit.collider.GetComponent<Entity>().Click(hit.point);
        }

        // StartDelayedDrag (mouse down and hasn't moved for a while)
        // Topmost object gets dragged
        if (Input.GetMouseButton(0) && grabbed == null && hits.Length > 0 &&
            elapsedTime >= DELAYED_DRAG_TIME)
        {
            RaycastHit hit = hits[0];
            grabbed = hit.collider.GetComponent<Entity>().StartDelayedDrag(hit.point);
            if (grabbed != null)
                grabbed.gameObject.layer = 9;

        }

        // StartDrag (movement)
        // Topmost object gets dragged
        if (Input.GetMouseButton(0) && grabbed == null && hits.Length > 0 &&
            (Input.mousePosition - startingMousePosition).magnitude > DRAG_START_DISTANCE)
        {

            RaycastHit hit = hits[0];
            grabbed = hit.collider.GetComponent<Entity>().StartDrag(hit.point);
            if (grabbed != null)
                grabbed.gameObject.layer = 9;
        }

        // Hover (holding something over something else)
        // HoverOff for when we stop hovering it
        // We hover the topmost entity that responds (so not cards for example)
        if (Input.GetMouseButton(0) && grabbed)
        {
            Entity newhovering = null;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<Entity>().Hover(grabbed, hit.point))
                {
                    newhovering = hit.collider.GetComponent<Entity>();
                    break;
                }
            }

            if (newhovering != hovering && hovering != null)
                hovering.HoverOff();

            hovering = newhovering;
        }

        // Drop (release something over something)
        // We try from the topmost object, if nothing takes the object we put it back
        if (Input.GetMouseButtonUp(0) && grabbed && hits.Length > 0)
        {
            grabbed.gameObject.layer = 8;

            bool dropSuccessful = false;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<Entity>().Drop(grabbed, hit.point))
                {
                    // If drop is successful, we're good
                    dropSuccessful = true;
                    break;
                }
            }

            if (!dropSuccessful)
            {
                // If not we put it back to where it came from
                Ray ray2 = Camera.main.ScreenPointToRay(startingMousePosition);
                RaycastHit[] hits2 = Physics.RaycastAll(ray2, 100, 1 << 8); // Entity Layer
                foreach (RaycastHit hit2 in hits2)
                {
                    if (hit2.collider.GetComponent<Entity>().Drop(grabbed, hit2.point))
                        break;
                }
            }
            grabbed = null;
        }
    }
}
