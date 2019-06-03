using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Trigger/OnButton")]
public class OnButton : Triggers
{
    //public GameObject target;
    public Sprite buttonImage;
    public bool oncePerRound = false;
    public float coolDown = 30f;

    public override void Initialise()
    {
        base.Initialise();
    }
}