using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Delivery
{
    public bool constSpeed = true;
    public bool gravity = false;

    public Vector3 direction;
    public float speed = 5;
    
    public bool bounce = false;
    public bool includeTargetsInBounce = true;
    public int maxBounces = 0;
    public bool piercing = true;
    public bool includeTargetsInPierce = true;
    public int maxPierces = 0;
    public bool multiTargets = false;
    public int maxTargetsHit = 0;

    protected Rigidbody rb;
    protected bool forceapplied;
    protected bool curving;

    protected int currentBounceNumb = 0;
    protected int currentPierceNumb = 0;
    protected int currentTargetsHit = 0;

    private float deathTimer = 0.2f;

    protected override void Initialize()
    {
        base.Initialize();

        rb = GetComponent<Rigidbody>();
        forceapplied = false;
        rb.useGravity = gravity;
    }

    protected void Move()
    {
        if (constSpeed)
        {
            // Curve Projectile
            rb.AddForce(direction * speed, ForceMode.Acceleration);
        }
        else if (constSpeed && !curving)
        {
            // Instantly change direction
            rb.velocity = Vector3.Normalize(direction) * speed;
        }
        else if (!constSpeed && !forceapplied)
        {
            // Apply force once
            rb.AddForce(direction * speed, ForceMode.Impulse);
            forceapplied = true;
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
    }

    protected override void AdditionalUpdates()
    {
        base.AdditionalUpdates();

        if (currentBounceNumb >= maxBounces && maxBounces != 0)
            Destroy(this.gameObject, deathTimer);

        if (currentPierceNumb >= maxPierces && maxPierces != 0)
            Destroy(this.gameObject, deathTimer);

        if (currentTargetsHit >= maxTargetsHit && maxTargetsHit != 0)
            Destroy(this.gameObject, deathTimer);
    }

    protected void AdditionalTriggerEvents(Collider other)
    {
        // if hit object is a player
        if ((other.gameObject.tag == "Team 1" || other.gameObject.tag == "Team 2"))
        {
            if (multiTargets)
            {
                //other.gameObject.GetComponent<Renderer>().material.color = Color.red;

                if (maxTargetsHit != 0)
                    currentTargetsHit++;
            }
            else
            {
                //Apply Effect here
                //other.gameObject.GetComponent<Renderer>().material.color = Color.red;
                Destroy(this.gameObject, deathTimer);
            }
        }
        else
        {
            //Debug.Log("Unknown Object!");
            // if not piercing
            if (!piercing)
                Destroy(this.gameObject, deathTimer);
        }
    }
}