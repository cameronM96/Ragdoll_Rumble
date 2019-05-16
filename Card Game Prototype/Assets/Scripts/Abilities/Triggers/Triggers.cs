using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : ScriptableObject
{
    public enum TargetID { Enemies, Allies, All };
    public TargetID viableTargetID = TargetID.Enemies;

    public Effect[] effects;
    
    public void ApplyEffect(GameObject target)
    {
        foreach (Effect effect in effects)
        {
            effect.TriggerEffect(target);
        }
    }
}
