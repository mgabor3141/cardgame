using System.Collections.Generic;
using UnityEngine;

public class Hand : Entity, IContainer
{
    public int size = 7;
    private List<Entity> _entities = new List<Entity>();

    public AutoFacingOptions AutoFacing { get; set; }
    public bool ForceFacing { get; set; }

    private float width { get { return GetComponent<Renderer>().bounds.size.x; } }

    private int WorldToHand(float x)
    {
        return Mathf.FloorToInt(((x - transform.position.x) / width + 0.5f) * (_entities.Count + 1));
    }

    private float HandToWorld(int index, int count)
    {
        return transform.position.x + width * ((float)index / count - 0.5f) + width / count / 2;
    }

    public bool AddEntity(Entity entity, Vector3 hitPos)
    {
        if (_entities.Count >= size) return false;

        entity.GetComponent<Movement>().Container = this;

        _entities.Insert(WorldToHand(hitPos.x), entity);

        Rearrange();

        return true;
    }

    public void RemoveEntity(Entity entity)
    {
        _entities.Remove(entity);
        entity.GetComponent<Movement>().Container = null;
        Rearrange();
    }

    private void Rearrange(int hoverAt = -1)
    {
        int count = _entities.Count;
        if (hoverAt != -1)
            count++;

        foreach (Entity e in _entities)
        {
            int index = _entities.IndexOf(e);
            if (hoverAt != -1 && index >= hoverAt)
                index++;

            e.GetComponent<Movement>().TargetPosition =
                new Vector3(HandToWorld(index, count), transform.position.y + 0.1f, transform.position.z);
        }
    }

    // Event handlers

    public override bool Hover(Entity entity, Vector3 hitPos)
    {
        if (_entities.Count >= size) return false;
        Rearrange(WorldToHand(hitPos.x));
        entity.GetComponent<Movement>().TargetPosition =
            new Vector3(HandToWorld(WorldToHand(hitPos.x), _entities.Count + 1),
            transform.position.y + 1, transform.position.z);
        return true;
    }

    public override void HoverOff()
    {
        Rearrange();
    }

    public override bool Drop(Entity entity, Vector3 hitPos)
    {
        return AddEntity(entity, hitPos);
    }
}
