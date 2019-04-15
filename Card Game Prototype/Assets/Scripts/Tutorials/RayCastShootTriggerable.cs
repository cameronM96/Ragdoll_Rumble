using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastShootTriggerable : MonoBehaviour
{
    [HideInInspector] public int gunDamage = 1;
    [HideInInspector] public float weaponRange = 50f;
    [HideInInspector] public float hitForce = 100f;
    public Transform gunEnd;
    [HideInInspector] public LineRenderer laserLine;

    private Camera fpsCam;
    private WaitForSeconds shotDuraction = new WaitForSeconds(.07f);
    private AudioSource gunAudio;

    public void Initialize()
    {

    }

    public void Fire()
    {

    }
}
