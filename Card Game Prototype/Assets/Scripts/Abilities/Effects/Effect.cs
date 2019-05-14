using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{
    public GameObject particleEffect;
    public int effectLength = 0;

    protected int effectTimer = 0;

    public void TriggerEffect(GameObject target)
    {
        GameObject tempParticle = Instantiate(particleEffect,target.transform);
        tempParticle.GetComponent<ParticleSystem>().Play();
        if (effectLength <= 0)
            Destroy(tempParticle, tempParticle.GetComponent<ParticleSystem>().main.duration);
        else
            Destroy(tempParticle, effectLength);
        ApplyEffect(target);
    }

    protected virtual void ApplyEffect(GameObject target)
    {

    }
}
