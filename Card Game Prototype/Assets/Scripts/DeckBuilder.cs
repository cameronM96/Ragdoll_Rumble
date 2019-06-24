using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class DeckBuilder : MonoBehaviour
{
    public GameObject cardNamePrefab;
    public Transform content;
    public InputField deckName;
    public GameObject DeckCompleteFrame;

    public int sameCardLimit = 2;
    public int amountOfCardsInDeck = 10;

    private List<Card> deckList = new List<Card>();
    private Dictionary<Card, CardNameRibbon> ribbons = new Dictionary<Card, CardNameRibbon>();

    public bool InDeckBuildingMode { get; set; }

    private void Awake()
    {
        DeckCompleteFrame.GetComponent<Image>().raycastTarget = false;
    }
    public void AddCard(Card gameCard)
    {
        if (!InDeckBuildingMode)
        {
            return;
        }
        if (deckList.Count == amountOfCardsInDeck)
        {
            return;
        }

        int count = NumberOfThisCardInDeck(gameCard);
        int limitOfThisCardInDeck = sameCardLimit;

        if (count<limitOfThisCardInDeck)
        {
            deckList.Add(gameCard);

            CheckDeckCompleteFrame();
            count++;

            if (ribbons.ContainsKey(gameCard))
            {
                ribbons[gameCard].SetQuantity(count);
            }
            else
            {
                GameObject cardName = Instantiate(cardNamePrefab, content) as GameObject;
                cardName.transform.SetAsLastSibling();
                cardName.transform.localScale = Vector3.one;
                CardNameRibbon ribbon = cardName.GetComponent<CardNameRibbon>();
                ribbon.ApplyAsset(gameCard, count);
                ribbons.Add(gameCard, ribbon);
            }
        }
    }

    void CheckDeckCompleteFrame()
    {
        DeckCompleteFrame.SetActive(deckList.Count == amountOfCardsInDeck);
    }

    public int NumberOfThisCardInDeck(Card gameCard)
    {
        int count = 0;
        foreach (Card card in deckList)
        {
            if (card == gameCard)
            {
                count++;
            }
        }
        return count;
    }

    public void RemoveCard(Card gameCard)
    {
        CardNameRibbon ribbonToRemove = ribbons[gameCard];
        ribbonToRemove.SetQuantity(ribbonToRemove.Quantity - 1);

        if (NumberOfThisCardInDeck(gameCard)==1)
        {
            ribbons.Remove(gameCard);
            Destroy(ribbonToRemove.gameObject);
        }
        deckList.Remove(gameCard);
        CheckDeckCompleteFrame();
        DeckBuildingScreen.Instance.collectionBrowserScript.UpdateQuantitiesOnPage();
    }

    public void BuildADeck()
    {
        InDeckBuildingMode = true;

        while (deckList.Count>0)
        {
            RemoveCard(deckList[0]);
        }

        DeckBuildingScreen.Instance.collectionBrowserScript.ShowCollectionForDeckBuilding();
        CheckDeckCompleteFrame();
        deckName.text = "";
    }
    public void DoneButtonHandler()
    {
        DeckInfo deckToSave = new DeckInfo(deckList, deckName.text);
        DecksStorage.Instance.allDecks.Add(deckToSave);
        DecksStorage.Instance.SaveDecksIntoPlayerPrefs();

        DeckBuildingScreen.Instance.ShowScreenForCollectionBrowsing();
    }

    private void OnApplicationQuit()
    {
        DoneButtonHandler();
    }
}