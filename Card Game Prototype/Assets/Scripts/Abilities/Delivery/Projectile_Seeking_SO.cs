using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class Projectile_Seeking_SO : ProjectileSO
{
    public bool curve = false;
    public float maxSteerForce = 1;

    public bool uniqueTargetsOnly = true;
    public bool alternateTeams = false;

    public TargetSelection targetSelectionMethod = TargetSelection.Closest;
    public bool getClosestTarget = true;

    public float seekDelayTime = 0;

    public override void ApplyDelivery(GameObject target)
    {
        base.ApplyDelivery(target);

        foreach (GameObject projectile in projectileArray)
            Initialise(projectile);
    }

    public override void Initialise(GameObject targetObject)
    {
        base.Initialise(targetObject);

        Projectile_Seeking seekingProjectile = targetObject.GetComponent<Projectile_Seeking>();
        seekingProjectile.curve = curve;
        seekingProjectile.maxSteerForce = maxSteerForce;
        seekingProjectile.uniqueTargetsOnly = uniqueTargetsOnly;
        seekingProjectile.alternateTeams = alternateTeams;
        seekingProjectile.targetSelectionMethod = targetSelectionMethod;
        seekingProjectile.getClosestTarget = getClosestTarget;
        seekingProjectile.seekDelayTime = seekDelayTime;

    }
}
