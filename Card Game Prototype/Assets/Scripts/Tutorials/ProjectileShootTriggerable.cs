using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShootTriggerable : MonoBehaviour
{
    [HideInInspector] public Rigidbody projectile;
    public Transform bulletSpawn;
    [HideInInspector] public float projectileForce = 500f;

    public void Launch()
    {

    }
}
