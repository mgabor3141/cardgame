using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Vector3 targetPosition;
    public Surface surface;

    private bool resting = false;

    // hmmm...
    public abstract void Initialize(bool facingUp, Surface surface, Texture front, Texture back);

    public abstract void Tap(Vector2 pos);

    public virtual void FlyTo(Vector3 pos)
    {
        resting = false;
        targetPosition = pos;
    }

    public void Start()
    {

    }

    public void Update()
    {
        if (resting) return;

        if ((targetPosition - transform.position).magnitude < 0.0005)
        {
            resting = true;
            return;
        }

        transform.position = transform.position + (targetPosition - transform.position) * 0.2f;
    }
}
