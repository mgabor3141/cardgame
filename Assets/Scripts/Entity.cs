using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Vector3 targetPosition;
    public IContainer container;

    public abstract void Tap(Vector2 pos);

    public abstract void Drag(Vector2 pos);
    
    public void Start()
    {
        if (container == null)
            container = new Table();
    }

    public void Update()
    {
        transform.position = transform.position + (targetPosition - transform.position) / 2;
    }
}
