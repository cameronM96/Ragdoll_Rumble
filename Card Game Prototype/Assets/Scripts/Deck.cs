using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    Player player;
    public CardCollection cardCollection;

    public List<Card> cards;
    //[HideInInspector]
    public List<GameObject> deckOfCards;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();

        deckOfCards = new List<GameObject>();

        if (player.MyDecks.ContainsKey(LevelHolder.SelectedDeckName))
        {
            cards = new List<Card>();
            // Create all the cards
            int[] cardIDs = player.MyDecks[LevelHolder.SelectedDeckName];
            for (int i = 0; i < cardIDs.Length; i++)
            {
                // Match Id with a card and add it to the current cards list and create it.
                if (cardCollection.allCardIDDictionary.ContainsKey(cardIDs[i]))
                {
                    cards.Add(cardCollection.allCardIDDictionary[cardIDs[i]]);
                }
                else
                    Debug.LogError("Unidentified card!");
            }
        }
        Debug.LogError("Unknown Deck! " + LevelHolder.SelectedDeckName + " could not be found.");

        foreach (Card card in cards)
        {
            if (card != null)
            {
                GameObject newCard = card.CreateCard();
                deckOfCards.Add(newCard);
                newCard.transform.position = this.transform.position;
                newCard.transform.SetParent(this.transform);
            }
        }
    }
}
