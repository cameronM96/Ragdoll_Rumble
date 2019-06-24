using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

[RequireComponent(typeof(AIDeck))]
public class AICardSelector : MonoBehaviour
{
    public GameManager gameManager;

    public Base_Stats bStats;

    public AIDeck deck;

    public int handSize = 5;
    private int maxCardsThisRound = 0;
    [SerializeField] private int maxCardsPlayed = 5;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        deck = GetComponent<AIDeck>();
        bStats = GetComponent<Base_Stats>();
    }

    private void OnEnable()
    {
        GameManager.EnterCardPhase += InitialiseCardPhaseUI;
    }

    private void OnDisable()
    {
        GameManager.EnterCardPhase -= InitialiseCardPhaseUI;
    }

    public void InitialiseCardPhaseUI()
    {
        //Debug.Log("Initialising Card Phase UI");

        maxCardsThisRound += gameManager.cardsEachRound[gameManager.currentRound - 1];
        if (maxCardsPlayed != 0)
        {
            if (maxCardsThisRound > maxCardsPlayed)
                maxCardsThisRound = maxCardsPlayed;
        }

        deck.DealNewHand(handSize);

        PickCard();

        gameManager.EndTurn();
    }

    public void PickCard()
    {
        //Debug.Log("AI is picking their cards");
        while (PlayCard())
        {
            int randomIndex = Random.Range(0, deck.currentHand.Capacity);

            // Pick Random Card
            Card chosenCard = deck.currentHand[randomIndex];

            GameObject[] slots = null;
            PlayableSlot chosenSlot = PlayableSlot.None;
            if (chosenCard.item != null)
            {
                // Pick a random viable slot to play the card into
                chosenSlot = PickSlot(chosenCard);

                // Get the joints to attach items to.
                if (chosenSlot != PlayableSlot.None)
                    slots = GetSlots(chosenSlot);
                else
                    Debug.Log("AI was unable to find a playable slot for this card");
            }

            chosenCard.PlayCard(bStats, slots);
            Debug.Log("AI played " + chosenCard.name + " in the " + chosenSlot + " slot");
        }
    }

    public PlayableSlot PickSlot(Card card)
    {
        bool foundViableSlot = false;
        // Pick a random slot to start from
        PlayableSlot chosenSlot = PlayableSlot.None;
        int slotNumber = Random.Range(1, 4);
        switch (slotNumber)
        {
            case 1:
                chosenSlot = PlayableSlot.Head;
                break;
            case 2:
                chosenSlot = PlayableSlot.Chest;
                break;
            case 3:
                chosenSlot = PlayableSlot.Hand;
                break;
            case 4:
                chosenSlot = PlayableSlot.Feet;
                break;
            default:
                break;
        }

        // Iterate through slots until viable one is found
        int iterationAttempts = 0;
        int maxAttempts = System.Enum.GetValues(typeof(PlayableSlot)).Length;
        while (!foundViableSlot)
        {
            // If the chosen slot is a slot this card can be placed in, proceed.
            if (chosenSlot == (chosenSlot & card.playableSlots))
                foundViableSlot = true;
            else
            {
                // If AI has iterated through all possible slots and still not found
                // A valid slots, end and say there are no viable slots
                if (iterationAttempts >= maxAttempts)
                {
                    chosenSlot = PlayableSlot.None;
                    foundViableSlot = true;
                }
                else
                {
                    // Iterate to the next slot
                    switch (chosenSlot)
                    {
                        case PlayableSlot.None:
                            Debug.Log("This should never happen");
                            break;
                        case PlayableSlot.Head:
                            chosenSlot = PlayableSlot.Chest;
                            break;
                        case PlayableSlot.Chest:
                            chosenSlot = PlayableSlot.Hand;
                            break;
                        case PlayableSlot.Hand:
                            chosenSlot = PlayableSlot.Feet;
                            break;
                        case PlayableSlot.Feet:
                            chosenSlot = PlayableSlot.Head;
                            break;
                        default:
                            Debug.Log("Unidentified Slot!");
                            break;
                    }

                    ++iterationAttempts;
                }
            }
        }

        return chosenSlot;
    }

    public GameObject[] GetSlots(PlayableSlot typeOfItem)
    {
        // Find the slots the items will attach to.
        GameObject[] slot = new GameObject[1];
        bool rightSide = false;

        switch (typeOfItem)
        {
            case PlayableSlot.None:
                break;
            case PlayableSlot.Head:
                slot[0] = bStats.slots[0];
                break;
            case PlayableSlot.Chest:
                slot[0] = bStats.slots[1];
                break;
            case PlayableSlot.Hand:
                // Pick random hand
                rightSide = (Random.Range(0, 2) == 0);
                //Debug.Log(rightSide);
                if (rightSide)
                    slot[0] = bStats.slots[2];
                else
                    slot[0] = bStats.slots[3];
                break;
            case PlayableSlot.Feet:
                slot = new GameObject[2];
                slot[0] = bStats.slots[4];
                slot[1] = bStats.slots[5];
                break;
            default:
                break;
        }

        return slot;
    }

    public bool PlayCard()
    {
        bool playable = false;
        // Check if player is allowed to play more cards
        if (maxCardsThisRound > 0)
        {
            --maxCardsThisRound;
            playable = true;
        }

        return playable;
    }
}
