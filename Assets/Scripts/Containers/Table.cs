using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour, IContainer
{
    public bool isEntityInContainer(Vector2 pos)
    {
        return true;
    }

    public Vector3 GetTargetPosition(Vector2 screenPos)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if(Physics.Raycast(ray, out hit, 100))
        {
            Vector3 directionNormal = ray.direction;
            directionNormal.Normalize();
            return hit.point;// - directionNormal * 1;
        }
        throw new Exception("This should never happen");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
