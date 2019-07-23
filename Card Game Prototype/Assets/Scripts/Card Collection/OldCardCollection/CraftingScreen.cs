using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;


[System.Serializable]
public class RarityTradingCost
{
    public Rarity cardRarity;
    public int craftCost;
    public int uncraftCost;
}

public class CraftingScreen : MonoBehaviour
{
    public static CraftingScreen Instance;
    public GameObject Content;

    public GameObject cardPrefab;
    public Text craftText;
    public Text uncraftText;
    public Text quantityText;
    public RarityTradingCost[] tradingCostsArray;

    public bool Visible { get { return Content.activeInHierarchy; } }

    private Card currentCard;
    private Dictionary<Rarity, RarityTradingCost> tradingCosts = new Dictionary<Rarity, RarityTradingCost>();

    private void Awake()
    {
        Instance = this;
        foreach(RarityTradingCost rtc in tradingCostsArray)
        {
            tradingCosts.Add(rtc.cardRarity, rtc);
        }
    }

    public void ShowCraftingScreen(Card cardToShow)
    {
        currentCard = cardToShow;
        GameObject cardObject = cardPrefab;
        cardPrefab.SetActive(true);

        Card manager = cardObject.GetComponent<Card>();
        manager = cardToShow;

        craftText.text = "Craft this card for " + tradingCosts[cardToShow.rarity].craftCost.ToString() + " dust";
        uncraftText.text = "Break this card down for " + tradingCosts[cardToShow.rarity].uncraftCost.ToString() + " dust";

        ShopManager.instance.dustHud.SetActive(true);
        UpdateQuantityOfCurrentCard();
        Content.SetActive(true);
    }

    public void UpdateQuantityOfCurrentCard()
    {
        int amountOfThisCardInYourCollection = CardCollection.Instance.quantityOfEachCard[currentCard];
        quantityText.text = "You have " + amountOfThisCardInYourCollection.ToString() + " of these";
        DeckBuildingScreen.Instance.collectionBrowserScript.UpdatePage();
    }

    public void HideCraftingScreen()
    {
        ShopManager.instance.dustHud.SetActive(false);
        Content.SetActive(false);
    }

    public void CraftCurrentCard()
    {
        if (currentCard.rarity!= Rarity.None)
        {
            if (ShopManager.instance.Dust>=tradingCosts[currentCard.rarity].craftCost)
            {
                ShopManager.instance.Dust -= tradingCosts[currentCard.rarity].craftCost;
                CardCollection.Instance.quantityOfEachCard[currentCard]++;
                UpdateQuantityOfCurrentCard();
            }
        }
    }

    public void UncraftCurrentCard()
    {
        if (currentCard.rarity!=Rarity.None)
        {
            if (CardCollection.Instance.quantityOfEachCard[currentCard]>0)
            {
                CardCollection.Instance.quantityOfEachCard[currentCard]--;
                ShopManager.instance.Dust += tradingCosts[currentCard.rarity].uncraftCost;
                UpdateQuantityOfCurrentCard();

                foreach(DeckInfo di in DecksStorage.Instance.allDecks)
                {
                    while (di.NumberOfThisCardInDeck(currentCard) > CardCollection.Instance.quantityOfEachCard[currentCard])
                    {
                        di.deckCards.Remove(currentCard);
                    }
                }
            }
        }
    }
}
