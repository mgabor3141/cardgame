using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum DeckColor { Blue, Red };

public class Deck : Entity, IFlippable, IContainer
{
    public List<Card> cards = new List<Card>();

    public bool FacingUp
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            foreach (Card card in cards)
            {
                card.FacingUp = value;
            }
        }
    }

    public AutoFacingOptions AutoFacing { get; set; }
    public bool ForceFacing { get; set; }

    public void Initialize(IContainer container, DeckColor deckColor)
    {
        GetComponent<Movement>().Container = container;

        string color;

        switch (deckColor)
        {
            case DeckColor.Blue:
                color = "back_blue";
                break;
            case DeckColor.Red:
                color = "back_red";
                break;
            default:
                color = "back_blue";
                break;
        }

        CmdCreateCard("10_of_clubs", color);
        CmdCreateCard("10_of_diamonds", color);
        CmdCreateCard("10_of_hearts", color);
        CmdCreateCard("10_of_spades", color);
        CmdCreateCard("2_of_clubs", color);
        CmdCreateCard("2_of_diamonds", color);
        CmdCreateCard("2_of_hearts", color);
        CmdCreateCard("2_of_spades", color);
        CmdCreateCard("3_of_clubs", color);
        CmdCreateCard("3_of_diamonds", color);
        CmdCreateCard("3_of_hearts", color);
        CmdCreateCard("3_of_spades", color);
        CmdCreateCard("4_of_clubs", color);
        CmdCreateCard("4_of_diamonds", color);
        CmdCreateCard("4_of_hearts", color);
        CmdCreateCard("4_of_spades", color);
        CmdCreateCard("5_of_clubs", color);
        CmdCreateCard("5_of_diamonds", color);
        CmdCreateCard("5_of_hearts", color);
        CmdCreateCard("5_of_spades", color);
        CmdCreateCard("6_of_clubs", color);
        CmdCreateCard("6_of_diamonds", color);
        CmdCreateCard("6_of_hearts", color);
        CmdCreateCard("6_of_spades", color);
        CmdCreateCard("7_of_clubs", color);
        CmdCreateCard("7_of_diamonds", color);
        CmdCreateCard("7_of_hearts", color);
        CmdCreateCard("7_of_spades", color);
        CmdCreateCard("8_of_clubs", color);
        CmdCreateCard("8_of_diamonds", color);
        CmdCreateCard("8_of_hearts", color);
        CmdCreateCard("8_of_spades", color);
        CmdCreateCard("9_of_clubs", color);
        CmdCreateCard("9_of_diamonds", color);
        CmdCreateCard("9_of_hearts", color);
        CmdCreateCard("9_of_spades", color);
        CmdCreateCard("ace_of_clubs", color);
        CmdCreateCard("ace_of_diamonds", color);
        CmdCreateCard("ace_of_hearts", color);
        CmdCreateCard("ace_of_spades", color);
        CmdCreateCard("black_joker", color);
        CmdCreateCard("jack_of_clubs", color);
        CmdCreateCard("jack_of_diamonds", color);
        CmdCreateCard("jack_of_hearts", color);
        CmdCreateCard("jack_of_spades", color);
        CmdCreateCard("king_of_clubs", color);
        CmdCreateCard("king_of_diamonds", color);
        CmdCreateCard("king_of_hearts", color);
        CmdCreateCard("king_of_spades", color);
        CmdCreateCard("queen_of_clubs", color);
        CmdCreateCard("queen_of_diamonds", color);
        CmdCreateCard("queen_of_hearts", color);
        CmdCreateCard("queen_of_spades", color);
        CmdCreateCard("red_joker", color);

        Shuffle();
        UpdateDeck();
    }

    [Command]
    private void CmdCreateCard(string cardname, string color)
    {
        GameObject newcard = Instantiate(Resources.Load<GameObject>("Prefabs/Card"), transform.position, Quaternion.identity);
        newcard.layer = 9;
        newcard.transform.parent = this.transform;
        newcard.GetComponent<Card>().Initialize(false, this, cardname, color);
        NetworkServer.SpawnWithClientAuthority(newcard, GameObject.Find("NetworkManager").GetComponent<NetworkManager>().client.connection.playerControllers[0].gameObject);
        cards.Add(newcard.GetComponent<Card>());
    }

    public void Shuffle()
    {
        // Fisher-Yates shuffle
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
        UpdateDeck();
    }

    public bool AddEntity(Entity entity, Vector3 hitPos)
    {
        if (entity.GetType() != typeof(Card))
            return false;

        Card card = (Card)entity;
        card.GetComponent<Movement>().Container = this;
        cards.Add(card);
        UpdateDeck();
        return true;
    }

    public void RemoveEntity(Entity entity)
    {
        Card card = (Card)entity;

        card.GetComponent<Movement>().Container = null;

        cards.Remove(card);

        UpdateDeck();
    }

    private void UpdateDeck()
    {
        if (cards.Count >= 2)
        {
            cards[cards.Count - 2].gameObject.SetActive(true);
            cards[cards.Count - 2].GetComponent<Movement>().TargetPosition = transform.position + new Vector3(0, 0.005f * cards.Count / 2, 0);
        }

        if (cards.Count >= 1)
        {
            cards[0].gameObject.SetActive(true);
            cards[0].GetComponent<Movement>().TargetPosition = transform.position + new Vector3(0, 0.005f, 0);
        }

        if (cards.Count >= 1)
        {
            cards[cards.Count - 1].gameObject.SetActive(true);
            cards[cards.Count - 1].GetComponent<Movement>().TargetPosition = transform.position + new Vector3(0, 0.005f * cards.Count, 0);
        }
    }

    // Event handlers

    public override void Click(Vector3 hitPos)
    {
        Shuffle();
    }

    public override bool Drop(Entity entity, Vector3 hitPos)
    {
        entity.gameObject.layer = 9;
        entity.transform.parent = this.transform;
        return AddEntity(entity, hitPos);
    }

    public override Entity StartDelayedDrag(Vector3 hitPos)
    {
        GetComponent<Movement>().Container.RemoveEntity(this);
        return this;
    }

    public override Entity StartDrag(Vector3 hitPos)
    {
        Card card = cards[cards.Count - 1];
        cards.RemoveAt(cards.Count - 1);

        card.GetComponent<Movement>().Container = null;
        card.transform.parent = null;

        UpdateDeck();

        return (Entity)card;
    }
}
