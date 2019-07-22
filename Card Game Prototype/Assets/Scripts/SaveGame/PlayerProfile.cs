﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    public string playerName;
    public int coins;
    public int gems;
    public Dictionary<int,int> myCards;
    public Dictionary<string, int[]> myDecks;
    public Dictionary<int, int> myUnopenedPacks;
    public int campaignProgress;

    public PlayerProfile (Player player)
    {
        playerName = player.playerName;
        coins = player.coins;
        gems = player.gems;
        myCards = player.myCards;
        myDecks = player.myDecks;
        campaignProgress = player.campaignProgress;
        myUnopenedPacks = player.myUnopenedPacks;
    }
}
