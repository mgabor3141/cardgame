using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Surface : MonoBehaviour
{
    public List<Entity> entities;

    public enum ForceFacingOptions { None, Up, Down };
    public ForceFacingOptions ForceFacing;

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

            // this is ugly, TODO
            if (entity.GetType() == typeof(Card))
            {
                Card card = (Card)entity;
                switch (ForceFacing)
                {
                    case ForceFacingOptions.Up:
                        card.facingUp = true;
                        break;
                    case ForceFacingOptions.Down:
                        card.facingUp = false;
                        break;
                }

                entity = card;
            }

            PlaceNewEntity(entity, hitPos);

            return true;
        }
        return false;
    }
}