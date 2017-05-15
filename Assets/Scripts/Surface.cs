using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Surface : MonoBehaviour
{
    public List<Entity> entities;

    protected abstract bool CanTakeEntity();

    protected virtual void PlaceNewEntity(Entity entity, Vector3 hitPos)
    {
        entities.Add(entity);
    }

    protected virtual void RemoveEntity(Entity entity)
    {
        entities.Remove(entity);
    }

    public bool InsertEntity(Entity entity, Vector3 hitPos)
    {
        if (CanTakeEntity())
        {
            entity.surface.RemoveEntity(entity);
            entity.surface = this;
            PlaceNewEntity(entity, hitPos);
            return true;
        }
        return false;
    }
}