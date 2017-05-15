using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
    public List<IContainer> containers = new List<IContainer>();
    public List<Entity> entities;

    private void SetContainer(Entity heldEntity, Vector2 position)
    {
        foreach (IContainer container in containers)
        {
            if (container.isEntityInContainer(position))
            {
                heldEntity.container = container;
                break;
            }

        }
    }

    // Use this for initialization
    void Start () {
        containers.Add(new Table());
	}

    float elapsedTime;
    Entity heldEntity;
        
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            elapsedTime = 0;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 9; // Entity layer
            if (Physics.Raycast(ray, out hit, 100, layerMask))
                heldEntity = hit.collider.GetComponentInParent<Entity>();
            Debug.Log(heldEntity);
        }

        if (Input.GetMouseButton(0) && heldEntity)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > 0.1)
                heldEntity.Drag(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && heldEntity)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > 0.1)
                heldEntity.Drag(Input.mousePosition);
            else
                heldEntity.Tap(Input.mousePosition);

            SetContainer(heldEntity, Input.mousePosition);

            heldEntity = null;
        }
    }
}
