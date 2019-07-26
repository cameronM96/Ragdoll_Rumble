using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWindow : MonoBehaviour
{
    public GameObject currentCardObj;
    public Card currentCard;
    public GameObject cardSlot;

    public Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();
    }

    public void LoadWindow(Card card)
    {
        foreach (Transform child in cardSlot.transform)
        {
            Destroy(child.gameObject);
        }

        currentCard = card;
        currentCardObj = card.CreateCard();

        currentCardObj.transform.SetParent(cardSlot.transform);
        currentCardObj.transform.localScale *= 2;
        currentCardObj.transform.localPosition = Vector3.zero;

        currentCardObj.GetComponent<Draggable>().enabled = false;
    }

    public void DisenchantCard()
    {
        player.RemoveCardFromLibrary(currentCard);

        player.AddCurrency(currentCard.coinCost / 4, false);
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
}
