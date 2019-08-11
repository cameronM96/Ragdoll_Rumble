using System.Collections;
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
    public string defaultDeckName;

    public PlayerProfile (Player player)
    {
        playerName = player.playerName;
        coins = player.Coins;
        gems = player.Gems;
        myCards = player.MyCards;
        myDecks = player.MyDecks;
        campaignProgress = player.CampaignProgress;
        myUnopenedPacks = player.MyUnopenedPacks;
        defaultDeckName = player.DefaultDeckName;
    }
}
