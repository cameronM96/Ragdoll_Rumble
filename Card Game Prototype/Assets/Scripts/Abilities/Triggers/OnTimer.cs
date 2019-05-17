using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Trigger/OnTimer")]
public class OnTimer : Triggers
{
    public float delay;

    public void StartTimer (GameObject target)
    {
        // Add component that holds the ticking timer and calls this object to apply the effect
        OnTimerManager checkIfExists = target.GetComponent<OnTimerManager>();

        // If none exist add one
        if (checkIfExists == null)
            target.AddComponent<OnTimerManager>().SetTimer(this, delay, target);
        else
            checkIfExists.SetTimer(this, delay, target);
    }
}
