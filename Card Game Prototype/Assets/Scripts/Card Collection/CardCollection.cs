using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EnumTypes;
using System.Linq;

public class CardCollection : MonoBehaviour
{
    public int defaultNumberOfBasicCards = 0; //how many cards the player starts with (so they dont get included into things)

    public static CardCollection Instance;
    public Dictionary<string, Card> allCardsDictionary = new Dictionary<string, Card>();
    public Dictionary<int, Card> allCardIDDictionary = new Dictionary<int, Card>();
    [HideInInspector] public List<Card> commonCards = new List<Card>();
    [HideInInspector] public List<Card> unCommonCards = new List<Card>();
    [HideInInspector] public List<Card> rareCards = new List<Card>();

    public Dictionary<Card, int> quantityOfEachCard = new Dictionary<Card, int>();

    public CardLibrary cardLibrary;

    public float chanceOfRare;
    public float chanceOfUncommon;

    private void Start()
    {
        Instance = this;

        //Debug.Log(cardLibrary.cardLibrary.Count + " cards were loaded into the AllCardsArray");
        LoadCardLibrary();
    }

    public Card GetRandomRarity (Rarity rarity)
    {
        Card chosenCard = null;
        Card[] cardArray;
        int index;

        Random.InitState(System.DateTime.Now.Millisecond);
        switch (rarity)
        {
            case Rarity.None:
                cardArray = commonCards.ToArray();
                break;
            case Rarity.Common:
                cardArray = commonCards.ToArray();
                break;
            case Rarity.Uncommon:
                cardArray = unCommonCards.ToArray();
                break;
            case Rarity.Rare:
                cardArray = rareCards.ToArray();
                break;
            default:
                cardArray = null;
                break;
        }

        if (cardArray == null)
            return null;

        index = Mathf.FloorToInt(Random.Range(0, cardArray.Length));
        chosenCard = cardArray[index];

        return chosenCard;
    }

    public Card GetRandomCard ()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float chance = Random.Range(0, 100);
        Rarity newRarity = Rarity.None;
        if (chance > (100 - chanceOfRare))
            newRarity = Rarity.Rare;
        else if (chance > (100 - (chanceOfUncommon + chanceOfRare)))
            newRarity = Rarity.Uncommon;
        else
            newRarity = Rarity.Common;

        return GetRandomRarity(newRarity);
    }

    public void LoadCardLibrary()
    {
        foreach (Card ca in cardLibrary.cardLibrary)
        {
            if (!allCardsDictionary.ContainsKey(ca.cardName))
            {
                allCardsDictionary.Add(ca.cardName, ca);
            }

            if (!allCardIDDictionary.ContainsKey(ca.iD))
            {
                allCardIDDictionary.Add(ca.iD, ca);
            }

            switch (ca.rarity)
            {
                case Rarity.None:
                    commonCards.Add(ca);
                    break;
                case Rarity.Common:
                    commonCards.Add(ca);
                    break;
                case Rarity.Uncommon:
                    unCommonCards.Add(ca);
                    break;
                case Rarity.Rare:
                    rareCards.Add(ca);
                    break;
                default:
                    Debug.LogError("Unknown Rarity");
                    break;
            }
        }

        LoadQuantityOfCardsFromPlayerPrefs();
    }

    private void LoadQuantityOfCardsFromPlayerPrefs()
    {
        foreach (Card ca in cardLibrary.cardLibrary)
        {
            if (ca.rarity == Rarity.None)
            {
                quantityOfEachCard.Add(ca, defaultNumberOfBasicCards);
            }
            else if (PlayerPrefs.HasKey("NumberOf" + ca.cardName))
            {
                quantityOfEachCard.Add(ca, PlayerPrefs.GetInt("NumberOf" + ca.cardName));
            }
            else
            {
                quantityOfEachCard.Add(ca, 0);
            }
        }
    }

    private void SaveQuantityOfCardsInPlayerPrefs()
    {
        foreach (Card ca in cardLibrary.cardLibrary)
        {
            if (ca.rarity == Rarity.None)
            {
                PlayerPrefs.SetInt("NumberOf" + ca.cardName, defaultNumberOfBasicCards);
            }
            else
            {
                PlayerPrefs.SetInt("NumberOf" + ca.cardName, quantityOfEachCard[ca]);
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveQuantityOfCardsInPlayerPrefs();
    }

    public Card GetCardAssetsByName(string name)
    {
        if (allCardsDictionary.ContainsKey(name))
        {
            return allCardsDictionary[name];
        }
        else
        {
            return null;
        }
    }

    public List<Card>GetCards(bool showingCardsPlayerDoesNotOwn = false, //show or hide cards the player does not own
        bool includeAllTypes = true, //show or hide cards based off their typing
        bool includeAllRarities = true,//show or hide cards based off their rarity
        string keyword = "", //show or hide cards based off a searched keyword
        CardType cardTypeCollection = CardType.Ability, //filter var for specific card types
        Rarity cardRarityCollection = Rarity.Common)//filter var for specific card rarities
    {
        var cards = from card in cardLibrary.cardLibrary select card;

        if (!showingCardsPlayerDoesNotOwn)
        {
            cards = cards.Where(card => quantityOfEachCard[card] > 0);
        }
        if (!includeAllTypes)
        {
            cards = cards.Where(card => card.currentCardType == cardTypeCollection);
        }
        if (!includeAllRarities)
        {
            cards = cards.Where(card => card.rarity == cardRarityCollection);
        }
        if (keyword != null && keyword != "")
        {
            cards = cards.Where(card => (card.name.ToLower().Contains(keyword.ToLower()) ||
            (card.description.ToLower().Contains(keyword.ToLower()) && !keyword.ToLower().Contains(" "))));
        }

        var returnList = cards.ToList<Card>();
        returnList.Sort();

        return returnList;
    }

    public List<Card>GetCardsWithRarity(Rarity rarity)
    {
        return GetCards(true, true, false, "", CardType.None, rarity);
    }

}