using UnityEngine;

public class Spot : Entity, IContainer
{
    public bool SpawnWithDeck = false;
    public DeckColor DeckColor = DeckColor.Blue;

    public AutoFacingOptions AutoFacing { get; set; }
    public bool ForceFacing { get; set; }

    public Entity _entity;

    void Start()
    {
        if (SpawnWithDeck)
        {
            GameObject deck = Instantiate(Resources.Load<GameObject>("Prefabs/Deck"), transform.position + new Vector3(0, 0.05f, 0), Quaternion.identity);
            deck.GetComponent<Deck>().Initialize(this, DeckColor);
        }
    }

    public bool AddEntity(Entity entity, Vector3 hitPos)
    {
        if (_entity != null) return false;
        _entity = entity;
        entity.GetComponent<Movement>().Container = this;
        entity.GetComponent<Movement>().TargetPosition = transform.position + new Vector3(0, 0.05f, 0);
        return true;
    }

    public void RemoveEntity(Entity entity)
    {
        entity.GetComponent<Movement>().Container = null;
        _entity = null;
    }

    // Event handlers

    public override bool Hover(Entity entity, Vector3 hitPos)
    {
        if (_entity != null) return false;
        entity.GetComponent<Movement>().TargetPosition = transform.position + new Vector3(0, 1, 0);
        return true;
    }

    public override bool Drop(Entity entity, Vector3 hitPos)
    {
        return AddEntity(entity, hitPos);
    }
}
