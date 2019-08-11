using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public CollectionManager cM;
    public GameObject confirmationWindow;

    private void Start()
    {
        cM = transform.root.GetComponent<CollectionManager>();
    }

    public void Back()
    {
        if (!cM.deckSaved)
        {
            confirmationWindow.SetActive(true);
        }
        else
            ConfirmBack();
    }

    public void ConfirmBack()
    {
        if (cM.creatingDeck)
        {
            cM.LoadDeckButtons();
            cM.ToggleDeckCreation();
            cM.LoadCards(cM.creatingDeck);
            cM.deckSaved = true;
        }
        else
            SceneManager.LoadScene("StephenTestMaps");
    }
}
