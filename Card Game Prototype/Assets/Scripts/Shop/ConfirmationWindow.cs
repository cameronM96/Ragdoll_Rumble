using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationWindow : MonoBehaviour
{
    public ShopManagerv2 shopManager;
    public GameObject shopCardTemplate;
    public GameObject shopPackTemplate;
    public GameObject windowCardPanel;

    public InputField shopItemInfo;

    [SerializeField] private Card currentCard;
    [SerializeField] private PackOfCards currentPack;

    // Card Shop
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

        // Info
        shopItemInfo.text = "Item Contains: \n";
        if (card != null)
            shopItemInfo.text += "1x " + card.cardName;

        currentCard = card;

        this.gameObject.SetActive(true);
    }


    // Pack Shop
    public void OpenWindow(PackOfCards pack)
    {
        GameObject shopCard = CreatePack(pack);

        foreach (Transform child in windowCardPanel.transform)
        {
            Destroy(child.gameObject);
        }

        shopCard.transform.SetParent(windowCardPanel.transform);
        shopCard.transform.localScale = new Vector3(2, 2, 2);

        shopCard.GetComponent<PackHolder>().costValues.SetActive(false);

        // Info
        shopItemInfo.text = "Item Contains: \n";
        if (pack != null)
        {
            // Specific Cards
            if (pack.guaranteedSpecificCards)
            {
                if (pack.gSpecificCards != null)
                {
                    foreach (Card card in pack.gSpecificCards)
                        shopItemInfo.text += card.cardName + "\n";
                }
            }

            // Random Specific
            if (pack.randomSpecificCards && pack.guaranteedFromSet > 0)
            {
                shopItemInfo.text += pack.guaranteedFromSet + "x random card from " + pack.setName + "\n";
            }

            // Random Rarity
            if (pack.guaranteedRandom)
            {
                if (pack.numberOfRares > 0)
                    shopItemInfo.text += pack.numberOfRares + "x Rare\n";

                if (pack.numberOfUncommons > 0)
                    shopItemInfo.text += pack.numberOfUncommons + "x Uncommon\n";

                if (pack.numberOfCommons > 0)
                    shopItemInfo.text += pack.numberOfCommons + "x Common\n";
            }

            // Random Card
            if (pack.randomCard)
            {
                if (pack.numberOfRandomCards > 0)
                    shopItemInfo.text += pack.numberOfRandomCards + "x Random Card\n";
            }
        }

        currentPack = pack;

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

    public GameObject CreatePack(PackOfCards newPack)
    {
        if (shopPackTemplate == null)
            return null;

        GameObject newCard = Instantiate(shopPackTemplate);

        newCard.GetComponent<PackHolder>().Initialise(newPack);

        return newCard;
    }

    public void ConfirmCardPurchase(bool usingPremiumCurrency)
    {
        bool purchaseSuccessful = false;
        Player player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();

        Debug.Log(player.playerName);
        Debug.Log(currentCard.cardName);
        if (player != null && currentCard != null)
        {
            if (usingPremiumCurrency)
            {
                // Using Gems
                int gemCost = currentCard.gemCost;
                if (gemCost == 0)
                    gemCost = shopManager.ReturnDefaultCosts(usingPremiumCurrency, currentCard);

                if (player.gems >= gemCost && gemCost != -1)
                {
                    player.SpendCurrency(gemCost, usingPremiumCurrency);
                    purchaseSuccessful = true;
                }
                else
                    Debug.Log("Not enough Gems!");
            }
            else
            {
                // Using Coins
                int coinCost = currentCard.gemCost;
                if (coinCost == 0)
                    coinCost = shopManager.ReturnDefaultCosts(usingPremiumCurrency, currentCard);

                if (player.coins >= coinCost && coinCost != -1)
                {
                    player.SpendCurrency(coinCost, usingPremiumCurrency);
                    purchaseSuccessful = true;
                }
                else
                    Debug.Log("Not enough coins!");
            }

            if (purchaseSuccessful)
            {
                // Purchase Successful, save profile
                player.AddCardToLibrary(currentCard);
            }
        }
        else
            Debug.LogError("Player Profile was not found and could not confirm purchase!");
    }

    public void ConfirmPackPurchase(bool usingPremiumCurrency)
    {
        bool purchaseSuccessful = false;
        Player player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();

        if (player != null && currentPack != null)
        {
            if (usingPremiumCurrency)
            {
                // Using Gems
                if (player.gems >= currentPack.gemCost)
                {
                    player.SpendCurrency(currentPack.gemCost, usingPremiumCurrency);
                    purchaseSuccessful = true;
                }
                else
                    Debug.Log("Not enough Gems!");
            }
            else
            {
                // Using Coins
                if (player.coins >= currentPack.coinCost)
                {
                    player.SpendCurrency(currentPack.coinCost, usingPremiumCurrency);
                    purchaseSuccessful = true;
                }
                else
                    Debug.Log("Not enough coins!");
            }

            if (purchaseSuccessful)
            {
                // Purchase Successful, save profile
                player.AddPack(currentPack.iD);
            }
        }
        else
            Debug.LogError("Player Profile was not found and could not confirm purchase!");
    }
}
