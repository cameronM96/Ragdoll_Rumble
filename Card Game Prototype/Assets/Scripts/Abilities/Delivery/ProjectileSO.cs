using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSO : DeliverySO
{
    public bool constSpeed = true;
    public bool gravity = false;

    public Vector3 direction;
    public float speed = 5;

    public bool multiTargets = false;
    public int maxTargetsHit = 0;
}
