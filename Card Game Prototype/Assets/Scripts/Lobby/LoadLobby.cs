using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLobby : MonoBehaviour
{
    Player player;

    public int lobbySceneNumber = 12;

    public bool randomScene = false;
    public bool usingSceneName;
    public int nextLevelNumber;
    public string nextLevelName;
    public Vector2 sceneNumberRange;

    public bool isCampaign = true;
    public int campaignNumber = -1;
    public int difficulty = 1;
    public int rounds = 3;

    public bool randomAIDeck;
    public List<Card> aiDeck;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("PlayerProfile");
        if (playerObject == null)
            return;

        player = playerObject.GetComponent<Player>();

        if (player != null)
        {
            if (isCampaign)
            {
                GetComponent<Button>().enabled = player.CampaignProgress < campaignNumber;
            }
            else
                GetComponent<Button>().enabled = true;
        }
    }

    public void LoadScene()
    {
        if (randomScene)
            LoadRandomScene();

        if (usingSceneName)
        {
            GameInfo.NextLevelName = nextLevelName;
            GameInfo.NextLevelNumb = -1;
        }
        else
        {
            GameInfo.NextLevelNumb = nextLevelNumber;
            GameInfo.NextLevelName = null;
        }

        GameInfo.CampaignNumber = campaignNumber;
        GameInfo.Difficulty = difficulty;
        GameInfo.Rounds = rounds;

        GameInfo.RandomAIDeck = randomAIDeck;
        if (!randomAIDeck)
            GameInfo.AIDeck = CompileDeck();
        else
            GameInfo.AIDeck = new int[15];

        SceneManager.LoadScene(lobbySceneNumber);
    }

    private void LoadRandomScene()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int randomSceneNumber = Random.Range(Mathf.RoundToInt(sceneNumberRange.x), Mathf.RoundToInt(sceneNumberRange.y));

        usingSceneName = false;
        nextLevelNumber = randomSceneNumber;
    }

    private int[] CompileDeck()
    {
        int[] newDeck = new int[15];
        for (int i = 0; i < aiDeck.Count; i++)
        {
            newDeck[i] = aiDeck[i].iD;
        }

        return newDeck;
    }
}
