using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour
{
    public GameObject costValues;
    public Text coinValue;
    public Text gemValue;

    CardDisplay cardDis;

    private bool initialised = false;

    public void CardPicked ()
    {
        Card card = GetComponent<CardDisplay>().card;
        SinglesShop shop = transform.parent.GetComponentInParent<SinglesShop>();

        if (shop != null && card != null)
            shop.confirmWindow.OpenWindow(card);
        else
            Debug.Log("Something went wrong and I couldn't find everything I needed");
    }

    private void Start()
    {
        cardDis = GetComponent<CardDisplay>();
    }

    private void Update()
    {
        if (!initialised)
        {
            if (cardDis.card != null)
            {
                if (cardDis.card.coinCost != 0)
                    coinValue.text = "" + cardDis.card.coinCost;

                if (cardDis.card.gemCost != 0)
                    gemValue.text = "" + cardDis.card.gemCost;

                initialised = true;
            }
        }
    }
}