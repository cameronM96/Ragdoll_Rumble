using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLobby : MonoBehaviour
{
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

        SceneManager.LoadScene(lobbySceneNumber);
    }

    private void LoadRandomScene()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        int randomSceneNumber = Random.Range(Mathf.RoundToInt(sceneNumberRange.x), Mathf.RoundToInt(sceneNumberRange.y));

        usingSceneName = false;
        nextLevelNumber = randomSceneNumber;
    }
}
