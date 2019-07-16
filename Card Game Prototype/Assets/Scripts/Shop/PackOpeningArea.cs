using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EnumTypes;

[RequireComponent(typeof(BoxCollider))]
public class PackOpeningArea : MonoBehaviour
{
    public bool AllowedToDragAPack { get; set; }
    public GameObject cardFromPackPrefab;
    public Button doneButton;
    public Button backButton;
    [Header("Probabilities")]
    [Range(0, 1)]
    public float rareProbability;
    [Range(0, 1)]
    public float uncommonProbability;
    /* sparkle cards rarity, disabled until implimented
    [Range(0, 1)]
    public float rareSparkleProbability;
    [Range(0, 1)]
    public float uncommonSparkleProbability;
    [Range(0, 1)]
    public float commonSparkleProbability;
    */

    public bool giveAtLeastOneRare = false;

    public Transform[] slotsForCards;

    private BoxCollider col;
    private List<GameObject> cardsFromPackCreated = new List<GameObject>();
    private int numOfCardsOpened = 0;
    public int numberOfCardsOpenedFromPack
    {
        get { return numOfCardsOpened; }
        set
        {
            numOfCardsOpened = value;
            if (value == slotsForCards.Length)
            {
                doneButton.gameObject.SetActive(true);
            }
        }
    }

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        AllowedToDragAPack = true;
    }

    public bool CursorOverArea()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
            {
                passedThroughTableCollider = true;
            }
        }
        return passedThroughTableCollider;
    }

    public void ShowPackOpening(Vector3 cardsInitialPosition)
    {
        Debug.Log("Openning Pack!");
        Rarity[] rarities = new Rarity[slotsForCards.Length];
        bool atLeastOneRareGiven = false;
        for(int i=0;i<rarities.Length;i++)
        {
            //add in sparkle cards later, need to fix up how the card prefbs handle rarities
            float prob = Random.Range(0f, 1f);
            if (prob<rareProbability)
            {
                rarities[i] = Rarity.Rare;
                atLeastOneRareGiven = true;
            }
            if (prob<uncommonProbability)
            {
                rarities[i] = Rarity.Uncommon;
            }
            else
            {
                rarities[i] = Rarity.Common;
            }
        }

        //makes a random card rare if none were rolled and give at least oen rare is enabled
        if (atLeastOneRareGiven ==false && giveAtLeastOneRare)
        {
            rarities[Random.Range(0, rarities.Length)] = Rarity.Rare;
        }

        for(int i =0;i<rarities.Length;i++)
        {
            Card card = cardFromPack(rarities[i]);
            cardsFromPackCreated.Add(card.CreateCard());
        }
    }

    private Card cardFromPack(Rarity rarity)
    {
        List<Card> CardsOfThisRarity = CardCollection.Instance.GetCardsWithRarity(rarity);
        Card a = CardsOfThisRarity[Random.Range(0, CardsOfThisRarity.Count)];

        CardCollection.Instance.quantityOfEachCard[a]++;

        GameObject card;
        card = Instantiate(cardFromPackPrefab) as GameObject;

        card.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        Card manager = card.GetComponent<Card>();
                return manager;
    }

    public void Done()
    {
        AllowedToDragAPack = true;
        numberOfCardsOpenedFromPack = 0;
        while (cardsFromPackCreated.Count>0)
        {
            GameObject g = cardsFromPackCreated[0];
            cardsFromPackCreated.RemoveAt(0);
            Destroy(g);
        }
        backButton.interactable = true;
    }
}