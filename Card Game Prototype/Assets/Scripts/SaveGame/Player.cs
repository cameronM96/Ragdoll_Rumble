using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public string playerName;
    private int coins;
    private int gems;
    private Dictionary<int, int> myCards;
    private Dictionary<string, int[]> myDecks;
    private Dictionary<int, int> myUnopenedPacks;
    private int campaignProgress;
    private string defaultDeckName;

    public List<Card> starterDeck;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadGame();
    }

    public int Coins
    {
        get { return coins; }
    }
    public int Gems
    {
        get { return gems; }
    }
    public Dictionary<int, int> MyCards
    {
        get { return myCards; }
    }
    public Dictionary<string, int[]> MyDecks
    {
        get { return myDecks; }
    }
    public Dictionary<int, int> MyUnopenedPacks
    {
        get { return myUnopenedPacks; }
    }
    public int CampaignProgress
    {
        get { return campaignProgress; }
    }
    public string DefaultDeckName
    {
        get { return defaultDeckName; }
        set { defaultDeckName = value; }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }

    public void AddCurrency (int value, bool premium)
    {
        if (premium)
            gems += value;
        else
            coins += value;

        Debug.Log("Adding " + value);
        SaveGame();
    }

    public void SpendCurrency(int value, bool premium)
    {
        if (premium)
            gems -= value;
        else
            coins -= value;

        Debug.Log("Spending " + value);
        SaveGame();
    }

    public void AddPack(int id)
    {
        // Add pack
        if (myUnopenedPacks.ContainsKey(id))
        {
            myUnopenedPacks[id] += 1;
        }
        else
            myUnopenedPacks.Add(id, 1);

        Debug.Log("Adding Pack");
        SaveGame();
    }

    public void RemovePack(int id)
    {
        // Remove Pack
        if (myUnopenedPacks.ContainsKey(id))
        {
            if (myUnopenedPacks[id] <= 1)
                myUnopenedPacks.Remove(id);
            else
                myUnopenedPacks[id] -= 1;

            Debug.Log("Removing Pack");
            SaveGame();
        }
        else
            Debug.Log("Could not find Pack");
    }

    public void UpdateCampaignProg(int i)
    {
        if (i > campaignProgress)
        {
            campaignProgress = i;

            SaveGame();
        }
    }

    public void LoadGame()
    {
        PlayerProfile data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            playerName = data.playerName;
            Debug.Log("Loading " + playerName);
            coins = data.coins;
            gems = data.gems;

            if (data.myCards == null)
                myCards = new Dictionary<int, int>();
            else
                myCards = data.myCards;

            if (data.myDecks == null)
                myDecks = new Dictionary<string, int[]>();
            else
                myDecks = data.myDecks;

            if (data.myUnopenedPacks == null)
                myUnopenedPacks = new Dictionary<int, int>();
            else
                myUnopenedPacks = data.myUnopenedPacks;

            campaignProgress = data.campaignProgress;
            DefaultDeckName = data.defaultDeckName;
        }
        else
            NewProfile("New Player");
    }

    public void AddCardToLibrary (Card card)
    {
        // Add to myCardLibrary
        //Debug.Log(card.cardName);
        if (myCards.ContainsKey(card.iD))
        {
            myCards[card.iD] += 1;
        }
        else
            myCards.Add(card.iD, 1);

        Debug.Log("Adding " + card.cardName);
        SaveGame();
    }

    public void RemoveCardFromLibrary (Card card)
    {
        // Remove from myCardLibrary
        if (myCards.ContainsKey(card.iD))
        {
            if (myCards[card.iD] <= 1)
                myCards.Remove(card.iD);
            else
                myCards[card.iD] -= 1;

            Debug.Log("Removing " + card.cardName);
            SaveGame();
        }
        else
            Debug.Log("Could not find card in library");
    }

    public void AddDeck (string name, int[] deck)
    {
        myDecks.Add(name, deck);

        Debug.Log("Adding " + name + " to my decks");
        SaveGame();
    }

    public void UpdateDeck(string name, int[] deck)
    {
        if (myDecks.ContainsKey(name))
        {
            myDecks[name] = deck;

            Debug.Log("Updating " + name);
            SaveGame();
        }
        else
            Debug.Log("Could not find this deck");
    }

    public void RemoveDeck(string name)
    {
        if (myDecks.ContainsKey(name))
        {
            myDecks.Remove(name);

            Debug.Log("Removing " + name + " from my decks");
            SaveGame();
        }
        else
            Debug.Log("Could not find this deck");
    }

    public void NewProfile (string name)
    {
        playerName = name;
        coins = 10000;
        gems = 10000;
        campaignProgress = 0;

        myCards = new Dictionary<int, int>();
        myDecks = new Dictionary<string, int[]>();
        myUnopenedPacks = new Dictionary<int, int>();

        int[] idArray = new int[starterDeck.Count];
        for (int i = 0; i < starterDeck.Count; i++)
        {
            // Populate myCardLibrary
            if (myCards.ContainsKey(starterDeck[i].iD))
            {
                ++myCards[starterDeck[i].iD];
            }
            else
                myCards.Add(starterDeck[i].iD, 1);

            // Populate Deck
            idArray[i] = starterDeck[i].iD;
        }

        myDecks.Add("Starter Deck", idArray);

        Debug.Log("Created new profile");
        SaveGame();
    }

    public void PrintValues()
    {
        Debug.Log("Name: " + playerName);
        Debug.Log("Coins: " + coins);
        Debug.Log("Gems: " + gems);

        foreach (KeyValuePair<string, int[]> deckInfo in myDecks)
        {
            //Now you can access the key and value both separately from this attachStat as:
            Debug.Log("Deck name: " + deckInfo.Key);
        }

        foreach (KeyValuePair<int, int> packInfo in myUnopenedPacks)
        {
            //Now you can access the key and value both separately from this attachStat as:
            Debug.Log(packInfo.Value + "x Pack ID: " + packInfo.Key);
        }

        foreach (KeyValuePair<int, int> cardInfo in myCards)
        {
            //Now you can access the key and value both separately from this attachStat as:
            Debug.Log(cardInfo.Value + "x Card ID: " + cardInfo.Key);
        }
    }
}
