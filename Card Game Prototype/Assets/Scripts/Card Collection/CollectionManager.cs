using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class CollectionManager : MonoBehaviour
{
    public Player player;

    // Card Stuff
    public GameObject collectionPanel;
    public GameObject collectionCardTemplate;
    public GameObject deckBuildingCardTemplate;
    public bool alphaBetically = true;
    public int rarity = 0; // None, Common, UnCommon, Rare, common -> rare, rare -> common
    public int cardType = 0; // None, Weapon, Armour, Ability, Environmental, Behaviour

    public GameObject cardWindow;

    // Deck Stuff
    public int requiredDeckSize;
    public GameObject deckButtons;
    public GameObject deckCreation;
    public GameObject deckPanel;
    public GameObject deckButtonPrefab;
    [HideInInspector] public bool deckSaved = true;

    // Player Stuff
    public Text playerName;
    public Text coins;
    public Text premium;

    // Pack Stuff
    public GameObject packsDropZone;
    public GameObject packsPanel;
    public GameObject packsTemplate;
    public Vector2 packOffset;

    public List<Card> currentCollection;

    public PackCollection packCollection;
    public CardCollection cardCollection;

    public string currentDeckName;
    public Card[] currentDeck;

    [HideInInspector] public bool creatingDeck;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();

        creatingDeck = true;
        ToggleDeckCreation();
        LoadCards(creatingDeck);
        LoadDeckButtons();
        LoadPacks();
        UpdatePlayerInfo();
    }

    public void UpdatePlayerInfo()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("PlayerProfile")?.GetComponent<Player>();

        if (player == null)
            return;

        playerName.text = player.playerName;
        coins.text = player.Coins.ToString();
        premium.text = player.Gems.ToString();
    }

    public void ToggleDeckCreation()
    {
        creatingDeck = !creatingDeck;

        deckButtons.SetActive(!creatingDeck);
        deckCreation.SetActive(creatingDeck);
    }

    public void LoadPacks()
    {
        // Clear Packs Panel
        foreach (Transform child in packsPanel.transform)
            Destroy(child.gameObject);

        // Clear Drop Zone
        foreach (Transform child in packsDropZone.transform)
            Destroy(child.gameObject);

        // Load all owned packs
        if (packCollection?.packIDDictionary != null && player?.MyUnopenedPacks != null)
        {
            foreach (KeyValuePair<int, int> packs in player.MyUnopenedPacks)
            {
                if (packCollection.packIDDictionary.ContainsKey(packs.Key) && packs.Value > 0)
                {
                    PackOfCards loadedPack = packCollection.packIDDictionary[packs.Key];
                    Transform targetParent = packsPanel.transform;
                    for (int i = 0; i < packs.Value; ++i)
                    {
                        GameObject newPack = Instantiate(packsTemplate, targetParent);
                        newPack.GetComponent<CollectionPack>().Initialise(loadedPack);
                        // Stacks packs ontop of each other
                        newPack.transform.localPosition += new Vector3(packOffset.x, packOffset.y, 0);
                        targetParent = newPack.transform;
                    }
                }
            }
        }
    }

    public void LoadDeckButtons()
    {
        // Destroy old buttons
        List<Transform> unParent = new List<Transform>();
        // Cache children (can't change the list you are looping through so need to cache it)
        foreach (Transform child in deckButtons.transform)
        {
            unParent.Add(child);
        }

        // Unparent and destroy children
        foreach (Transform child in unParent)
        {
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        // Create buttons based on decks in player class.
        foreach (KeyValuePair<string, int[]> deck in player.MyDecks)
        {
            GameObject newButton = Instantiate(deckButtonPrefab, deckButtons.transform);
            newButton.GetComponent<DeckButton>().Initialise(this, deck.Key);
        }

        deckButtons.GetComponent<PanelResizer>().Resize();
    }

    public void CreateDeck(Text textEl)
    {
        CreateDeck(textEl.text);
    }

    public void CreateDeck(string deckName)
    {
        // Create a new deck and add it to player.Mydecks
        currentDeckName = deckName;
        SaveDeck();
        bool loaded = LoadDeck(deckName);
        if (loaded)
        {
            ToggleDeckCreation();
            Debug.Log("Creation of '" + deckName + "' was successful.");
        }
        else
            Debug.Log("Creation of '" + deckName + "' was unsuccessful.");
    }

    public bool LoadDeck(string deckName)
    {
        // Loads all the cards from the selected deck into the deck window
        currentDeckName = deckName;

        if (player.MyDecks.ContainsKey(deckName))
        {
            // Destroy cards currently in the deck holder
            List<Transform> unParent = new List<Transform>();
            // Cache children (can't change the list you are looping through so need to cache it)
            foreach (Transform child in deckPanel.transform)
            {
                unParent.Add(child);
            }

            // Unparent and destroy children
            foreach (Transform child in unParent)
            {
                child.SetParent(null);
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

        foreach(Card deckCard in currentDeck)
        {
            GameObject newDeckCard = Instantiate(deckBuildingCardTemplate, deckPanel.transform);
            newDeckCard.GetComponent<CardDisplay>().Initialise(deckCard);
            newDeckCard.GetComponent<DeckCard_Draggable>().returnParent = deckPanel.transform;
        }

        deckPanel.GetComponent<PanelResizer>().Resize();

        return true;
    }

    public void SaveDeck()
    {
        if (deckPanel.transform.childCount <= requiredDeckSize)
        {
            // Compile Deck
            List<int> deckInts = new List<int>();
            foreach (Transform child in deckPanel.transform)
            {
                if (child.GetComponent<CardDisplay>()?.card != null)
                {
                    if (child.GetComponent<CardDisplay>().card.iD != 0)
                        deckInts.Add(child.GetComponent<CardDisplay>().card.iD);
                }
            }

            // Save deck
            if (player.MyDecks.ContainsKey(currentDeckName))
                player.UpdateDeck(currentDeckName, deckInts.ToArray());
            else
                player.AddDeck(currentDeckName, deckInts.ToArray());

            deckSaved = true;
        }
        else
            Debug.LogError("Deck is too big!");
    }

    public void ClearDeck()
    {
        // Empties Deck (doesn't save it)

        List<Transform> unParent = new List<Transform>();
        // Cache children (can't change the list you are looping through so need to cache it)
        foreach (Transform child in deckPanel.transform)
        {
            unParent.Add(child);
        }

        // Unparent and destroy children
        foreach (Transform child in unParent)
        {
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        deckPanel.GetComponent<PanelResizer>().Resize();
    }

    public void DeleteDeck()
    {
        // Removes deck from player.Mydeck
        player.RemoveDeck(currentDeckName);
        LoadDeckButtons();
        ToggleDeckCreation();
    }

    public void LoadCards(bool deckBuilding)
    {
        // Clear Shop
        currentCollection.Clear();

        List<Transform> unParent = new List<Transform>();
        // Cache children (can't change the list you are looping through so need to cache it)
        foreach (Transform child in collectionPanel.transform)
        {
            unParent.Add(child);
        }

        // Unparent and destroy children
        foreach (Transform child in unParent)
        {
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        // Load Shop
        if (cardCollection?.allCardsDictionary == null)
        {
            Debug.Log("Dictionary was null!");
            return;
        }

        foreach (Card card in cardCollection.allCardsDictionary.Values)
        {
            bool addToShop = true;
            //If a card type was specified
            // None, Weapon, Armour, Ability, Environmental, Behaviour
            if (cardType != 0)
            {
                switch (card.currentCardType)
                {
                    case CardType.None:
                        break;
                    case CardType.Weapon:
                        if (cardType != 1)
                            addToShop = false;
                        break;
                    case CardType.Armour:
                        if (cardType != 2)
                            addToShop = false;
                        break;
                    case CardType.Ability:
                        if (cardType != 3)
                            addToShop = false;
                        break;
                    case CardType.Behaviour:
                        if (cardType != 4)
                            addToShop = false;
                        break;
                    case CardType.Environmental:
                        if (cardType != 5)
                            addToShop = false;
                        break;
                    default:
                        Debug.Log("I don't know what type of card this is");
                        break;
                }
            }

            // If card rarity was specified
            // None, Common, UnCommon, Rare, common -> rare, rare -> common
            if (rarity != 0)
            {
                switch (card.rarity)
                {
                    case Rarity.None:
                        Debug.Log("This card has no rarity! " + card.cardName);
                        addToShop = false;
                        break;
                    case Rarity.Common:
                        if (rarity != 1)
                            addToShop = false;
                        break;
                    case Rarity.Uncommon:
                        if (rarity != 2)
                            addToShop = false;
                        break;
                    case Rarity.Rare:
                        if (rarity != 3)
                            addToShop = false;
                        break;
                    default:
                        Debug.Log("I don't know what rarity of card this is");
                        break;
                }
            }

            if (addToShop)
                currentCollection.Add(card);
        }

        if (currentCollection.Count <= 0)
            return;

        //Debug.Log("Sorting List!");
        //currentShopCards.Sort();

        if (alphaBetically)
        {
            // A-Z
            for (int i = 0; i < currentCollection.Count; i++)
            {
                GameObject shopCard = CreateCard(currentCollection[i], deckBuilding);
                shopCard.transform.SetParent(collectionPanel.transform);
            }
        }
        else
        {
            // Z-A
            for (int i = currentCollection.Count - 1; i >= 0; i--)
            {
                GameObject shopCard = CreateCard(currentCollection[i], deckBuilding);
                shopCard.transform.SetParent(collectionPanel.transform);
            }
        }

        collectionPanel.GetComponent<PanelResizer>().Resize();
        //collectionPanel.GetComponent<PanelResizer>().Resize();
    }

    public GameObject CreateCard(Card card, bool deckBuilding)
    {
        // Creates the cards seen in the collection window
        if (collectionCardTemplate == null)
            return null;

        GameObject newCard;
        newCard = Instantiate(collectionCardTemplate);

        newCard.GetComponent<CardDisplay>().Initialise(card);

        int cardCount = 0;
        if (player.MyCards.ContainsKey(card.iD))
            cardCount = player.MyCards[card.iD];

        if (deckBuilding)
        {
            // Reduce the number of cards available if the deck contains this card
            foreach(int id in player.MyDecks[currentDeckName])
            {
                if (id == card.iD)
                    --cardCount;
            }
        }

        newCard.GetComponent<Collection_Card>().UpdateCardCount(cardCount);

        return newCard;
    }

    public void ToggleAlphaBetically()
    {
        // Sorts cards alphabetically
        alphaBetically = !alphaBetically;

        LoadCards(creatingDeck);
    }

    public void OpenCardWindow(Card card)
    {
        // Opens window that displays the cards much larger so they are easier to read
        cardWindow.GetComponent<CardWindow>().LoadWindow(card);
        cardWindow.SetActive(true);
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max, bool clamp = false)
    {
        if (clamp) x = Mathf.Max(in_min, Mathf.Min(x, in_max));
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
