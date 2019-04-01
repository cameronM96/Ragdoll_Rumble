using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHand : MonoBehaviour
{
    public GameObject cardSlot;
    public GameObject deck;
    public int handSize;
    public Text numCardsLeft;

    private void Start()
    {
        deck = GameObject.FindGameObjectWithTag("Deck");

        foreach(GameObject card in deck.GetComponent<Deck>().deckOfCards)
        {
            Instantiate(card, deck.transform);
        }
    }

    public void DealNewHand()
    {
        int cardsLeft = cardSlot.transform.childCount;
        for (int i = 0; i < cardsLeft; i++)
        {
            cardSlot.transform.GetChild(0).SetParent(deck.transform);
        }

        for (int i = 0; i < handSize; i++)
        {
            if (deck.transform.childCount != 0)
            {
                deck.transform.GetChild(0).SetParent(cardSlot.transform);
            }
        }

        numCardsLeft.text = "Deck\n" + deck.transform.childCount;
    }
}