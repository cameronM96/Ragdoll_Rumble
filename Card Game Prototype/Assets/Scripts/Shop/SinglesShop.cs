using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
using System.Linq;

public class SinglesShop : MonoBehaviour
{
    public ConfirmationWindow confirmWindow;
    public GameObject shopCardPanel;
    public GameObject shopCardTemplate;
    public Scrollbar scrollBar;
    public float startYPos;
    public float colLength;
    public bool alphaBetically = true;
    public int rarity = 0; // None, Common, UnCommon, Rare
    public int cardType = 0; // None, Weapon, Armour, Ability, Environmental, Behaviour

    public List<Card> currentShopCards;

    private CardCollection cardCollection;

    private void Start()
    {
        cardCollection = GetComponent<CardCollection>();
    }

    private void Update()
    {
        //ScrollWindow(scrollBar);
    }

    public void ScrollWindow (Scrollbar bar)
    {
        Debug.Log(Mathf.CeilToInt(currentShopCards.Count / 6f));
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

        foreach (Transform child in shopCardPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Load Shop
        if (cardCollection?.allCardsDictionary == null)
            return;

        foreach (Card card in cardCollection.allCardsDictionary.Values)
        {
            bool addToShop = true;
            //If a card type was specified
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
            if (rarity != 0)
            {
                switch (card.rarity)
                {
                    case Rarity.None:
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
    }

    public GameObject CreateCard(Card card)
    {
        if (shopCardTemplate == null)
            return null;

        GameObject newCard = Instantiate(shopCardTemplate);

        newCard.GetComponent<CardDisplay>().card = card;
        newCard.GetComponent<CardDisplay>().Initialise();

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
