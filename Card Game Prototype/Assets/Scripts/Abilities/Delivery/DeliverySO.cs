﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public abstract class DeliverySO : ScriptableObject
{
    public SO_Ability ability;

    public TargetID viableTargetID = TargetID.Enemies;

    [HideInInspector] public string friendlyTag = "Team 1";
    [HideInInspector] public GameObject owner;
    [HideInInspector] public GameObject triggeringTarget;

    public float lifeSpan = 5;

    public virtual void ApplyDelivery (GameObject target)
    {
        triggeringTarget = target;
    }

    public virtual void Initialise(GameObject targetObject)
    {
        Delivery delivery = targetObject.GetComponent<Delivery>();

        if (delivery != null)
        {
            delivery.viableTargetID = viableTargetID;
            delivery.owner = owner;
            delivery.TriggeringTarget = triggeringTarget;
            delivery.lifeSpan = lifeSpan;
        }
        else
            Debug.Log("Failed to find Delivery!");
    }
}