using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    [HideInInspector] public float cardPhaseTimer = 0;
    [HideInInspector] public float combatPhaseTimer = 0;

    public int numberOfRounds = 3;
    public int[] cardsEachRound;
    [HideInInspector] public int currentRound = 0;

    public int numberOfTeams;
    public int[] teamScore;
    public int[] alivePlayersPerTeam;
    public int winningTeam;

    public List<int> defeatedTeams = new List<int>();

    public int gameStartDelay;

    public int numbOfPlayers = 0;
    private int readyPlayers = 0;
    private bool gameOver = false;

    // Reward system
    public int baseReward;
    public int gameWinReward;
    public int roundWinReward;
    public int dominationReward;
    public int campaignReward;

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
        StartCoroutine(StartGameDelay(gameStartDelay));
    }

    IEnumerator StartGameDelay (float waitTimer)
    {
        if (waitTimer > 0)
            yield return new WaitForSeconds(waitTimer);
        else
            yield return new WaitForEndOfFrame();

        InitialiseCardPhase();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (cardPhase)
            {
                cardPhaseTimer -= Time.deltaTime;

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

                if (combatPhaseTimer <= 0)
                {
                    // Check if event has subscribers
                    if (EnterCardPhase != null)
                    {
                        // Go to Card phase!
                        InitialiseCardPhase();
                    }
                    else
                        Debug.Log("ERROR: Card Phase has no subscribers!");
                }
            }
        }
    }

    public void InitialiseCardPhase ()
    {
        currentRound++;
        readyPlayers = 0;
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
            EndTheGame();
    }

    public void InitialiseCombatPhase ()
    {
        // Enter Combat Phase
        //Debug.Log("Beginning Combat Phase!");

        // Get all alive Players
        alivePlayersPerTeam = new int[numberOfTeams];
        for (int i = 0; i < numberOfTeams; i++)
            alivePlayersPerTeam[i] = GameObject.FindGameObjectsWithTag("Team " + (i + 1)).Length;
        defeatedTeams = new List<int>();

        combatPhaseTimer = combatPhaseLength;
        cardPhase = false;
        EnterCombatPhase?.Invoke();
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
                    Debug.Log("Team " + (i + 1) + " has won and now has won " + teamScore[i] + " round(s)!");
                    InitialiseCardPhase();
                    break;
                }
            }
        }
        else if (defeatedTeams.Count >= numberOfTeams - 1)
            Debug.Log("Some how everyone is dead but no one won...");
    }

    public void EndTheGame ()
    {
        Debug.Log("Team " + (winningTeam) + " is victorious!");
        gameOver = true;
        EndGame?.Invoke();
    }

    //baseReward;
    //gameWinReward;
    //roundWinReward;
    //dominationReward;
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
