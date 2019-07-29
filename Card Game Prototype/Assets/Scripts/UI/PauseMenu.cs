using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameManager gameManager;
    float previousTimeScale;

    //private float previousTimeScale = 1;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    //Maybe put pause function in later
    //private void OnEnable()
    //{
    //    if (gameManager != null)
    //    {
    //        if (gameManager.numbOfPlayers == 1)
    //        {
    //            previousTimeScale = Time.timeScale;
    //            Time.timeScale = 0;
    //        }
    //        else
    //            previousTimeScale = 1f;
    //    }
    //}

    //private void OnDisable()
    //{
    //    Time.timeScale = previousTimeScale;
    //}

    public void QuitButton()
    {
        // Go back to main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ResumeButton ()
    {
        this.gameObject.SetActive(false);
    }

    public void RestartLevel ()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
