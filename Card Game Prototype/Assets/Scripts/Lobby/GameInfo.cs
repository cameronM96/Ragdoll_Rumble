using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo
{
    private static string nextLevelName, returnLevelName, selectedDeckName;
    private static int nextLevelNumb, returnLevelNumb, campaignNumber, difficulty, rounds;

    public static string NextLevelName
    {
        get
        {
            return nextLevelName;
        }
        set
        {
            nextLevelName = value;
        }
    }

    public static string ReturnLevelName
    {
        get
        {
            return returnLevelName;
        }
        set
        {
            returnLevelName = value;
        }
    }

    public static string SelectedDeckName
    {
        get
        {
            return selectedDeckName;
        }
        set
        {
            selectedDeckName = value;
        }
    }

    public static int NextLevelNumb
    {
        get
        {
            return nextLevelNumb;
        }
        set
        {
            nextLevelNumb = value;
        }
    }

    public static int ReturnLevelNumb
    {
        get
        {
            return returnLevelNumb;
        }
        set
        {
            returnLevelNumb = value;
        }
    }

    public static int CampaignNumber
    {
        get
        {
            return campaignNumber;
        }
        set
        {
            campaignNumber = value;
        }
    }

    public static int Difficulty
    {
        get
        {
            return difficulty;
        }
        set
        {
            difficulty = value;
        }
    }

    public static int Rounds
    {
        get
        {
            return rounds;
        }
        set
        {
            rounds = value;
        }
    }
}
