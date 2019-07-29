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

    public void LoadScene()
    {
        if (randomScene)
            LoadRandomScene();

        if (usingSceneName)
        {
            LevelHolder.NextLevelName = nextLevelName;
            LevelHolder.NextLevelNumb = -1;
        }
        else
        {
            LevelHolder.NextLevelNumb = nextLevelNumber;
            LevelHolder.NextLevelName = null;
        }

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
