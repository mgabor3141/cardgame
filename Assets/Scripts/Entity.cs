using UnityEngine;
using UnityEngine.Networking;

public abstract class Entity : NetworkBehaviour
{
    public virtual void Click(Vector3 hitPos) { }
    public virtual Entity StartDrag(Vector3 hitPos) { return null; }
    public virtual Entity StartDelayedDrag(Vector3 hitPos) { return null; }
    public virtual bool Hover(Entity entity, Vector3 hitPos) { return false; }
    public virtual void HoverOff() { }
    public virtual bool Drop(Entity entity, Vector3 hitPos) { return false; }
}
