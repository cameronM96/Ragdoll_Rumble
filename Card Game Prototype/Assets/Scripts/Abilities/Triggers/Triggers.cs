using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : ScriptableObject
{
    public SO_Ability ability;

    public enum TargetID { Enemies, Allies, All };
    public TargetID viableTargetID = TargetID.Enemies;

    public virtual void Initialise ()
    {

    }
    
    public void ApplyEffect(GameObject target)
    {
        if (target != null)
            ability.deliveryMethod.ApplyDelivery(target);

        //foreach (DeliverySO delivery in ability.deliveryMethod)
        //{
        //    delivery.ability = ability;
        //    delivery.triggeringTarget = target;
        //    delivery.ApplyDelivery();
        //}
    }
}
