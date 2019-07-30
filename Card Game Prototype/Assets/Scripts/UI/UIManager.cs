using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public CameraController myCameraController;
    public GameObject cardPhaseUI;
    public GameObject combatPhaseUI;
    public GameObject endGameUI;
    private Button quitGameButton;

    public GameObject pauseMenu;
    public Button pauseMenuButton;

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

    // Round Results
    public GameObject resultsWindow;
    public Text resultsWindowText;

    public GameManager gameManager;

    private bool cardPhase = true;
    private int maxCardsThisRound = 0;
    [SerializeField] private int maxCardsPlayed = 5;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // End Game UI
        endGameUI = transform.GetChild(4).gameObject;
        endGameUI.SetActive(false);
        quitGameButton = endGameUI.GetComponentInChildren<Button>();

        // Card Phase UI
        cardPhaseUI = transform.GetChild(0).gameObject;
        cardCount = transform.GetChild(0).GetChild(9).GetComponent<Text>();
        cardSlot = transform.GetChild(0).GetChild(1).gameObject;
        deck = transform.GetChild(0).GetChild(7).gameObject;
        numCardsLeft = transform.GetChild(0).GetChild(8).GetChild(0).GetChild(0).GetComponent<Text>();

        // Combat Phase UI
        combatPhaseUI = transform.GetChild(1).gameObject;

        quitGameButton.onClick.AddListener(QuitGame);

        // General HUD
        timeBars = new Image[2];
        timeBars[0] = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        timeBars[1] = transform.GetChild(2).GetChild(1).GetComponent<Image>();
        timerText = transform.GetChild(2).GetChild(2).GetComponent<Text>();
        roundNumber = transform.GetChild(2).GetChild(3).GetComponent<Text>();

        // Pause Menu
        pauseMenu = transform.GetChild(3).gameObject;
        pauseMenu.SetActive(false);
        pauseMenuButton.onClick.AddListener(PauseMenu);
    }

    private void OnEnable()
    {
        GameManager.EnterCardPhase += InitialiseCardPhaseUI;
        GameManager.EnterCombatPhase += InitialiseCombatPhaseUI;
        GameManager.EndGame += EndGame;
        GameManager.DisplayRoundResults += RoundResults;
    }

    private void OnDisable()
    {
        GameManager.EnterCardPhase -= InitialiseCardPhaseUI;
        GameManager.EnterCombatPhase -= InitialiseCombatPhaseUI;
        GameManager.EndGame -= EndGame;
        GameManager.DisplayRoundResults -= RoundResults;
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

    public void EndGame()
    {
        // Generate End Game Score
        Text endGameText = endGameUI.transform.GetChild(0).GetComponent<Text>();
        Text scoreBoard = endGameUI.transform.GetChild(2).GetComponent<Text>();
        endGameText.text = "Team " + (gameManager.winningTeam) + " is victorious!";
        var scoreInfo = gameManager.CalcScore("Team 1");
        scoreBoard.text = scoreInfo.scoreBoard;

        Player player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();
        // Get team number
        int.TryParse(("Team 1").Replace("Team ", ""), out int teamNumb);

        // Add campaign Reward
        if (player.CampaignProgress < GameInfo.CampaignNumber && gameManager.winningTeam == teamNumb)
        {
            scoreBoard.text += "\nCampaign Bonus:       " + gameManager.campaignReward;
            scoreInfo.score += gameManager.campaignReward;
            player.UpdateCampaignProg(GameInfo.CampaignNumber);
        }

        // Total
        scoreBoard.text += "\n\nTotal:        " + scoreInfo.score;

        player.AddCurrency(scoreInfo.score, false);
        if (gameManager.winningTeam == 1)
            endGameText.color = Color.blue;
        else if (gameManager.winningTeam == 2)
            endGameText.color = Color.red;

        endGameUI.SetActive(true);
    }

    public void QuitGame()
    {
        // Go back to main menu
        SceneManager.LoadScene(0);
    }

    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void RoundResults()
    {
        resultsWindow.SetActive(true);

        resultsWindowText.text = "Player " + gameManager.roundWinner + " has won\nround " + gameManager.currentRound + "!";

        if (gameManager.roundWinner == 1)
            resultsWindowText.color = Color.blue;
        else if (gameManager.roundWinner == 2)
            resultsWindowText.color = Color.red;
    }

    public void InitialiseCardPhaseUI()
    {
        resultsWindow.SetActive(false);
        //Debug.Log("Initialising Card Phase UI");
        if (gameManager.currentRound - 1 >= gameManager.cardsEachRound.Length)
            maxCardsPlayed += 1;
        else
            maxCardsThisRound += gameManager.cardsEachRound[gameManager.currentRound - 1];

        if (maxCardsPlayed != 0)
        {
            if (maxCardsThisRound > maxCardsPlayed)
                maxCardsThisRound = maxCardsPlayed;
        }
        cardCount.text = "Cards Left: " + maxCardsThisRound;
        roundNumber.text = " Round: " + gameManager.currentRound + " ";
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
