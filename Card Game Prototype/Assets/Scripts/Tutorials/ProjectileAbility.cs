using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ProjectileAbility")]
public class ProjectileAbility : Ability
{
    public float projectileFore = 500f;
    public Rigidbody projectile;

    private ProjectileShootTriggerable launcher;

    public override void Initialize(GameObject obj)
    {
        launcher = obj.GetComponent<ProjectileShootTriggerable>();
        launcher.projectileForce = projectileFore;

        throw new System.NotImplementedException();
    }

    public override void TriggerAbility()
    {
        launcher.Launch();

        throw new System.NotImplementedException();
    }
}
