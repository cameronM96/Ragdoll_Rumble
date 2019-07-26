using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class CollectionManager : MonoBehaviour
{
    public Player player;

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

    public List<Card> currentCollection;

    public CardCollection cardCollection;

    public string currentDeckName;
    public Card[] currentDeck;

    private bool creatingDeck;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();

        creatingDeck = true;
        ToggleDeckCreation();
        LoadCards(creatingDeck);
    }

    public void ToggleDeckCreation()
    {
        creatingDeck = !creatingDeck;

        deckButtons.SetActive(!creatingDeck);
        deckCreation.SetActive(creatingDeck);
    }

    public void CreateDeck()
    {

    }

    public bool LoadDeck(string deckName, DeckButton deckButton)
    {
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

        return true;
    }

    public void SaveDeck(string deckName)
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
        if (player.MyDecks.ContainsKey(deckName))
            player.UpdateDeck(deckName, deckInts.ToArray());
        else
            player.AddDeck(deckName, deckInts.ToArray());
    }

    public void ClearDeck()
    {
        foreach (Transform child in deckPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DeleteDeck(string deckName)
    {
        player.RemoveDeck(deckName);
    }

    public void LoadCards(bool deckBuilding)
    {
        // Clear Shop
        currentCollection.Clear();

        foreach (Transform child in collectionPanel.transform)
        {
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
    }

    public GameObject CreateCard(Card card, bool deckBuilding)
    {
        if (collectionCardTemplate == null)
            return null;

        GameObject newCard;
        if (deckBuilding)
            newCard = Instantiate(deckBuildingCardTemplate);
        else
            newCard = Instantiate(collectionCardTemplate);

        newCard.GetComponent<CardDisplay>().Initialise(card);

        return newCard;
    }

    public void ToggleAlphaBetically()
    {
        alphaBetically = !alphaBetically;

        LoadCards(creatingDeck);
    }

    public void OpenCardWindow(Card card)
    {
        cardWindow.GetComponent<CardWindow>().LoadWindow(card);
        cardWindow.SetActive(true);
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max, bool clamp = false)
    {
        if (clamp) x = Mathf.Max(in_min, Mathf.Min(x, in_max));
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
