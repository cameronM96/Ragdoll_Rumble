using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
using System.Linq;

public class SinglesShop : MonoBehaviour
{
    public ShopManagerv2 shopManager;
    public ConfirmationWindow confirmWindow;
    public GameObject shopCardPanel;
    public GameObject shopCardTemplate;
    public Scrollbar scrollBar;
    public float startYPos;
    public float colLength;
    public bool alphaBetically = true;
    public int rarity = 0; // None, Common, UnCommon, Rare, common -> rare, rare -> common
    public int cardType = 0; // None, Weapon, Armour, Ability, Environmental, Behaviour

    public List<Card> currentShopCards;

    public CardCollection cardCollection;

    public void ScrollWindow (Scrollbar bar)
    {
        //Debug.Log(Mathf.CeilToInt(currentShopCards.Count / 6f));
        float top = startYPos + (colLength * (Mathf.CeilToInt(currentShopCards.Count / 6f)));
        if (top < startYPos)
            top = startYPos;

        Vector3 newPos = shopCardPanel.transform.localPosition;

        newPos.y = Map(bar.value, 0, 1, startYPos, top, true);

        shopCardPanel.transform.localPosition = newPos;
    }

    public void LoadCards ()
    {
        // Clear Shop
        currentShopCards.Clear();

        List<Transform> unParent = new List<Transform>();
        // Cache children (can't change the list you are looping through so need to cache it)
        foreach (Transform child in shopCardPanel.transform)
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
                currentShopCards.Add(card);
        }

        if (currentShopCards.Count <= 0)
            return;

        //Debug.Log("Sorting List!");
        //currentShopCards.Sort();

        if (alphaBetically)
        {
            // A-Z
            for (int i = 0; i < currentShopCards.Count; i++)
            {
                GameObject shopCard = CreateCard(currentShopCards[i]);
                shopCard.transform.SetParent(shopCardPanel.transform);
            }
        }
        else
        {
            // Z-A
            for (int i = currentShopCards.Count - 1; i >= 0; i--)
            {
                GameObject shopCard = CreateCard(currentShopCards[i]);
                shopCard.transform.SetParent(shopCardPanel.transform);
            }
        }

        shopCardPanel.GetComponent<PanelResizer>().Resize();
    }

    public GameObject CreateCard(Card card)
    {
        if (shopCardTemplate == null)
            return null;

        GameObject newCard = Instantiate(shopCardTemplate);

        newCard.GetComponent<CardDisplay>().Initialise(card);
        newCard.GetComponent<ShopCard>().Initialise(shopManager, card);

        return newCard;
    }

    public void ToggleAlphaBetically()
    {
        alphaBetically = !alphaBetically;

        LoadCards();
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max, bool clamp = false)
    {
        if (clamp) x = Mathf.Max(in_min, Mathf.Min(x, in_max));
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
