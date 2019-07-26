using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection_Card : MonoBehaviour
{
    public CollectionManager collectionManager;
    public GameObject cardDisabled;
    public GameObject counterDisplay;
    public Text cardCount;
    public int currentCardCount;

    // Start is called before the first frame update
    void Start()
    {
        collectionManager = transform.root.GetComponent<CollectionManager>();
    }

    public void UpdateCardCount (int count)
    {
        currentCardCount = count;
        if (currentCardCount < 0)
            currentCardCount = 0;

        if (count > 1)
        {
            // Display number
            cardCount.text = "x" + count;
            counterDisplay.SetActive(true);
            cardDisabled.SetActive(false);
        }
        else if (count == 1)
        {
            // Hide number
            cardCount.text = "x" + count;
            counterDisplay.SetActive(false);
            cardDisabled.SetActive(false);
        }
        else
        {
            // Gray out card
            cardCount.text = "0";
            counterDisplay.SetActive(false);
            cardDisabled.SetActive(true);
        }
    }

    public void CardSelected ()
    {
        if (!cardDisabled.activeSelf)
        {
            if (collectionManager.creatingDeck)
            {
                UpdateCardCount(--currentCardCount);
                GameObject deckCard = Instantiate(collectionManager.deckBuildingCardTemplate);
                deckCard.GetComponent<CardDisplay>().Initialise(GetComponent<CardDisplay>().card);
                deckCard.GetComponent<DeckCard_Draggable>().collectionCard = this;
            }
            else
                collectionManager.OpenCardWindow(transform.GetComponentInParent<CardDisplay>().card);
        }
        // else card is disabled (you don't have any left to pull from)
    }
}
