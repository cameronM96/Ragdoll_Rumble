using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class DeliverySO : ScriptableObject
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
}