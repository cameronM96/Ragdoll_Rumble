using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeck : MonoBehaviour
{
    [HideInInspector] public CardCollection cardCollection;

    public List<Card> gameDeck;

    public int requiredDeckSize = 15;

    public List<Card> currentHand;
    public Card[] viewingDeck;
    public Queue<Card> currentDeck;

    private void Start()
    {
        cardCollection = GetComponent<CardCollection>();
    }

    private void OnEnable()
    {
        GameManager.InitialiseTheGame += Initialise;
    }

    private void OnDisable()
    {
        GameManager.InitialiseTheGame -= Initialise;
    }

    public void Initialise()
    {
        gameDeck = new List<Card>();
        Debug.Log(GameInfo.RandomAIDeck);
        // Create all the cards
        if (GameInfo.RandomAIDeck)
        {
            for (int i = 0; i < requiredDeckSize; i++)
            {
                Random.InitState(System.DateTime.Now.Millisecond);
                gameDeck.Add(cardCollection.GetRandomCard());
            }
        }
        else
        {
            Debug.Log(GameInfo.AIDeck);
            Debug.Log(GameInfo.AIDeck.Length);
            int[] cardIDs = GameInfo.AIDeck;
            for (int i = 0; i < cardIDs.Length; i++)
            {
                // Match Id with a card and add it to the current cards list and create it.
                if (cardCollection.allCardIDDictionary.ContainsKey(cardIDs[i]))
                {
                    gameDeck.Add(cardCollection.allCardIDDictionary[cardIDs[i]]);
                }
                else
                    Debug.LogError("Unidentified card!");
            }

            ShuffleDeck();
        }

        currentDeck = new Queue<Card>();

        foreach (Card card in gameDeck)
            currentDeck.Enqueue(card);

        viewingDeck = currentDeck.ToArray();

        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Debug.Log(transform.root.gameObject + " Initialising!");
        gm.InitialsePlayer();
    }

    public void ShuffleDeck()
    {
        Debug.Log("Shuffling AI Deck");
        List<Card> cards = new List<Card>();
        foreach (Card card in gameDeck)
            cards.Add(card);
        gameDeck.Clear();

        for (int i = 0; i < requiredDeckSize; i++)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            Card shuffledCard = cards[Random.Range(0, cards.Count)];
            gameDeck.Add(shuffledCard);
            cards.Remove(shuffledCard);
        }
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
