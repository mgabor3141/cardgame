using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Vector3 targetPosition;
    public Surface surface;

    public abstract void Tap(Vector2 pos);

    public abstract void FlyTo(Vector3 pos);

    public void Start()
    {

    }

    public void Update()
    {
        transform.position = transform.position + (targetPosition - transform.position) * 0.2f;
    }
}
