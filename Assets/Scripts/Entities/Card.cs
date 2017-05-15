using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : Entity {
    bool facingUp = true;

    public override void Tap(Vector2 pos)
    {
        facingUp = !facingUp;
        Debug.Log("Card flipped");
    }

    public override void FlyTo(Vector3 pos)
    {
        targetPosition = pos;
    }
}
