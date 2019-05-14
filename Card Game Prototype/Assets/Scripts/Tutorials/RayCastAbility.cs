using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastAbility : Ability
{
    public int gunDamage = 1;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Color laserColor = Color.white;

    private RayCastShootTriggerable rcShoot;

    public override void Initialize(GameObject obj)
    {
        rcShoot = obj.GetComponent<RayCastShootTriggerable>();
        rcShoot.Initialize();

        rcShoot.gunDamage = gunDamage;
        rcShoot.weaponRange = weaponRange;
        rcShoot.hitForce = hitForce;
        rcShoot.laserLine.material = new Material(Shader.Find("Unlit/Color"));
        rcShoot.laserLine.material.color = laserColor;

        throw new System.NotImplementedException();
    }

    public override void TriggerAbility()
    {
        rcShoot.Fire();

        throw new System.NotImplementedException();
    }
}
