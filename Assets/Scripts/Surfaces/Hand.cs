using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Surface
{
    protected override bool CanTakeEntity()
    {
        return entities.Count < 10;
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
            float horizontalPos = width * ((float) entities.IndexOf(e) / (float) entities.Count - 0.5f) + width / (float) entities.Count / 2;
            e.FlyTo(new Vector3(horizontalPos, transform.position.y + 0.1f, transform.position.z));
        }
    }
}
