using UnityEngine;

public class Card : Entity, IFlippable {
    Texture _front, _back;
    private bool _facingUp;
    public bool FacingUp {
        get
        {
            return _facingUp;
        }

        set
        {
            _facingUp = value;
            if (FacingUp)
                GetComponent<Renderer>().material.mainTexture = _front;
            else
                GetComponent<Renderer>().material.mainTexture = _back;
        }
    }

    public void Initialize(bool facingUp, IContainer container, Texture front, Texture back)
    {
        GetComponent<Movement>().Container = container;
        _front = front;
        _back = back;
        FacingUp = facingUp;
    }

    // Event handlers

    public override void Click(Vector3 hitPos)
    {
        if (GetComponent<Movement>().Container.ForceFacing == true) return;
        FacingUp = !FacingUp;

        transform.position = transform.position + new Vector3(0, 0.2f, 0);
        GetComponent<Movement>().Wake();
    }

    public override Entity StartDrag(Vector3 hitPos)
    {
        GetComponent<Movement>().Container.RemoveEntity(this);
        return this;
    }
}
