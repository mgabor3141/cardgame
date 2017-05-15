using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : Surface
{
    public int size = 100;
    public bool generateFull = true;

    void Start()
    {
        if (generateFull)
        {
            CreateCard("10_of_clubs");
            CreateCard("10_of_diamonds");
            CreateCard("10_of_hearts");
            CreateCard("10_of_spades");
            CreateCard("2_of_clubs");
            CreateCard("2_of_diamonds");
            CreateCard("2_of_hearts");
            CreateCard("2_of_spades");
            CreateCard("3_of_clubs");
            CreateCard("3_of_diamonds");
            CreateCard("3_of_hearts");
            CreateCard("3_of_spades");
            CreateCard("4_of_clubs");
            CreateCard("4_of_diamonds");
            CreateCard("4_of_hearts");
            CreateCard("4_of_spades");
            CreateCard("5_of_clubs");
            CreateCard("5_of_diamonds");
            CreateCard("5_of_hearts");
            CreateCard("5_of_spades");
            CreateCard("6_of_clubs");
            CreateCard("6_of_diamonds");
            CreateCard("6_of_hearts");
            CreateCard("6_of_spades");
            CreateCard("7_of_clubs");
            CreateCard("7_of_diamonds");
            CreateCard("7_of_hearts");
            CreateCard("7_of_spades");
            CreateCard("8_of_clubs");
            CreateCard("8_of_diamonds");
            CreateCard("8_of_hearts");
            CreateCard("8_of_spades");
            CreateCard("9_of_clubs");
            CreateCard("9_of_diamonds");
            CreateCard("9_of_hearts");
            CreateCard("9_of_spades");
            CreateCard("ace_of_clubs");
            CreateCard("ace_of_diamonds");
            CreateCard("ace_of_hearts");
            CreateCard("ace_of_spades");
            CreateCard("ace_of_spades2");
            CreateCard("black_joker");
            CreateCard("jack_of_clubs");
            CreateCard("jack_of_clubs2");
            CreateCard("jack_of_diamonds");
            CreateCard("jack_of_diamonds2");
            CreateCard("jack_of_hearts");
            CreateCard("jack_of_hearts2");
            CreateCard("jack_of_spades");
            CreateCard("jack_of_spades2");
            CreateCard("king_of_clubs");
            CreateCard("king_of_clubs2");
            CreateCard("king_of_diamonds");
            CreateCard("king_of_diamonds2");
            CreateCard("king_of_hearts");
            CreateCard("king_of_hearts2");
            CreateCard("king_of_spades");
            CreateCard("king_of_spades2");
            CreateCard("queen_of_clubs");
            CreateCard("queen_of_clubs2");
            CreateCard("queen_of_diamonds");
            CreateCard("queen_of_diamonds2");
            CreateCard("queen_of_hearts");
            CreateCard("queen_of_hearts2");
            CreateCard("queen_of_spades");
            CreateCard("queen_of_spades2");
            CreateCard("red_joker");

            Shuffle();
            Rearrange();
        }
    }

    private void CreateCard(string cardname)
    {
        GameObject newcard = Instantiate(Resources.Load<GameObject>("Prefabs/Card"), transform.position, Quaternion.identity);
        newcard.GetComponent<Entity>().Initialize(false, this, Resources.Load<Texture>("Textures/" + cardname), Resources.Load<Texture>("Textures/back-blue"));
        entities.Add(newcard.GetComponent<Entity>());
    }
    
    public void Shuffle()
    {
        // Fisher-Yates shuffle
        int n = entities.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Entity value = entities[k];
            entities[k] = entities[n];
            entities[n] = value;
        }
    }

    protected override bool CanTakeEntity()
    {
        return entities.Count < size;
    }

    protected override void PlaceNewEntity(Entity entity, Vector3 hitPos)
    {
        entities.Add(entity);

        Rearrange();
    }

    protected override void RemoveEntity(Entity entity)
    {
        entities.Remove(entity);
        Rearrange();
    }

    private void Rearrange()
    {
        foreach (Entity e in entities)
        {
            e.FlyTo(new Vector3(transform.position.x, transform.position.y + 0.005f * (1f + entities.IndexOf(e)), transform.position.z));
        }
    }
}
