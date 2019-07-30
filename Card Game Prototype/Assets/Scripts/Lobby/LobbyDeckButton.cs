using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyDeckButton : MonoBehaviour
{
    public Text buttonText;
    public string myDeckName;
    public LobbyManager myLobbyManager;

    public void Initialise(string deckName, LobbyManager lobbyManager)
    {
        myDeckName = deckName;
        myLobbyManager = lobbyManager;
        buttonText.text = myDeckName;
    }

    public void SelectDeck()
    {
        if (myLobbyManager.LoadDeck(myDeckName))
            GameInfo.SelectedDeckName = myDeckName;
    }
}