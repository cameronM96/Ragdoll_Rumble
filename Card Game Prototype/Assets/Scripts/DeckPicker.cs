using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckPicker : MonoBehaviour
{
    public Button playButton;
    public Button buildDeckButton;
    public DeckSlot selectedDeck { get; set; }

    void Awake()
    {
        OnOpen();
    }

    public void OnOpen()
    {
        SelectDeck(null);
    }

    public void SelectDeck(DeckSlot deck)
    {
        if (deck == null||selectedDeck ==deck||!deck.deckInformation.IsComplete())
        {
            selectedDeck = null;
            if (playButton!=null)
            {
                playButton.interactable = false;
            }
        }
        else
        {
            selectedDeck = deck;
            BattleStartInfo.selectedDeck = selectedDeck.deckInformation;
            if (playButton!=null)
            {
                playButton.interactable = true;
            }
        }
    }

    public void GoToDeckBuilding()
    {
        DeckBuildingScreen.Instance.BuildADeck();
    }
}
