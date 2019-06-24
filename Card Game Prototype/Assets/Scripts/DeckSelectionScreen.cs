using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DeckSelectionScreen : MonoBehaviour
{
    public GameObject ScreenContent;
    public static DeckSelectionScreen Instance;
    public GameObject[] deckSlots;
    public DeckPicker deckPicker;

    private void Awake()
    {
        Instance = this;
        HideScreen();
    }

    public void ShowDecks()
    {
        if (DecksStorage.Instance.allDecks.Count == 0)
        {
            HideScreen();
            DeckBuildingScreen.Instance.BuildADeck();
            return;
        }

        foreach (GameObject slot in deckSlots)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < DecksStorage.Instance.allDecks.Count; i++)
        {
            deckSlots[i].gameObject.SetActive(true);
        }
    }

    public void ShowScreen()
    {
        ScreenContent.SetActive(true);
        ShowDecks();
        deckPicker.OnOpen();
    }

    public void HideScreen()
    {
        ScreenContent.SetActive(false);
    }
}
