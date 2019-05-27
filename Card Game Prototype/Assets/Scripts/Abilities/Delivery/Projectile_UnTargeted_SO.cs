using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_UnTargeted_SO : ProjectileSO
{
    public bool bounce = false;
    public bool includeTargetsInBounce = true;
    public int maxBounces = 0;
    public bool piercing = true;
    public bool includeTargetsInPierce = true;
    public int maxPierces = 0;
}
