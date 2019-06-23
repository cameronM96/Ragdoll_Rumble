using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    public Button endTurnButton;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        endTurnButton = GetComponent<Button>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        endTurnButton.onClick.AddListener(TaskOnClick);
    }

    private void OnEnable()
    {
        GameManager.EnterCardPhase += ResetButton;
    }

    private void OnDisable()
    {
        GameManager.EnterCardPhase -= ResetButton;
    }

    void TaskOnClick()
    {
        endTurnButton.interactable = false;
        gameManager.EndTurn();
    }

    void ResetButton ()
    {
        endTurnButton.interactable = true;
    }
}
