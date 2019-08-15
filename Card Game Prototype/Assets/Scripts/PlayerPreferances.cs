using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPreferances
{
    private static string master = "master";
    private static string music = "music";
    private static string sfx = "sfx";
    private static string vocals = "vocals";
    private static float masterValue;
    private static float musicValue;
    private static float sfxValue;
    private static float vocalsValue;

    public static void SavePrefs()
    {
        PlayerPrefs.SetFloat(master, masterValue);
        PlayerPrefs.SetFloat(music, musicValue);
        PlayerPrefs.SetFloat(sfx, sfxValue);
        PlayerPrefs.SetFloat(vocals, vocalsValue);
        PlayerPrefs.Save();

        LoadPrefs();
    }

    public static void LoadPrefs()
    {
        masterValue = PlayerPrefs.GetFloat(master);
        musicValue = PlayerPrefs.GetFloat(music);
        sfxValue = PlayerPrefs.GetFloat(sfx);
        vocalsValue = PlayerPrefs.GetFloat(vocals);
    }

    public static float Master
    {
        get
        {
            return masterValue;
        }
        set
        {
            masterValue = value;
        }
    }

    public static float Music
    {
        get
        {
            return musicValue;
        }
        set
        {
            musicValue = value;
        }
    }

    public static float SFX
    {
        get
        {
            return sfxValue;
        }
        set
        {
            sfxValue = value;
        }
    }

    public static float Vocals
    {
        get
        {
            return vocalsValue;
        }
        set
        {
            vocalsValue = value;
        }
    }
}
