using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Effect/Instant_Effect")]
public class InstantEffect : Effect
{
    protected override void ApplyEffect(GameObject target)
    {
        ChangeStat(target);

        ApplyCC(target);

        SpawnParticleEffect(target);
    }
}
