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

    public override void Initialise()
    {
        base.Initialise();

        setoff = false;
    }

    public void CheckIfTriggered (GameObject target, int currentHealth, int MaxHealth)
    {
        if (!setoff)
        {
            if (percent)
            {
                // Get healthPercent
                int currentPercent = (currentHealth * 100 / MaxHealth) ;

                Debug.Log("Current %: " + currentPercent);

                if (currentPercent <= healthValue)
                {
                    Debug.Log("Ability Triggered");
                    ApplyEffect(target);
                    if (triggerOnce)
                        setoff = true;
                }
            }
            else
            {
                Debug.Log("Health: " + currentHealth);

                if (currentHealth <= healthValue)
                {
                    Debug.Log("Ability Triggered");
                    ApplyEffect(target);
                    if (triggerOnce)
                        setoff = true;
                }
            }
        }
    }
}
