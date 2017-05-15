using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Surface
{
    public int size = 10;

    protected override bool CanTakeEntity()
    {
        return entities.Count < size;
    }

    protected override void PlaceNewEntity(Entity entity, Vector3 hitPos)
    {
        float width = GetComponent<Renderer>().bounds.size.x;
        float handPlaceRatio = (hitPos.x - transform.position.x) / width + 0.5f;

        entities.Insert(Mathf.FloorToInt(handPlaceRatio * (entities.Count + 1)), entity);

        Rearrange();
    }

    protected override void RemoveEntity(Entity entity)
    {
        entities.Remove(entity);
        Rearrange();
    }

    private void Rearrange()
    {
        float width = GetComponent<Renderer>().bounds.size.x;
        foreach (Entity e in entities)
        {
            float horizontalPos = transform.position.x + width * ((float) entities.IndexOf(e) / entities.Count - 0.5f) + width / entities.Count / 2;
            e.FlyTo(new Vector3(horizontalPos, transform.position.y + 0.1f, transform.position.z));
        }
    }
}
