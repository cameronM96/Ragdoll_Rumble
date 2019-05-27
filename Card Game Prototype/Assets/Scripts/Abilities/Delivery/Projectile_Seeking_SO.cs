using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Seeking_SO : ProjectileSO
{
    public bool curve = false;
    public float maxSteerForce = 1;

    public bool uniqueTargetsOnly = true;
    public bool alternateTeams = false;

    public enum TargetSelection { Closest, Farthest, Random };
    public TargetSelection targetSelectionMethod = TargetSelection.Closest;
    public bool getClosestTarget = true;
}
