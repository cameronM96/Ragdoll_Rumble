using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    Player player;
    public bool playerDeck = false;
    public CardCollection cardCollection;
    public int requiredDeckSize = 15;

    public List<Card> cards;
    //[HideInInspector]
    public List<GameObject> deckOfCards;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();
        //StartCoroutine(EndOfFrame());
    }

    private void OnEnable()
    {
        GameManager.InitialiseTheGame += Initialise;
    }

    private void OnDisable()
    {
        GameManager.InitialiseTheGame -= Initialise;
    }

    public void Initialise()
    {
        //Debug.Log("Initialising Deck");
        deckOfCards = new List<GameObject>();

        if (player.MyDecks.ContainsKey(GameInfo.SelectedDeckName))
        {
            //Debug.Log("Compiling Deck");
            cards = new List<Card>();
            // Create all the cards
            int[] cardIDs = player.MyDecks[GameInfo.SelectedDeckName];
            for (int i = 0; i < cardIDs.Length; i++)
            {
                // Match Id with a card and add it to the current cards list and create it.
                if (cardCollection.allCardIDDictionary.ContainsKey(cardIDs[i]))
                {
                    cards.Add(cardCollection.allCardIDDictionary[cardIDs[i]]);
                }
                else
                    Debug.LogError("Unidentified card! Card ID: " + cardIDs[i] + "\n Owner: " + this.gameObject);
            }
        }
        else
            Debug.LogError("Unknown Deck! " + GameInfo.SelectedDeckName + " could not be found.");

        foreach (Card card in cards)
        {
            if (card != null)
            {
                //Debug.Log("Creating Card");
                GameObject newCard = card.CreateCard();
                deckOfCards.Add(newCard);
                newCard.transform.position = this.transform.position;
                newCard.transform.SetParent(this.transform);
            }
        }

        ShuffleDeck();

        // Let game manager know this player is ready
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Debug.Log(transform.root.gameObject + " Initialising!");
        gm.InitialsePlayer();
    }

    IEnumerator EndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        Initialise();
    }

    public void ShuffleDeck()
    {
        Debug.Log("Shuffling Player Deck");
        foreach (Transform card in transform)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            int index = Random.Range(0, transform.childCount);
            card.SetSiblingIndex(index);
        }
    }
}
