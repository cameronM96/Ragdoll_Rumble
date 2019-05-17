using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Trigger/OnDamaged")]
public class OnDamaged : Triggers
{
    public bool percent = false;
    public int damageNeeded;

    private int damageTaken;

    public override void Initialise()
    {
        base.Initialise();

        damageTaken = 0;
    }

    public void CheckIfEnoughDamaged(GameObject target, int damage, int maxhealth)
    {
        if (percent)
        {
            damageTaken += (damage * 100 / maxhealth) ;

            if (damageTaken >= damageNeeded)
            {
                damageTaken -= damageNeeded;

                ApplyEffect(target);
            }
        }
        else {
            damageTaken += damage;
            if (damageTaken >= damageNeeded)
            {
                // Reset Counter
                damageTaken -= damageNeeded;
                //Debug.Log("OnDamaged Triggered");

                ApplyEffect(target);
            }
        }
    }
}
