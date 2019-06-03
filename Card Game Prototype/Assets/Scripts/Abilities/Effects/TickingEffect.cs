using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Effect/Ticking_Effect")]
public class TickingEffect : Effect
{
    public int effectFrequency = 0;
    public bool applyTickOnStart = false;
    public bool applyTickOnEnd = false;
    // Scriptable Objects don't have update so Coroutines don't work here.
    // Extend to a monoBehaviour to get to work.
    protected override void ApplyEffect(GameObject target)
    {
        // Add component that holds the ticking timer and calls this object to apply the effect
        TickTimer[] checkIfExists = target.GetComponents<TickTimer>();

        // If none exist add one
        if (checkIfExists == null)
            target.AddComponent<TickTimer>().SetTimer(1, effectFrequency, effectLength, this);

        // Script exists but check if it's for this effect
        bool noneFound = true;
        foreach (TickTimer timer in checkIfExists)
        {
            if (timer.targetEffect == this)
            {
                timer.SetTimer(1, effectFrequency, effectLength, this);
                noneFound = false;
            }
        }

        // Script exist but it wasn't for this effect so add a new one
        if (noneFound)
            target.AddComponent<TickTimer>().SetTimer(1, effectFrequency, effectLength, this);
    }

    public void TickEffect(GameObject target)
    {
        ChangeStat(target);

        SpawnParticleEffect(target);
    }
}
