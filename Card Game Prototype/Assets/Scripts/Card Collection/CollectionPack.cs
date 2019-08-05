using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class CollectionPack : MonoBehaviour
{
    [HideInInspector] public PackOfCards pack;

    public Text nameText;
    public Image packImage;

    public void Initialise(PackOfCards newPack)
    {
        pack = newPack;
        nameText.text = pack.packName;
        packImage.sprite = pack.packImage;
    }

    public void OpenPack(CardCollection cc)
    {
        Player player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();
        Debug.Log("Openning pack!");
        // Unlock specific Card
        if (pack.guaranteedSpecificCards && pack.gSpecificCards != null)
        {
            foreach (Card card in pack.gSpecificCards)
            {
                // Cross reference with card collection (should always return true);
                if (cc.allCardIDDictionary.ContainsKey(card.iD))
                {
                    Card unLockedCard = cc.allCardIDDictionary[card.iD];
                    player.AddCardToLibrary(unLockedCard);
                    GameObject newCard = CreateCard(unLockedCard);
                    newCard.transform.SetParent(this.transform.parent);
                }
            }
        }

        // Unlock Random Set card
        if (pack.randomSpecificCards && pack.rSpecificCards != null && pack.guaranteedFromSet > 0)
        {
            for (int i = 0; i < pack.guaranteedFromSet; i++)
            {
                Card[] cards = pack.rSpecificCards.ToArray();
                Random.InitState(System.DateTime.Now.Millisecond);
                int index = Mathf.FloorToInt(Random.Range(0, cards.Length));
                Card chosenCard = cards[index];

                if (cc.allCardIDDictionary.ContainsKey(chosenCard.iD))
                {
                    Card unLockedCard = cc.allCardIDDictionary[chosenCard.iD];
                    player.AddCardToLibrary(unLockedCard);
                    GameObject newCard = CreateCard(unLockedCard);
                    newCard.transform.SetParent(this.transform.parent);
                }
            }
        }

        // Unlock random rarity
        if (pack.guaranteedRandom)
        {
            if (pack.numberOfRares > 0)
            {
                for (int i = 0; i < pack.numberOfRares; i++)
                {
                    Card unLockedCard = cc.GetRandomRarity(Rarity.Rare);
                    player.AddCardToLibrary(unLockedCard);
                    GameObject newCard = CreateCard(unLockedCard);
                    newCard.transform.SetParent(this.transform.parent);
                }
            }

            if (pack.numberOfUncommons > 0)
            {
                for (int i = 0; i < pack.numberOfUncommons; i++)
                {
                    Card unLockedCard = cc.GetRandomRarity(Rarity.Uncommon);
                    player.AddCardToLibrary(unLockedCard);
                    GameObject newCard = CreateCard(unLockedCard);
                    newCard.transform.SetParent(this.transform.parent);
                }
            }

            if (pack.numberOfCommons > 0)
            {
                for (int i = 0; i < pack.numberOfCommons; i++)
                {
                    Card unLockedCard = cc.GetRandomRarity(Rarity.Common);
                    player.AddCardToLibrary(unLockedCard);
                    GameObject newCard = CreateCard(unLockedCard);
                    newCard.transform.SetParent(this.transform.parent);
                }
            }
        }

        // Unlock random card
        if (pack.randomCard && pack.numberOfRandomCards > 0)
        {
            for (int i = 0; i < pack.numberOfRandomCards; i++)
            {
                Card unLockedCard = cc.GetRandomCard();
                player.AddCardToLibrary(unLockedCard);
                GameObject newCard = CreateCard(unLockedCard);
                newCard.transform.SetParent(this.transform.parent);
            }
        }

        player.RemovePack(pack.iD);
        Destroy(this.gameObject);
    }

    public GameObject CreateCard (Card card)
    {
        CollectionManager cm = transform.root.GetComponent<CollectionManager>();

        if (cm == null)
            return null;

        // Creates the cards seen in the collection window
        if (cm.collectionCardTemplate == null)
            return null;

        GameObject newCard;
        newCard = Instantiate(cm.collectionCardTemplate);

        newCard.GetComponent<CardDisplay>().Initialise(card);

        int cardCount = 0;

        newCard.GetComponent<Collection_Card>().UpdateCardCount(cardCount);
        newCard.GetComponent<Collection_Card>().PackCard();

        return newCard;
    }
}
