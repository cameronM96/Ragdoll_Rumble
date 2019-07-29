using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyDeckButton : MonoBehaviour
{
    public string myDeckName;
    public LobbyManager myLobbyManager;

    public void Initialise(string deckName, LobbyManager lobbyManager)
    {
        myDeckName = deckName;
        myLobbyManager = lobbyManager;
    }

    public void SelectDeck()
    {
        if (myLobbyManager.LoadDeck(myDeckName))
            LevelHolder.SelectedDeckName = myDeckName;
    }
}