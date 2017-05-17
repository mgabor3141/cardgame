using UnityEngine;
using UnityEngine.Networking;

public abstract class Entity : NetworkBehaviour
{
    public virtual void Click(Vector3 hitPos) { }

    public virtual Entity DragTarget(Vector3 hitPos) { return null; }
    public virtual void  StartDrag(Vector3 hitPos) { }

    public virtual Entity DelayedDragTarget(Vector3 hitPos) { return null; }
    public virtual void StartDelayedDrag(Vector3 hitPos) { }

    public virtual bool HoverAnswered(Entity entity, Vector3 hitPos) { return false; }
    public virtual void Hover(Entity entity, Vector3 hitPos) { }

    public virtual void HoverOff() { }

    public virtual bool DropAccepted(Entity entity, Vector3 hitPos) { return false; }
    public virtual void Drop(Entity entity, Vector3 hitPos) { }
}
