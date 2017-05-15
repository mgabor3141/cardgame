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
        
    void Update () {
        // MOUSE DOWN
        if (Input.GetMouseButtonDown(0))
        {
            elapsedTime = 0;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 9; // Entity layer
            if (Physics.Raycast(ray, out hit, 100, layerMask))
                heldEntity = hit.collider.GetComponentInParent<Entity>();
        }

        // MOUSE HELD
        if (Input.GetMouseButton(0) && heldEntity)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > 0.2) {
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

            if (elapsedTime < 0.2)
                heldEntity.Tap(Input.mousePosition);

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 8; // Surface layer
            if (Physics.Raycast(ray, out hit, 100, layerMask))
                hit.collider.GetComponentInParent<Surface>().InsertEntity(heldEntity, hit.point);

            heldEntity = null;
        }
    }
}
