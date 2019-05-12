using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Stats : MonoBehaviour
{
    public int damage = 2;
    public int armour;
    public int maxHP = 25;
    public float speed = 20f;
    public float atkSpeed = 1;

    private int currentHP;
    public bool dead;

    public Display_Base_Stats statDisplay;

    private void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        currentHP = maxHP;
    }

    public void UpdateStatDisplay()
    {
        statDisplay.UpdateStatText(this);
    }

    public void ChangeHealth(bool heal, int healthChange, bool ability)
    {

        if (!heal)
        {
            // Taking Damage
            if (ability)
            {
                // Taking Ability Damage (not reduced by armour)
                currentHP -= healthChange;

                if (currentHP <= 0)
                {
                    currentHP = 0;
                    Dead();
                }
            }
            else
            {
                // Taking Weapon Damage (Reduced by armour)
                int newDamageAmount = healthChange - armour;
                if (newDamageAmount < 2)
                    newDamageAmount = 2;

                currentHP -= newDamageAmount;

                if (currentHP <= 0)
                {
                    currentHP = 0;
                    Dead();
                }
            }

        }
        else
        {
            // Healing
            if (!dead)
            {
                currentHP += healthChange;
                if (currentHP > maxHP)
                    currentHP = maxHP;
            }
        }
    }

    public void ChangeArmour(int armourChange)
    {
        armour += armourChange;
    }

    public void ChangeDamage(int damageChange)
    {
        damage += damageChange;
    }

    public void ChangeSpeed(int speedChange)
    {
        speed += speedChange;
    }

    public void ChangeAttackSpeed(int attackSpeedChange)
    {
        atkSpeed += attackSpeedChange;
    }

    public void Dead()
    {
        dead = true;
    }
}
