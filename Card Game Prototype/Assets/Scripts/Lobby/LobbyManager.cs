﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public Player player;
    public CardCollection cardCollection;

    public Dropdown difficulty;
    public Dropdown rounds;

    public GameObject deckButtonsPanel;
    public GameObject deckButtonPrefab;

    public GameObject cardTemplate;
    public GameObject deckPanel;
    public string currentDeckName;
    public Card[] currentDeck;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();

        // Get game info
        difficulty.interactable = (GameInfo.CampaignNumber == -1);
        rounds.interactable = (GameInfo.CampaignNumber == -1);
        difficulty.value = GameInfo.Difficulty;
        rounds.value = GameInfo.Rounds;

        // Load Deck buttons
        LoadDeckButtons();
        if (!(GameInfo.SelectedDeckName == "" || GameInfo.SelectedDeckName == null))
        {
            foreach (Transform child in deckButtonsPanel.transform)
            {
                LobbyDeckButton deckButton = child.gameObject.GetComponent<LobbyDeckButton>();
                if (deckButton.myDeckName == GameInfo.SelectedDeckName)
                {
                    child.gameObject.GetComponent<Button>().Select();
                    deckButton.SelectDeck();
                }
            }
        }
    }

    public void LoadNextLevel ()
    {
        if (!(GameInfo.NextLevelName == "" || GameInfo.NextLevelName == null))
            SceneManager.LoadScene(GameInfo.NextLevelName);
        else if (GameInfo.NextLevelNumb > -1)
            SceneManager.LoadScene(GameInfo.NextLevelNumb);
        else
            Debug.Log("Next level is invalid");
    }

    public void LoadPreviousLevel()
    {
        if (!(GameInfo.ReturnLevelName == "" || GameInfo.ReturnLevelName == null))
            SceneManager.LoadScene(GameInfo.ReturnLevelName);
        else if (GameInfo.ReturnLevelNumb > -1)
            SceneManager.LoadScene(GameInfo.ReturnLevelNumb);
        else
            Debug.Log("Previous level is invalid");
    }

    public void LoadDeckButtons()
    {
        // Destroy old buttons
        foreach (Transform child in deckButtonsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        if (player.MyDecks != null)
        {
            // Create buttons based on decks in player class.
            foreach (KeyValuePair<string, int[]> deck in player.MyDecks)
            {
                GameObject newButton = Instantiate(deckButtonPrefab, deckButtonsPanel.transform);
                newButton.GetComponent<LobbyDeckButton>().Initialise(deck.Key, this);
            }
        }
        else
            Debug.LogError("myDecks returned null!");
    }

    public bool LoadDeck(string deckName)
    {
        // Loads all the cards from the selected deck into the deck window
        currentDeckName = deckName;

        if (player.MyDecks.ContainsKey(deckName))
        {
            // Destroy cards currently in the deck holder
            foreach (Transform child in deckPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // Create all the cards
            int[] cardIDs = player.MyDecks[deckName];
            currentDeck = new Card[cardIDs.Length];
            for (int i = 0; i < cardIDs.Length; i++)
            {
                // Match Id with a card and add it to the current cards list and create it.
                if (cardCollection.allCardIDDictionary.ContainsKey(cardIDs[i]))
                {
                    Card newCard = cardCollection.allCardIDDictionary[cardIDs[i]];
                    currentDeck[i] = newCard;
                }
                else
                    Debug.LogError("Unidentified card!");
            }
        }
        else
        {
            Debug.LogError("Unknown Deck! " + deckName + " could not be found in the deck list");
            return false;
        }

        foreach (Card deckCard in currentDeck)
        {
            GameObject newDeckCard = Instantiate(cardTemplate, deckPanel.transform);
            newDeckCard.GetComponent<CardDisplay>().Initialise(deckCard);
            newDeckCard.GetComponent<DeckCard_Draggable>().enabled = false;
        }

        return true;
    }
}