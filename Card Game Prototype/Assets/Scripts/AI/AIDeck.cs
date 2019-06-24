using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeck : MonoBehaviour
{
    public List<Card> gameDeck;

    public List<Card> currentHand;
    public Card[] viewingDeck;
    public Queue<Card> currentDeck;

    // Start is called before the first frame update
    void Start()
    {
        currentDeck = new Queue<Card>();

        foreach (Card card in gameDeck)
            currentDeck.Enqueue(card);

        viewingDeck = currentDeck.ToArray();
    }

    public void DealNewHand(int handSize)
    {
        // Number of cards that weren't played
        Card[] deckArray = currentHand.ToArray();

        // Deal left over cards back into deck
        for (int i = 0; i < deckArray.Length; i++)
            currentDeck.Enqueue(deckArray[i]);

        currentHand.Clear();

        // Deal cards up to hand size
        for (int i = 0; i < handSize; i++)
        {
            if (currentDeck.Count > 0)
            {
                currentHand.Add(currentDeck.Dequeue());
            }
        }

        // Display Deck (Can't serialize a queue... or it's nota good idea... idk)
        viewingDeck = currentDeck.ToArray();
        //Debug.Log("New AI hand has been dealt");
    }
}
