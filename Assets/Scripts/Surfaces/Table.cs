using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Surface {
    public int size = 1000;

    protected override bool CanTakeEntity()
    {
        return entities.Count < size;
    }

    protected override void PlaceNewEntity(Entity entity, Vector3 hitPos)
    {
        entities.Add(entity);
        entity.FlyTo(hitPos + new Vector3(0, 0.1f, 0));
    }
}
