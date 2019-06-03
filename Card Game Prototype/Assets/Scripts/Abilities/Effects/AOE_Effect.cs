using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Effect/AOE_Effect")]
public class AOE_Effect : Effect
{
    public TargetID viableTargetID = TargetID.All;

    public float radius = 1f;

    public GameObject aoeParticleEffect;
    
    protected override void ApplyEffect(GameObject target)
    {
        base.ApplyEffect(target);

        List<GameObject> hitTargets = AOE(target.transform.position);

        foreach (GameObject hitTarget in hitTargets)
        {
            ChangeStat(hitTarget);

            SpawnParticleEffect(hitTarget);
        }
    }

    private List<GameObject> AOE (Vector3 aoeLocation)
    {
        // Spawn Particle effect
        if (aoeParticleEffect != null)
        {
            GameObject tempParticle = Instantiate(aoeParticleEffect, aoeLocation, new Quaternion(0,Random.Range(-180,180),0,0));

            tempParticle.GetComponent<ParticleSystem>().Play();
            if (effectLength <= 0)
                Destroy(tempParticle, tempParticle.GetComponent<ParticleSystem>().main.duration);
            else
                Destroy(tempParticle, effectLength);
        }

        // AOEhit effect
        List<GameObject> hitTargets = new List<GameObject>();

        Collider[] hitColliders = Physics.OverlapSphere(aoeLocation, radius);

        foreach (Collider collider in hitColliders)
        {
            GameObject newObject = collider.transform.root.gameObject;
            if (!hitTargets.Contains(newObject))
                hitTargets.Add(newObject);
        }

        return hitTargets;
    }
}
