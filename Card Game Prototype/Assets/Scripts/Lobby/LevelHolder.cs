using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelHolder
{
    private static string nextLevelName, returnLevelName, selectedDeckName;
    private static int nextLevelNumb, returnLevelNumb;

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
}
