﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display_Base_Stats : MonoBehaviour
{
    public Text atktxt;
    public Text armourtxt;
    public Text hPtxt;
    public Text mSpeedtxt;
    public Text atkSpeedtxt;

    public void UpdateStatText (Base_Stats bstats)
    {
        atktxt.text = ": " + bstats.attack;
        armourtxt.text = ": " + bstats.armour;
        hPtxt.text = ": " + bstats.maxHP;
        mSpeedtxt.text = ": " + bstats.speed;
        atkSpeedtxt.text = ": " + bstats.atkSpeed;
    }
}
