using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationWindow : MonoBehaviour
{
    public GameObject shopCardTemplate;
    public GameObject windowCardPanel;

    public void OpenWindow(Card card)
    {
        GameObject shopCard = CreateCard(card);

        foreach(Transform child in windowCardPanel.transform)
        {
            Destroy(child.gameObject);
        }

        shopCard.transform.SetParent(windowCardPanel.transform);
        shopCard.transform.localScale = new Vector3(2, 2, 2);

        shopCard.GetComponent<ShopCard>().costValues.SetActive(false);

        this.gameObject.SetActive(true);
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
}
