using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour
{
    public GameObject costValues;
    public Text coinValue;
    public Text gemValue;
    public Card card;
    CardDisplay cardDis;
    ShopManagerv2 shopManager;

    public void Initialise(ShopManagerv2 shopMan, Card newCard)
    {
        cardDis = GetComponent<CardDisplay>();
        shopManager = shopMan;
        card = newCard;
        cardDis.card = newCard;

        if (cardDis.card != null)
        {
            if (cardDis.card.coinCost != 0)
                coinValue.text = "" + cardDis.card.coinCost;
            else
                coinValue.text = "" + shopManager.ReturnDefaultCosts(false, card);

            if (cardDis.card.gemCost != 0)
                gemValue.text = "" + cardDis.card.gemCost;
            else
                gemValue.text = "" + shopManager.ReturnDefaultCosts(true, card);
        }
    }

    public void CardPicked ()
    {
        card = GetComponent<CardDisplay>().card;
        SinglesShop shop = transform.parent.GetComponentInParent<SinglesShop>();

        if (shop != null && card != null)
            shop.confirmWindow.OpenWindow(card);
        else
            Debug.Log("Something went wrong and I couldn't find everything I needed");
    }
}