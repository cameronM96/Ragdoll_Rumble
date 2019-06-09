using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> cards;
    //[HideInInspector]
    public List<GameObject> deckOfCards;

    private void Start()
    {
        deckOfCards = new List<GameObject>();

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
