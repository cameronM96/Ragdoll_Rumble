using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CameraController myCameraController;
    public GameObject cardPhaseUI;
    public GameObject combatPhaseUI;
    public GameObject cardSlot;
    public GameObject deck;
    public int handSize = 5;
    public Text numCardsLeft;
    public Text cardCount;
    public Text timerText;
    public Text roundNumber;
    public Image[] timeBars;
    public Color startColor;
    public Color middleColor;
    public Color endColor;

    public GameManager gameManager;

    private bool cardPhase = true;
    private int maxCardsThisRound = 0;
    [SerializeField] private int maxCardsPlayed = 5;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // General HUD
        timeBars = new Image[2];
        timeBars[0] = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        timeBars[1] = transform.GetChild(2).GetChild(1).GetComponent<Image>();
        timerText = transform.GetChild(2).GetChild(2).GetComponent<Text>();
        roundNumber = transform.GetChild(2).GetChild(3).GetComponent<Text>();

        // Card Phase UI
        cardPhaseUI = transform.GetChild(0).gameObject;
        cardCount = transform.GetChild(0).GetChild(9).GetComponent<Text>();
        cardSlot = transform.GetChild(0).GetChild(1).gameObject;
        deck = transform.GetChild(0).GetChild(7).gameObject;
        numCardsLeft = transform.GetChild(0).GetChild(8).GetChild(0).GetChild(0).GetComponent<Text>();

        // Combat Phase UI
        combatPhaseUI = transform.GetChild(1).gameObject;
    }

    private void OnEnable()
    {
        GameManager.EnterCardPhase += InitialiseCardPhaseUI;
        GameManager.EnterCombatPhase += InitialiseCombatPhaseUI;
    }

    private void OnDisable()
    {
        GameManager.EnterCardPhase -= InitialiseCardPhaseUI;
        GameManager.EnterCombatPhase -= InitialiseCombatPhaseUI;
    }

    // Update is called once per frame
    void Update()
    {
        // Update Phase Timer
        if (cardPhase)
        {
            // Card Phase
            timerText.text = "Timer: " + Mathf.CeilToInt(gameManager.cardPhaseTimer);
            foreach (Image timeBar in timeBars)
            {
                timeBar.fillAmount = gameManager.cardPhaseTimer / gameManager.cardPhaseLength;

                // Change Bar Color over Time
                if (timeBar.fillAmount > 0.5f)
                    timeBar.color = Color.Lerp(middleColor, startColor, Map(timeBar.fillAmount, 1, 0.5f, 1, 0));
                else if (timeBar.fillAmount > 0.2f)
                    timeBar.color = Color.Lerp(endColor, middleColor, Map(timeBar.fillAmount, 0.5f, 0.2f, 1, 0));
                else
                    timeBar.color = endColor;
            }
        }
        else
        {
            // Combat Phase
            timerText.text = "Timer: " + Mathf.CeilToInt(gameManager.combatPhaseTimer);
            foreach (Image timeBar in timeBars)
            {
                timeBar.fillAmount = gameManager.combatPhaseTimer / gameManager.combatPhaseLength;

                // Change Bar Color over Time
                if (timeBar.fillAmount > 0.5f)
                    timeBar.color = Color.Lerp(middleColor, startColor, Map(timeBar.fillAmount, 1, 0.5f, 1, 0));
                else if (timeBar.fillAmount > 0.2f)
                    timeBar.color = Color.Lerp(endColor, middleColor, Map(timeBar.fillAmount, 0.5f, 0.2f, 1, 0));
                else
                    timeBar.color = endColor;
            }
        }
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
        cardCount.text = "Cards Left: " + maxCardsThisRound;
        roundNumber.text = "Round: " + gameManager.currentRound + " ";
        DealNewHand();
        cardPhase = true;
        cardPhaseUI.SetActive(true);
        combatPhaseUI.SetActive(false);
    }

    public void InitialiseCombatPhaseUI()
    {
        //Debug.Log("Initialising Combat Phase UI");
        cardPhase = false;
        cardPhaseUI.SetActive(false);
        combatPhaseUI.SetActive(true);
    }

    public bool PlayCard()
    {
        bool playable = false;
        // Check if player is allowed to play more cards
        if (maxCardsThisRound > 0)
        {
            --maxCardsThisRound;
            cardCount.text = "Cards Left: " + maxCardsThisRound;
            playable = true;
        }

        return playable;
    }

    public void DealNewHand()
    {
        int cardsLeft = cardSlot.transform.childCount;

        // Deal left over cards back into deck
        for (int i = 0; i < cardsLeft; i++)
            cardSlot.transform.GetChild(0).SetParent(deck.transform);

        // Deal cards up to hand size
        for (int i = 0; i < handSize; i++)
        {
            if (deck.transform.childCount != 0)
                deck.transform.GetChild(0).SetParent(cardSlot.transform);
        }

        numCardsLeft.text = "Deck\n" + deck.transform.childCount;
    }

    public float Map(float value, float in_min, float in_max, float out_min, float out_max)
    {
        return (value - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
