using UnityEngine;
using UnityEngine.Networking;

public class Card : Entity, IFlippable
{
    [SyncVar]
    private string _frontTextureName, _backTextureName;

    [SyncVar]
    private bool _facingUp;
    public bool FacingUp
    {
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

    [ClientRpc]
    public void RpcSetParent(NetworkInstanceId parent)
    {
        if (parent.IsEmpty())
            transform.parent = null;
        else
            transform.parent = ClientScene.FindLocalObject(parent).transform;
    }

    [ClientRpc]
    public void RpcSetLayer(int layer)
    {
        gameObject.layer = layer;
    }

    [ClientRpc]
    public void RpcInitialize(bool facingUp, NetworkInstanceId container, string frontTextureName, string backTextureName)
    {
        GetComponent<Movement>().ContainerID = container;
        _frontTextureName = frontTextureName;
        _backTextureName = backTextureName;
        FacingUp = facingUp;
    }

    [ClientRpc]
    public void RpcFlip()
    {
        FacingUp = !FacingUp;

        transform.position = transform.position + new Vector3(0, 0.2f, 0);
        GetComponent<Movement>().Wake();
    }

    public override float GetNetworkSendInterval()
    {
        return 0.02f;
    }

    // Event handlers

    public override void Click(Vector3 hitPos)
    {
        //if (GetComponent<Movement>().Container.ForceFacing == true) return;
        RpcFlip();
    }

    public override Entity DragTarget(Vector3 hitPos)
    {
        return this;
    }

    public override void StartDrag(Vector3 hitPos)
    {
        GetComponent<Movement>().Container.RemoveEntity(this);
    }
}
