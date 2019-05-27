using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySO : ScriptableObject
{
    public GameObject templateObj;

    public enum TargetID { Enemies, Allies, All };
    public TargetID viableTargetID = TargetID.Enemies;

    public string friendlyTag = "Team 1";
    public GameObject owner;
    public GameObject TriggeringTarget;

    public float lifeSpan = 5;

    public virtual void ApplyDelivery ()
    {

    }
}