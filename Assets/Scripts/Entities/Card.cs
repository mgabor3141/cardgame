using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : Entity {
    Texture front, back;
    private bool _facingUp;
    public bool facingUp {
        get
        {
            return _facingUp;
        }

        set
        {
            _facingUp = value;
            if (facingUp)
                GetComponent<Renderer>().material.mainTexture = front;
            else
                GetComponent<Renderer>().material.mainTexture = back;
        }
    }

    public override void Initialize(bool facingUp, Surface surface, Texture front, Texture back)
    {
        this.surface = surface;
        this.front = front;
        this.back = back;
        this.facingUp = facingUp;
    }

    public override void Tap(Vector2 pos)
    {
        if (surface.ForceFacing != Surface.ForceFacingOptions.None) return;
        facingUp = !facingUp;

        transform.position = transform.position + new Vector3(0, 0.2f, 0);
    }
}
