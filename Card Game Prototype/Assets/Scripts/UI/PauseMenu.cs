using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameManager gameManager;
    public Button resumeButton;
    public Button quitButton;

    //private float previousTimeScale = 1;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        resumeButton = transform.GetChild(1).GetComponent<Button>();
        quitButton = transform.GetChild(0).GetComponent<Button>();
        resumeButton.onClick.AddListener(ResumeButton);
        quitButton.onClick.AddListener(QuitButton);
    }

    // Maybe put pause function in later
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

    void QuitButton()
    {
        // Go back to main menu
        SceneManager.LoadScene(0);
    }

    void ResumeButton ()
    {
        this.gameObject.SetActive(false);
    }
}
