using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadQuickScene : MonoBehaviour
{
    public bool sceneNameCheck;
    public int sceneBuildNumber;
    public string sceneName;

    public void LoadScene()
    {
        if (sceneNameCheck)
            SceneManager.LoadScene(sceneName);
        else
            SceneManager.LoadScene(sceneBuildNumber);
    }
}
