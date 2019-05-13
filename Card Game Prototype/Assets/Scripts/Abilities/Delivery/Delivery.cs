using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    public enum TargetID { Enemies, Allies, All };
    public TargetID viableTargetID = TargetID.Enemies;

    public string friendlyTag = "Team 1";

    public float lifeSpan = 5;

    protected float timer = 0;

    public Triggers triggerEventScript;
    public Effect triggerEffectScript;

    private void OnEnable()
    {
        triggerEventScript.TriggeredEvent += TriggerEffect;
    }

    protected virtual void Initialize()
    {
        //friendlyTag = this.gameObject.tag;
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

    public void TriggerEffect()
    {
        triggerEffectScript.ApplyEffect();
    }
}
