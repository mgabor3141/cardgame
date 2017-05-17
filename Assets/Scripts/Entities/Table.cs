using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Table : Entity, IContainer {
    public int size = 1000;
    private List<Entity> _entities = new List<Entity>();

    public AutoFacingOptions AutoFacing { get; set; }
    public bool ForceFacing { get; set; }

    [ClientRpc]
    public void RpcAddEntity(NetworkInstanceId entityId, Vector3 hitPos)
    {
        AddEntity(ClientScene.FindLocalObject(entityId).GetComponent<Entity>(), hitPos);
    }
    public bool AddEntity(Entity entity, Vector3 hitPos)
    {
        if (_entities.Count == size) return false;

        entity.GetComponent<Movement>().Container = this;

        _entities.Add(entity);
        entity.GetComponent<Movement>().TargetPosition = hitPos + new Vector3(0, 0.1f, 0);

        return true;
    }

    void IContainer.RemoveEntity(Entity entity)
    {
        entity.GetComponent<Movement>().Container = null;
        _entities.Remove(entity);
    }

    // Event handlers

    public override bool HoverAnswered(Entity entity, Vector3 hitPos)
    {
        return true;
    }

    public override void Hover(Entity entity, Vector3 hitPos)
    {
        entity.GetComponent<Movement>().TargetPosition = hitPos + new Vector3(0, 1, 0);
    }

    public override bool DropAccepted(Entity entity, Vector3 hitPos)
    {
        return (_entities.Count != size);
    }

    public override void Drop(Entity entity, Vector3 hitPos)
    {
        if (DropAccepted(entity, hitPos))
        {
            RpcAddEntity(entity.netId, hitPos);
        }
    }
}
