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
    [HideInInspector] public bool weapon = true;
    [HideInInspector] public bool armour = true;
    [HideInInspector] public bool ability = true;
    [HideInInspector] public bool environmental = true;
    [HideInInspector] public bool behaviour = true;
    [HideInInspector] public int owned = 0;

    public GameObject cardWindow;

    // Deck Stuff
    public int requiredDeckSize = 15;
    public GameObject deckButtons;
    public GameObject deckCreation;
    public GameObject deckPanel;
    public GameObject deckCounter;
    public Text saveDeckText;
    public Color validDeckSizeColour;
    public Color invalidDeckSizeColour;
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
            if (deck.Value.Length != requiredDeckSize)
                newButton.GetComponent<Image>().color = invalidDeckSizeColour;
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
        if (deckName != "" || deckName != null)
        {
            bool deckExists = false;
            foreach (KeyValuePair<string, int[]> deck in player.MyDecks)
            {
                if (deck.Key == deckName)
                {
                    deckExists = true;
                    break;
                }
            }

            if (deckExists)
            {
                Debug.LogError("Invalid deck name! Deck already exists!");
                return;
            }

            currentDeckName = deckName;
            SaveDeck();
            bool loaded = LoadDeck(deckName);
            if (loaded)
            {
                ClearDeck();
                ToggleDeckCreation();
                Debug.Log("Creation of '" + deckName + "' was successful.");
            }
            else
                Debug.Log("Creation of '" + deckName + "' was unsuccessful.");
        }
        else
            Debug.LogError("Invalid deck name! Can't have an empty deck name!");
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
        UpdateDeckCardCount();
        saveDeckText.text = ("Save " + currentDeckName).ToUpper();

        return true;
    }

    public void UpdateDeckCardCount()
    {
        int deckCount = deckPanel.transform.childCount;
        deckCounter.GetComponentInChildren<Text>().text = deckCount + "/" + requiredDeckSize;
        if (deckCount == requiredDeckSize)
            deckCounter.GetComponent<Image>().color = validDeckSizeColour;
        else
            deckCounter.GetComponent<Image>().color = invalidDeckSizeColour;
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
            saveDeckText.text = ("Save " + currentDeckName).ToUpper();
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
        UpdateDeckCardCount();
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
            if (owned != 0)
            {
                switch (owned)
                {
                    case 1:
                        // Owned
                        if (!player.MyCards.ContainsKey(card.iD))
                            addToShop = false;
                        break;
                    case 2:
                        // Unowned
                        if (player.MyCards.ContainsKey(card.iD))
                            addToShop = false;
                        break;
                    default:
                        break;
                }
            }
            // None, Weapon, Armour, Ability, Environmental, Behaviour
            switch (card.currentCardType)
            {
                case CardType.None:
                    break;
                case CardType.Weapon:
                    if (!weapon)
                        addToShop = false;
                    break;
                case CardType.Armour:
                    if (!armour)
                        addToShop = false;
                    break;
                case CardType.Ability:
                    if (!ability)
                        addToShop = false;
                    break;
                case CardType.Behaviour:
                    if (!behaviour)
                        addToShop = false;
                    break;
                case CardType.Environmental:
                    if (!environmental)
                        addToShop = false;
                    break;
                default:
                    Debug.Log("I don't know what type of card this is");
                    break;
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
                {
                    --cardCount;
                    foreach (Transform child in deckPanel.transform)
                    {
                        if (child.gameObject.GetComponent<CardDisplay>().card == card)
                            child.GetComponent<DeckCard_Draggable>().collectionCard = newCard.GetComponent<Collection_Card>();
                    }
                }
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

    public void DropdownChange (Dropdown dropDown)
    {
        owned = dropDown.value;
        LoadCards(creatingDeck);
    }

    public void ToggleChange (int value, Toggle toggle)
    {
        // Weapon, Armour, Ability, Environmental, Behaviour
        switch (value)
        {
            case 0:
                weapon = toggle.isOn;
                break;
            case 1:
                armour = toggle.isOn;
                break;
            case 2:
                ability = toggle.isOn;
                break;
            case 3:
                environmental = toggle.isOn;
                break;
            case 4:
                behaviour = toggle.isOn;
                break;
            default:
                Debug.LogError("Unknown Value!");
                break;
        }

        LoadCards(creatingDeck);
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max, bool clamp = false)
    {
        if (clamp) x = Mathf.Max(in_min, Mathf.Min(x, in_max));
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
