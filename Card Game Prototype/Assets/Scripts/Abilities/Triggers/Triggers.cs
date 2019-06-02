﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : ScriptableObject
{
    public SO_Ability ability;

    public enum TargetID { Enemies, Allies, All };
    public TargetID viableTargetID = TargetID.Enemies;

    public DeliverySO[] deliveryMethods;

    public virtual void Initialise ()
    {

    }
    
    public void ApplyEffect(GameObject target)
    {
        foreach (DeliverySO delivery in deliveryMethods)
        {
            delivery.ability = ability;
            delivery.triggeringTarget = target;
            delivery.ApplyDelivery();
        }
    }
}
