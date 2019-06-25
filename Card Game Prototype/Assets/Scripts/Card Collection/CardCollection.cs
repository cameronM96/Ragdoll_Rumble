﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using System.Linq;

public class CardCollection : MonoBehaviour
{
    public int defaultNumberOfBasicCards = 0; //how many cards the player starts with (so they dont get included into things)

    public static CardCollection Instance;
    private Dictionary<string, Card> allCardsDictionary = new Dictionary<string, Card>();

    public Dictionary<Card, int> quantityOfEachCard = new Dictionary<Card, int>();

    private Card[] allCardsArray;

    private void Awake()
    {
        Instance = this;

        allCardsArray = Resources.LoadAll<Card>("");
        Debug.Log(allCardsArray.Length + " cards were loaded into the AllCardsArray");
        foreach (Card ca in allCardsArray)
        {
            if (!allCardsDictionary.ContainsKey(ca.name))
            {
                allCardsDictionary.Add(ca.name, ca);
            }
        }
        LoadQuantityOfCardsFromPlayerPrefs();
    }

    private void LoadQuantityOfCardsFromPlayerPrefs()
    {
        foreach (Card ca in allCardsArray)
        {
            if (ca.rarity == Rarity.None)
            {
                quantityOfEachCard.Add(ca, defaultNumberOfBasicCards);
            }
            else if (PlayerPrefs.HasKey("NumberOf" + ca.name))
            {
                quantityOfEachCard.Add(ca, PlayerPrefs.GetInt("NumberOf" + ca.name));
            }
            else
            {
                quantityOfEachCard.Add(ca, 0);
            }
        }
    }

    private void SaveQuantityOfCardsInPlayerPrefs()
    {
        foreach (Card ca in allCardsArray)
        {
            if (ca.rarity == Rarity.None)
            {
                PlayerPrefs.SetInt("NumberOf" + ca.name, defaultNumberOfBasicCards);
            }
            else
            {
                PlayerPrefs.SetInt("NumberOf" + ca.name, quantityOfEachCard[ca]);
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
        var cards = from card in allCardsArray select card;

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