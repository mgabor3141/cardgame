using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : IContainer
{
    public bool isEntityInContainer(Vector2 pos)
    {
        return true;
    }

    public Vector3 GetTargetPosition(Vector2 screenPos)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        int layerMask = 1 << 8; // Container layer
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            Vector3 directionNormal = ray.direction;
            directionNormal.Normalize();
            Vector3 scaledNormal = hit.normal;
            scaledNormal.Scale(new Vector3(0.1f, 0.1f, 0.1f));
            return hit.point + scaledNormal;// - directionNormal * 1;
        }
        throw new Exception("This should never happen");
    }
}
