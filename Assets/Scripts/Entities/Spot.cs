using UnityEngine;
using UnityEngine.Networking;

public class Spot : Entity, IContainer
{
    public DeckColor SpawnedDeckColor = DeckColor.Blue;

    public AutoFacingOptions AutoFacing { get; set; }
    public bool ForceFacing { get; set; }

    public Entity _entity;

    private void CreateDeck()
    {
        GameObject deck = Instantiate(Resources.Load<GameObject>("Prefabs/Deck"), transform.position + new Vector3(0, 0.05f, 0), Quaternion.identity);
        NetworkServer.Spawn(deck);
        deck.GetComponent<Deck>().Initialize(this.netId, SpawnedDeckColor);
    }

    [ClientRpc]
    public void RpcAddEntity(NetworkInstanceId entity, Vector3 hitPos)
    {
        AddEntity(ClientScene.FindLocalObject(entity).GetComponent<Entity>(), hitPos);
    }
    public bool AddEntity(Entity entity, Vector3 hitPos) {
        if (HoverAnswered(entity, hitPos))
        {
            _entity = entity;
            entity.GetComponent<Movement>().Container = this;
            entity.GetComponent<Movement>().TargetPosition = transform.position + new Vector3(0, 0.05f, 0);
            return true;
        }
        return false;
    }

    [ClientRpc]
    public void RpcRemoveEntity(NetworkInstanceId entity)
    {
        RemoveEntity(ClientScene.FindLocalObject(entity).GetComponent<Entity>());
    }
    public void RemoveEntity(Entity entity)
    { 
        entity.GetComponent<Movement>().ContainerID = new NetworkInstanceId();
        _entity = null;
    }

    // Event handlers

    public override bool HoverAnswered(Entity entity, Vector3 hitPos)
    {
        return (_entity == null);
    }

    public override void Hover(Entity entity, Vector3 hitPos)
    {
        if (HoverAnswered(entity, hitPos))
        {
            entity.GetComponent<Movement>().TargetPosition = transform.position + new Vector3(0, 1, 0);
        }
    }

    public override bool DropAccepted(Entity entity, Vector3 hitPos)
    {
        return HoverAnswered(entity, hitPos);
    }

    public override void Drop(Entity entity, Vector3 hitPos)
    {
        RpcAddEntity(entity.netId, hitPos);
    }

    public override void Click(Vector3 hitPos)
    {
        CreateDeck();
    }
}
