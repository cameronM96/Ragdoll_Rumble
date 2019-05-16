using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Trigger/OnDamaged")]
public class OnDamaged : Triggers
{
    public int damageNeeded;
    private int damageTaken;

    public void CheckIfEnoughDamaged(GameObject target, int damage)
    {
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
