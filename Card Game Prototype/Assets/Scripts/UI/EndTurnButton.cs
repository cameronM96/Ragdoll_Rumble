using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    public Button endTurnButton;
    public GameManager gameManager;

    private bool initialised = false;
    // Start is called before the first frame update
    void Start()
    {
        endTurnButton = GetComponent<Button>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        endTurnButton.onClick.AddListener(TaskOnClick);
        initialised = true;
        ResetButton();
    }

    private void OnEnable()
    {
        //Debug.Log("Enabling Button");
        if (initialised)
            ResetButton();
    }

    void TaskOnClick()
    {
        //Debug.Log("Make Button non interactable");
        endTurnButton.interactable = false;
        gameManager.EndTurn();
    }

    public void ResetButton ()
    {
        //Debug.Log("Make Button Interactable");
        endTurnButton.interactable = true;
    }
}
