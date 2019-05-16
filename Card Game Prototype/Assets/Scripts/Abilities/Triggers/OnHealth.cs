using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Trigger/OnHealth")]
public class OnHealth : Triggers
{
    public bool percent = false;
    public int healthValue;
    public bool triggerOnce = true;

    private bool setoff = false;

    public void CheckIfTriggered (GameObject target, int currentHealth, int MaxHealth)
    {
        if (!setoff)
        {
            if (percent)
            {
                // Get healthPercent
                int currentPercent = currentHealth / MaxHealth;
                if (currentHealth % MaxHealth != 0)
                    ++currentPercent;

                if (currentPercent <= healthValue)
                {
                    setoff = true;
                    ApplyEffect(target);
                }
            }
            else
            {
                if(currentHealth <= healthValue)
                {
                    setoff = true;
                    ApplyEffect(target);
                }
            }
        }
    }
}
