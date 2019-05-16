using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Trigger/OnButton")]
public class OnButton : Triggers
{
    private void OnGUI()
    {
        if(GUI.Button(new Rect (Screen.width / 2 - 50, 5, 100, 30), "Click"))
        {
            //ApplyEffect(this.gameObject);
        }
    }
}