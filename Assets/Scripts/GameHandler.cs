using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
    public List<Entity> entities;
    
    void Start () {

	}

    float elapsedTime;
    Entity heldEntity;

    Vector3 startingPosition; // World space
    Vector3 startingMousePosition; // Screen space
    Surface startingSurface; // in case of failiure, we put it back here
        
    void Update () {
        // MOUSE DOWN
        if (Input.GetMouseButtonDown(0))
        {
            elapsedTime = 0;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 9; // Entity layer
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                heldEntity = hit.collider.GetComponent<Entity>();

                startingPosition = heldEntity.targetPosition;
                startingMousePosition = Input.mousePosition;
            }
        }

        // MOUSE HELD
        if (Input.GetMouseButton(0) && heldEntity)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > 0.2 || (Input.mousePosition - startingMousePosition).magnitude > 10) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMask = 1 << 8; // Surface layer
                if (Physics.Raycast(ray, out hit, 100, layerMask))
                    heldEntity.FlyTo(hit.point + new Vector3(0, 1, 0));
            }
        }

        // MOUSE RELEASED
        if (Input.GetMouseButtonUp(0) && heldEntity)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < 0.2 && (Input.mousePosition - startingMousePosition).magnitude < 10)
            {
                heldEntity.Tap(Input.mousePosition);
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMask = 1 << 8; // Surface layer
                if (Physics.Raycast(ray, out hit, 100, layerMask))
                    if (!hit.collider.GetComponentInParent<Surface>().InsertEntity(heldEntity, hit.point))
                        heldEntity.targetPosition = startingPosition;
            }
            heldEntity = null;
        }
    }
}
