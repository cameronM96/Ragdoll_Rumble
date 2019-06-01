using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{
    public enum StatChange { None, Damage, Heal, MaxHealth, Attack, Armour, Speed, AttackSpeed}
    public StatChange statChange = StatChange.Damage;
    public int statChangeValue = 0;
    public bool changeCurrentHP = false;
    public bool multipler = false;

    public GameObject particleEffect;
    public int effectLength = 0;

    protected int effectTimer = 0;

    public void TriggerEffect(GameObject target)
    {
        // Spawn Particle effect
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

    // Change the stats of the target
    protected void ChangeStat (GameObject target)
    {
        if (target.GetComponent<Base_Stats>() != null)
        {
            Base_Stats targetStats = target.GetComponent<Base_Stats>();
            switch (statChange)
            {
                case StatChange.None:
                    break;
                case StatChange.Damage:
                    targetStats.TakeDamage(statChangeValue, true);
                    break;
                case StatChange.Heal:
                    targetStats.TakeHeal(statChangeValue);
                    break;
                case StatChange.MaxHealth:
                    targetStats.ChangeMaxHealth(statChangeValue,changeCurrentHP,multipler);
                    break;
                case StatChange.Attack:
                    targetStats.ChangeAttack(statChangeValue, multipler);
                    break;
                case StatChange.Armour:
                    targetStats.ChangeArmour(statChangeValue, multipler);
                    break;
                case StatChange.Speed:
                    targetStats.ChangeSpeed(statChangeValue, multipler);
                    break;
                case StatChange.AttackSpeed:
                    targetStats.ChangeAttackSpeed(statChangeValue, multipler);
                    break;
                default:
                    break;
            }
        }
    }
}
