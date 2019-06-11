using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class Delivery : MonoBehaviour
{
    public TargetID viableTargetID = TargetID.Enemies;

    public string friendlyTag = "Team 1";
    public GameObject owner;
    public GameObject TriggeringTarget;

    public float lifeSpan = 5;

    protected float timer = 0;

    private bool initialised = false;

    public virtual void Initialize()
    {
        //friendlyTag = this.gameObject.tag;
        initialised = true;
    }

    protected virtual void AdditionalUpdates()
    {
        if (lifeSpan != 0)
        {
            timer += Time.deltaTime;
            if (timer >= lifeSpan)
                Destroy(this.gameObject);
        }
    }
}