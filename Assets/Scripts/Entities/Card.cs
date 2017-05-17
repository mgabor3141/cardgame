using UnityEngine;
using UnityEngine.Networking;

public class Card : Entity, IFlippable {
    [SyncVar]
    private string _frontTextureName, _backTextureName;

    [SyncVar]
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
                GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>("Textures/" + _frontTextureName);
            else
                GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>("Textures/" + _backTextureName);
        }
    }

    public void Initialize(bool facingUp, IContainer container, string frontTextureName, string backTextureName)
    {
        GetComponent<Movement>().Container = container;
        _frontTextureName = frontTextureName;
        _backTextureName = backTextureName;
        FacingUp = facingUp;
    }

    public void Move(Vector3 position)
    {
        Debug.Log("Move");
        //transform.position = position;
        CmdMove(position);
    }

    [Command]
    private void CmdMove(Vector3 position)
    {
        Debug.Log("CmdMove");
        transform.position = position;
    }

    // Event handlers

    public override void Click(Vector3 hitPos)
    {
        if (GetComponent<Movement>().Container.ForceFacing == true) return;
        FacingUp = !FacingUp;

        Move(transform.position + new Vector3(0, 0.2f, 0));
        GetComponent<Movement>().Wake();
    }

    public override Entity StartDrag(Vector3 hitPos)
    {
        GetComponent<Movement>().Container.RemoveEntity(this);
        return this;
    }
}
