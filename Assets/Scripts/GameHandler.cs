using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
    public List<IContainer> container;
    public List<Entity> entity;
    
	// Use this for initialization
	void Start () {
		
	}

    float elapsedTime;
    Entity heldEntity;
        
    void Update () {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    elapsedTime = 0;
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out hit, 100))
                        heldEntity = hit.collider.GetComponentInParent<Entity>();
                    Debug.Log(heldEntity);
                    break;

                case TouchPhase.Stationary:
                    elapsedTime += touch.deltaTime;
                    break;

                case TouchPhase.Moved:
                    if (elapsedTime > 100)
                        Debug.Log("IDK");
                    break;
                    
                case TouchPhase.Ended:
                    if (elapsedTime < 100)
                        // tap happened
                        Debug.Log("Tap");
                    break;
            }
        }
    }
}
