using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Stats : MonoBehaviour
{
    public int damage = 2;
    public int armour;
    public int hP = 25;
    public float speed = 20f;
    public float atkSpeed = 1;

    public Display_Base_Stats statDisplay;

    public void UpdateStatDisplay ()
    {
        statDisplay.UpdateStatText(this);
    }
}
