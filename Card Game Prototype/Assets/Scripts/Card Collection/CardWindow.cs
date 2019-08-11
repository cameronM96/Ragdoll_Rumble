using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class CardWindow : MonoBehaviour
{
    public GameObject currentCardObj;
    public Card currentCard;
    public GameObject cardSlot;
    public Text disenchantText;

    public float disenchantCost = 4f;
    public Player player;

    public int RareDefaultCoinCost;
    public int UncommonDefaultCoinCost;
    public int CommonDefaultCoinCost;

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
        currentCardObj = transform.root.GetComponent<CollectionManager>().CreateCard(currentCard, false);
        currentCardObj.GetComponent<Collection_Card>().openCardButton.SetActive(false);

        currentCardObj.transform.SetParent(cardSlot.transform);
        currentCardObj.transform.localScale *= 2;
        currentCardObj.transform.localPosition = Vector3.zero;

        int coins = 0;
        if (currentCard.coinCost != 0)
            coins = currentCard.coinCost;
        else
        {
            switch (currentCard.rarity)
            {
                case Rarity.None:
                    break;
                case Rarity.Common:
                    coins = CommonDefaultCoinCost;
                    break;
                case Rarity.Uncommon:
                    coins = UncommonDefaultCoinCost;
                    break;
                case Rarity.Rare:
                    coins = RareDefaultCoinCost;
                    break;
                default:
                    break;
            }
        }

        coins = Mathf.FloorToInt(coins / disenchantCost);
        disenchantText.text = "Disenchant Value:\n" + coins;

        //currentCardObj.GetComponent<Draggable>().enabled = false;
    }

    public void DisenchantCard()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("PlayerProfile")?.GetComponent<Player>();

        if (player == null)
            return;

        player.RemoveCardFromLibrary(currentCard);

        int coins = 0;
        if (currentCard.coinCost != 0)
            coins = currentCard.coinCost;
        else
        {
            switch (currentCard.rarity)
            {
                case Rarity.None:
                    break;
                case Rarity.Common:
                    coins = CommonDefaultCoinCost;
                    break;
                case Rarity.Uncommon:
                    coins = UncommonDefaultCoinCost;
                    break;
                case Rarity.Rare:
                    coins = RareDefaultCoinCost;
                    break;
                default:
                    break;
            }
        }

        player.AddCurrency(Mathf.FloorToInt(coins / disenchantCost), false);

        int cardCount = 0;
        if (player.MyCards.ContainsKey(currentCard.iD))
            cardCount = player.MyCards[currentCard.iD];

        currentCardObj.GetComponent<Collection_Card>().UpdateCardCount(cardCount);

        transform.root.GetComponent<CollectionManager>().LoadCards(false);
        transform.root.GetComponent<CollectionManager>().UpdatePlayerInfo();

        if (cardCount <= 0)
            CloseWindow();
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
}
