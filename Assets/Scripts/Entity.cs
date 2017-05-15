using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Vector3 targetPosition;
    public IContainer container;

    public abstract void Tap(Vector2 pos);

    public abstract void Drag(Vector2 pos);

    // Update is called once per frame
    public void Update()
    {
        transform.position = (targetPosition - transform.position) / 2;
    }
}
