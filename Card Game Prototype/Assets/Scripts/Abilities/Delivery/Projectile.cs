using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : Delivery
{
    public bool constSpeed = true;
    public bool gravity = false;

    public Vector3 direction;
    public float speed = 5;

    public bool multiTargets = false;
    public int maxTargetsHit = 0;

    private bool hold = true;
    public float holdDelayTimer = 1;

    protected Rigidbody rb;
    protected bool forceapplied;
    protected bool curving;

    protected int currentTargetsHit = 0;

    protected float deathTimer = 0.2f;


    public override void Initialize()
    {
        base.Initialize();

        rb = GetComponent<Rigidbody>();
        forceapplied = false;
        rb.useGravity = gravity;
        if (holdDelayTimer > 0)
            StartCoroutine(HoldDelay(holdDelayTimer));
        else
            hold = false;
    }

    private IEnumerator HoldDelay (float waitTimer)
    {
        yield return new WaitForSeconds(waitTimer);
        hold = false;
    }

    protected void Move()
    {
        if (!hold)
        {
            if (constSpeed && curving)
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
    }

    protected override void AdditionalUpdates()
    {
        if (currentTargetsHit >= maxTargetsHit && maxTargetsHit != 0)
            Destroy(this.gameObject, deathTimer);

        if (!hold)
            base.AdditionalUpdates();
    }

    protected void AdditionalTriggerEvents(GameObject other)
    {
        // if hit object is a player
        if (other.tag == "Team 1" || other.tag == "Team 2")
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
    }
}