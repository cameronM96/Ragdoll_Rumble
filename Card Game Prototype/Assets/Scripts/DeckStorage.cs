using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckInfo
{
    public string deckName;
    public List<Card> deckCards;

    public DeckInfo(List<Card> cards, string name)
    {
        deckCards = new List<Card>(cards);
        deckName = name;
    }

    public bool IsComplete()
    {
        return (DeckBuildingScreen.Instance.builderScript.amountOfCardsInDeck == deckCards.Count);
    }

    public int NumberOfThisCardInDeck (Card asset)
    {
        int count = 0;
        foreach(Card ca in deckCards)
        {
            if (ca == asset)
            {
                count++;
            }
        }
        return count;
    }
}

public class DecksStorage : MonoBehaviour
{
    public static DecksStorage Instance;
    public List<DeckInfo> allDecks { get; set; }

    private bool alreadyLoadedDecks = false;

    private void Awake()
    {
        allDecks = new List<DeckInfo>();
        Instance = this;
    }

    private void Start()
    {
        if (!alreadyLoadedDecks)
        {
            LoadDecksFromPlayerPrefs();
            alreadyLoadedDecks = true;
        }
    }

    void LoadDecksFromPlayerPrefs()
    {
        List<DeckInfo> decksFound = new List<DeckInfo>();
        //9 is default for max decks, thus used in the for loop, can change later if needed
        for (int i = 0; i < 9; i++)
        {
            string deckListKey = "Deck" + i.ToString();
            string deckNameKey = "DeckName" + i.ToString();
            string[] deckAsCardNames = PlayerPrefsX.GetStringArray(deckListKey);

            Debug.Log("Has DeckName Key " + PlayerPrefs.HasKey(deckNameKey) + " and Length of DeckAsCardNames " + deckAsCardNames.Length);

            if (deckAsCardNames.Length > 0 && PlayerPrefs.HasKey(deckNameKey))
            {
                string deckName = PlayerPrefs.GetString(deckNameKey);

                List<Card> deckList = new List<Card>();
                foreach (string name in deckAsCardNames)
                {
                    deckList.Add(CardCollection.Instance.GetCardAssetsByName(name));
                }

                decksFound.Add(new DeckInfo(deckList, deckName));
            }
        }

        allDecks = decksFound;
    }

    public void SaveDecksIntoPlayerPrefs()
    {
        for (int i = 0; i < 9; i++)
        {
            string deckNameKey = "DeckName" + i.ToString();

            if (PlayerPrefs.HasKey(deckNameKey))
            {
                PlayerPrefs.DeleteKey(deckNameKey);
            }
        }

        for (int i = 0; i < allDecks.Count; i++)
        {
            string deckListKey = "Deck" + i.ToString();
            string deckNameKey = "DeckName" + i.ToString();

            List<string> cardNamesList = new List<string>();
            foreach (Card ca in allDecks[i].deckCards)
            {
                cardNamesList.Add(ca.name);
            }

            string[] cardNamesArray = cardNamesList.ToArray();
            PlayerPrefsX.SetStringArray(deckListKey, cardNamesArray);
            PlayerPrefs.SetString(deckNameKey, allDecks[i].deckName);
        }
    }

    private void OnApplicationQuit()
    {
        SaveDecksIntoPlayerPrefs();
    }
}