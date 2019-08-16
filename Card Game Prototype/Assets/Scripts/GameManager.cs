using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void InitialiseGame();
    public static event InitialiseGame InitialiseTheGame;

    public delegate void RoundResults();
    public static event RoundResults DisplayRoundResults;

    public delegate void CardPhase();
    public static event CardPhase EnterCardPhase;

    public delegate void CombatPhase();
    public static event CombatPhase EnterCombatPhase;

    public delegate void PlayerFell();
    public static event PlayerFell PlayerHasFallen;

    public delegate void GameOver();
    public static event GameOver EndGame;

    private bool cardPhase = true;

    public float cardPhaseLength;
    public float combatPhaseLength;

    /*[HideInInspector]*/ public float cardPhaseTimer = 0;
    /*[HideInInspector]*/ public float combatPhaseTimer = 0;

    public int numberOfRounds = 3;
    public int[] cardsEachRound;
    public int currentRound = 0;

    public int numberOfTeams;
    public int[] teamScore;
    public int[] alivePlayersPerTeam;
    public int winningTeam;

    public List<int> defeatedTeams = new List<int>();

    public int gameStartDelay;

    public int numbOfPlayers = 0;
    private int readyPlayers = 0;
    private bool gameOver = false;

    public int roundWinner;

    // Round Transition Delay
    public float roundTransitionDelay = 3f;

    // Reward system
    public int baseReward;
    public int gameWinReward;
    public int roundWinReward;
    public int dominationReward;
    public int campaignReward;

    // Initialise Game
    private bool gameInitialised = false;
    [SerializeField] private int initialisedPlayers = 0;

    public bool debugging = false;

    public AudioSource announcerAudioSource;
    public AudioSource musicAudioSource;
    public AudioClip[] victoryNoise;
    public AudioClip[] defeatNoise;
    public AudioClip[] draw;
    public AudioClip[] countDownNoise;
    public AudioClip[] beginAnnounce;
    public AudioClip[] fightAnnounce;
    public AudioClip nextRoundAnnounce;
    public AudioClip[] rounds;
    public AudioClip combatPhaseMusic;
    public AudioClip transitionMusic;


    private bool countDown, countDown10, countDown30;
    private bool showingResults;

    // Start is called before the first frame update
    void Start()
    {
        teamScore = new int[numberOfTeams];
        alivePlayersPerTeam = new int[numberOfTeams];
        for (int i = 0; i < numberOfTeams; i++)
            alivePlayersPerTeam[i] = GameObject.FindGameObjectsWithTag("Team " + (i + 1)).Length;

        cardPhaseTimer = cardPhaseLength;
        combatPhaseTimer = combatPhaseLength;
        currentRound = 0;

        if (debugging)
        {
            GameInfo.SelectedDeckName = "Starter Deck";
            GameInfo.RandomAIDeck = true;
        }

        StartCoroutine(StartGameDelay(gameStartDelay));
    }

    IEnumerator StartGameDelay (float waitTimer)
    {
        if (waitTimer > 0)
            yield return new WaitForSeconds(waitTimer);
        else
            yield return new WaitForEndOfFrame();

        Debug.Log("Initialising Game!");
        InitialiseTheGame?.Invoke();
    }

    public void InitialsePlayer()
    {
        Debug.Log("Initialising Character");
        ++initialisedPlayers;
        if (initialisedPlayers >= numbOfPlayers && !gameInitialised)
        {
            gameInitialised = true;
            Debug.Log("Starting Game!");
            InitialiseCardPhase();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameInitialised)
            return;

        if (gameOver)
            return;

        if (showingResults)
            return;

        if (cardPhase)
        {
            cardPhaseTimer -= Time.deltaTime;

            if (!countDown10 && cardPhaseTimer < 10)
            {
                announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                announcerAudioSource.clip = countDownNoise[1];
                announcerAudioSource.Play();
                countDown10 = true;
            }

            if (!countDown &&  cardPhaseTimer < 5)
            {
                announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                announcerAudioSource.clip = countDownNoise[0];
                announcerAudioSource.Play();
                countDown = true;
            }

            if (cardPhaseTimer <= 0)
            {
                // Check if event has subscribers
                if (EnterCombatPhase != null)
                {
                    // Go to combat phase!
                    InitialiseCombatPhase();
                }
                else
                    Debug.Log("ERROR: Combat Phase has no subscribers!");
            }
        }
        else
        {
            combatPhaseTimer -= Time.deltaTime;
            if (!countDown30 && combatPhaseTimer < 30)
            {
                announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                announcerAudioSource.clip = countDownNoise[2];
                announcerAudioSource.Play();
                countDown30 = true;
            }

            if (!countDown10 && combatPhaseTimer < 10)
            {
                announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                announcerAudioSource.clip = countDownNoise[1];
                announcerAudioSource.Play();
                countDown10 = true;
            }

            if (!countDown && combatPhaseTimer < 5)
            {
                announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                announcerAudioSource.clip = countDownNoise[0];
                announcerAudioSource.Play();
                countDown = true;
            }

            if (combatPhaseTimer <= 0)
            {
                // get health of AI
                GameObject ai = GameObject.FindGameObjectWithTag("Team 2");
                Base_Stats aiStats;
                int aiHealth = -1, aiHPPercent = -1;
                if (ai != null)
                {
                    aiStats = ai.GetComponent<Base_Stats>();
                    if (aiStats != null)
                    {
                        aiHealth = aiStats.GetHealth();
                        aiHPPercent = aiHealth / aiStats.maxHP;
                    }
                }

                // Get health of Player
                GameObject person = GameObject.FindGameObjectWithTag("Team 1");
                Base_Stats personStats;
                int personHealth = -1, personHPPercent = -1;
                if (person != null)
                {
                    personStats = ai.GetComponent<Base_Stats>();
                    if (personStats != null)
                    {
                        personHealth = personStats.GetHealth();
                        personHPPercent = personHealth / personStats.maxHP;
                    }
                }

                roundWinner = -1;
                // Check who wins
                if (personHPPercent != -1 && aiHPPercent != -1 && aiHealth != -1 && personHealth != -1)
                {
                    if (personHPPercent == aiHPPercent)
                    {
                        // HP percent is the same check total hp
                        if (personHealth == aiHealth)
                        {
                            // It's a tie
                            roundWinner = -1;
                        }
                        else if (personHealth > aiHealth)
                        {
                            // player wins
                            roundWinner = 1;
                        }
                        else
                        {
                            // AI wins
                            roundWinner = 2;
                        }
                    }
                    else if (personHPPercent > aiHPPercent)
                    {
                        // Player wins
                        roundWinner = 1;
                    }
                    else
                    {
                        // AI wins
                        roundWinner = 2;
                    }
                }
                else
                    Debug.LogError("Was unable to find players health and was not able to determine a winner");

                if (roundWinner > 0)
                    ++teamScore[roundWinner - 1];
                // End round
                ShowRoundResults();
            }
        }
    }

    public void ShowRoundResults ()
    {
        showingResults = true;
        DisplayRoundResults?.Invoke();

        // Find score that will win game
        int instaWinScore = numberOfRounds / 2;
        if (numberOfRounds % 2 != 0)
            ++instaWinScore;

        //Debug.Log("Victory requires " + instaWinScore + " wins");

        // Find which team has the highest score
        int highestScore = -1;
        winningTeam = -1;
        for (int i = 0; i < numberOfTeams; i++)
        {
            if (teamScore[i] > highestScore)
            {
                highestScore = teamScore[i];
                winningTeam = i + 1;
            }
        }

        if (winningTeam == 1)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
            announcerAudioSource.clip = victoryNoise[Random.Range(0,victoryNoise.Length)];
            announcerAudioSource.Play();
        }
        else if (winningTeam == 2)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
            announcerAudioSource.clip = defeatNoise[Random.Range(0, defeatNoise.Length)];
            announcerAudioSource.Play();
        }
        else
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
            announcerAudioSource.clip = draw[Random.Range(0, draw.Length)];
            announcerAudioSource.Play();
        }

        musicAudioSource.volume = PlayerPreferances.Music * PlayerPreferances.Master;
        StartCoroutine(AudioFadeOut.FadeOut(musicAudioSource, 2f));
        // Check if game should end
        if (highestScore >= instaWinScore)
            EndTheGame();
        else
            StartCoroutine(RoundResultsDelay(roundTransitionDelay));
    }

    IEnumerator RoundResultsDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        showingResults = false;
        InitialiseCardPhase();
    }

    public void InitialiseCardPhase ()
    {
        currentRound++;
        readyPlayers = 0;
        // Reset Audio
        countDown = false;
        countDown10 = false;
        countDown30 = false;
        // Find score that will win game
        int instaWinScore = numberOfRounds / 2;
        if (numberOfRounds % 2 != 0)
            ++instaWinScore;

        //Debug.Log("Victory requires " + instaWinScore + " wins");

        // Find which team has the highest score
        int highestScore = -1;
        winningTeam = -1;
        for (int i = 0; i < numberOfTeams; i++)
        { 
            if (teamScore[i] > highestScore)
            {
                highestScore = teamScore[i];
                winningTeam = i + 1;
            }
        }

        // Check if game should end
        if (highestScore < instaWinScore)
        {
            // Enter Card Phase
            //Debug.Log("Beginning Card Phase!");
            cardPhaseTimer = cardPhaseLength;
            cardPhase = true;
            EnterCardPhase?.Invoke();
        }
        else
        {
            // This should never occur, it's just for safety
            EndTheGame();
        }
    }

    public void InitialiseCombatPhase ()
    {
        roundWinner = -1;
        // Reset Audio
        countDown = false;
        countDown10 = false;
        countDown30 = false;
        // Enter Combat Phase
        //Debug.Log("Beginning Combat Phase!");
        // Get all alive Players
        StartCoroutine(CombatPhaseAudioHandler());
        alivePlayersPerTeam = new int[numberOfTeams];
        for (int i = 0; i < numberOfTeams; i++)
            alivePlayersPerTeam[i] = GameObject.FindGameObjectsWithTag("Team " + (i + 1)).Length;
        defeatedTeams = new List<int>();

        combatPhaseTimer = combatPhaseLength;
        cardPhase = false;
        EnterCombatPhase?.Invoke();
    }

    IEnumerator CombatPhaseAudioHandler()
    {
        // Music
        musicAudioSource.volume = PlayerPreferances.Music * PlayerPreferances.Master;
        musicAudioSource.clip = combatPhaseMusic;
        musicAudioSource.Play();

        // Announcer
        announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
        if (currentRound - 1 < rounds.Length)
            announcerAudioSource.clip = rounds[currentRound - 1];
        else
            announcerAudioSource.clip = nextRoundAnnounce;

        announcerAudioSource.Play();
        yield return new WaitForSeconds(2f);
        Random.InitState(System.DateTime.Now.Millisecond);
        announcerAudioSource.clip = beginAnnounce[Random.Range(0, beginAnnounce.Length)];
        announcerAudioSource.Play();
        yield return new WaitForSeconds(1f);
        announcerAudioSource.clip = fightAnnounce[Random.Range(0, fightAnnounce.Length)];
        announcerAudioSource.Play();
    }

    public void PlayerDied (GameObject deadPlayer)
    {
        // Get team number of defeated player
        int.TryParse((deadPlayer.tag).Replace("Team ", ""), out int teamNumb);

        // Mark off player on team if is a valid team number
        if (teamNumb != 0)
        {
            --alivePlayersPerTeam[teamNumb - 1];
            PlayerHasFallen?.Invoke();
            // Check if this player was the last one on their team
            if (alivePlayersPerTeam[teamNumb - 1] <= 0)
                defeatedTeams.Add(teamNumb);
        }
        else
            Debug.Log("ERROR: Could not get team number! Unidentified death!");

        // Check if there is only 1 team standing
        if (defeatedTeams.Count == numberOfTeams - 1)
        {
            // Find Winning team
            for (int i = 0; i < numberOfTeams; i++)
            {
                //Debug.Log("Checking Team " + (i + 1));
                if (!defeatedTeams.Contains(i + 1))
                {
                    ++teamScore[i];
                    roundWinner = i + 1;
                    Debug.Log("Team " + (i + 1) + " has won and now has won " + teamScore[i] + " round(s)!");
                    ShowRoundResults();
                    break;
                }
            }
        }
        else if (defeatedTeams.Count >= numberOfTeams - 1)
            Debug.Log("Some how everyone is dead but no one won... its a tie?");
    }

    public void EndTheGame ()
    {
        Debug.Log("Team " + (winningTeam) + " is victorious!");
        if (winningTeam == 1)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
            announcerAudioSource.clip = victoryNoise[Random.Range(0, victoryNoise.Length)];
            announcerAudioSource.Play();
        }
        else
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            announcerAudioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
            announcerAudioSource.clip = defeatNoise[Random.Range(0, defeatNoise.Length)];
            announcerAudioSource.Play();
        }
        gameOver = true;
        EndGame?.Invoke();
    }

    public (string scoreBoard, int score) CalcScore(string teamName)
    {
        string scoreBoard = "Score:";
        int score = 0;
        // Base Reward
        scoreBoard += "\nCompletion:        " + baseReward;
        score += baseReward;

        // Get team number
        int.TryParse((teamName).Replace("Team ", ""), out int teamNumb);

        // Round bonus
        if (teamScore[teamNumb - 1] > 0)
        {
            scoreBoard += "\nRound Bonus:        " + (roundWinReward * teamScore[teamNumb - 1]);
            score += (roundWinReward * teamScore[teamNumb - 1]);
        }

        // Victory Bonus
        if (winningTeam == teamNumb)
        {
            scoreBoard += "\nVictory Bonus:        " + gameWinReward;
            score += gameWinReward;
        }

        // Domination Bonus
        if (teamScore[teamNumb - 1] == currentRound)
        {
            scoreBoard += "\nDomination Bonus:        " + dominationReward;
            score += dominationReward;
        }

        return (scoreBoard,score);
    }

    // PlaceHolder until Multi-player is implemented
    public void PlayerJoined ()
    {
        Debug.Log("Player has joined!");
        ++numbOfPlayers;
    }

    public void PlayerLeft ()
    {
        --numbOfPlayers;
    }

    public void EndTurn ()
    {
        // Switch this out for a function that checks if everyone else has finished their turn
        ++readyPlayers;
        if (readyPlayers >= numbOfPlayers)
        {
            //Debug.Log("All Players Ready! Ending Card Phase Early!");
            InitialiseCombatPhase();
        }
        //else
            //Debug.Log("A player is ready!");

    }
}
