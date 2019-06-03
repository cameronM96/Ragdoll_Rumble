using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base_Stats : MonoBehaviour
{
    public int attack = 2;
    public int armour;
    public int maxHP = 25;
    public float speed = 20f;
    public float atkSpeed = 1;

    [SerializeField] private int currentHP;
    public bool dead;
    public GameObject hitText;

    public Display_Base_Stats statDisplay;
    private List<GameObject> hitList = new List<GameObject>();
    public float hitInterval = 1;

    public List<OnTimer> onTimerList = new List<OnTimer>();
    public List<DeliverySO> onHitEffectsList = new List<DeliverySO>();
    public List<DeliverySO> onGetHitEffectsList = new List<DeliverySO>();
    public List<OnDamaged> onDamagedList = new List<OnDamaged>();
    public List<OnHealth> onHealthList = new List<OnHealth>();

    private void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        currentHP = maxHP;

        // Initialise Triggers
        if (onTimerList.Count > 0)
        {
            foreach (OnTimer onTimer in onTimerList)
                onTimer.StartTimer(this.gameObject);
        }
        if (onDamagedList.Count > 0)
        {
            foreach (OnDamaged onDamaged in onDamagedList)
                onDamaged.Initialise();
        }
        if (onHealthList.Count > 0)
        {
            foreach (OnHealth onHealth in onHealthList)
                onHealth.Initialise();
        }
    }

    public void UpdateStatDisplay()
    {
        statDisplay.UpdateStatText(this);
    }

    // *************************** In Combat Events ***************************
    // Only lets the player hit someone once every x seconds
    IEnumerator RecentlyHit (float hitDelay, GameObject hitTarget)
    {
        hitList.Add(hitTarget);
        yield return new WaitForSeconds(hitDelay);
        hitList.Remove(hitTarget);
    }

    public void TakeDamage(int damage, bool ability)
    {
        int damageTaken = 0;
        if (ability)
        {
            // Taking Ability Damage (not reduced by armour)
            currentHP -= damage;

            SpawnHitText(Color.blue, damage);

            if (currentHP <= 0)
            {
                currentHP = 0;
                Dead();
                return;
            }

            damageTaken = damage;
        }
        else
        {
            // Taking Weapon Damage (Reduced by armour, min 2)
            int newDamageAmount = damage - armour;
            if (newDamageAmount < 2)
                newDamageAmount = 2;

            currentHP -= newDamageAmount;

            SpawnHitText(Color.red, newDamageAmount);

            if (currentHP <= 0)
            {
                currentHP = 0;
                Dead();
                return;
            }

            damageTaken = newDamageAmount;

            // Apply OnHit Events
            OnGetHit();
        }

        // Apply OnHit Events
        OnDamaged(damageTaken);
        OnHealth();
    }

    public void TakeHeal (int heal)
    {
        // Healing
        if (!dead)
        {
            currentHP += heal;

            SpawnHitText(Color.green, heal);

            if (currentHP > maxHP)
                currentHP = maxHP;
        }
    }

    // Kill player
    public void Dead()
    {
        Debug.Log(this.gameObject.name + " has Died!");
        dead = true;
        GetComponent<Renderer>().material.color = Color.black;
        //GetComponent<Collider>().enabled = false;
    }

    // *************************** Trigger Events ***************************
    public void OnHit(GameObject target)
    {
        // Deal damage if it is hurtable
        if (target.GetComponent<Base_Stats>() != null)
            target.GetComponent<Base_Stats>().TakeDamage(attack, false);

        // Apply on hit effects
        if (!hitList.Contains(target))
        {
            StartCoroutine(RecentlyHit(hitInterval, target));

            if (onHitEffectsList.Count > 0)
            {
                foreach (DeliverySO deliveryMethod in onHitEffectsList)
                {
                    deliveryMethod.triggeringTarget = target;
                    deliveryMethod.ApplyDelivery();
                }
            }
        }
    }

    private void OnGetHit()
    {
        if (onGetHitEffectsList.Count > 0)
        {
            foreach (DeliverySO deliveryMethod in onHitEffectsList)
            {
                //deliveryMethod.triggeringTarget = target;
                deliveryMethod.ApplyDelivery();
            }
        }
    }

    private void OnDamaged(int damage)
    {
        if (onDamagedList.Count > 0)
        {
            foreach (OnDamaged trigger in onDamagedList)
                trigger.CheckIfEnoughDamaged(this.gameObject, damage, maxHP);
        }
    }

    private void OnHealth()
    {
        if (onHealthList.Count > 0)
        {
            foreach (OnHealth trigger in onHealthList)
                trigger.CheckIfTriggered(this.gameObject, currentHP, maxHP);
        }
    }

    // *************************** Buffs/Debuffs/Card playing events ***************************
    public void ChangeMaxHealth(int healthChange)
    {
        ChangeMaxHealth(healthChange, false, false);
    }

    public void ChangeMaxHealth(int healthChange, bool changeCurrentHealth)
    {
        ChangeMaxHealth(healthChange, changeCurrentHealth, false);
    }

    public void ChangeMaxHealth (int healthChange, bool changeCurrentHealth, bool multiplier)
    {
        if (multiplier)
        {
            maxHP *= healthChange;

            // Change current health aswell
            if (changeCurrentHealth)
            {
                // Damage or Heal?
                if (healthChange > 0)
                {
                    int newHealth = (currentHP * healthChange) - currentHP;
                    TakeHeal(newHealth);
                }
                else
                {
                    int newHealth = currentHP - (currentHP / Mathf.Abs(healthChange));
                    TakeDamage(newHealth, true);
                }
            }
        }
        else
        {
            maxHP += healthChange;

            // Change current health aswell
            if (changeCurrentHealth)
            {
                // Damage or Heal?
                if (healthChange > 0)
                    TakeHeal(healthChange);
                else
                    TakeDamage(Mathf.Abs(healthChange), true);
            }
        }

        // Never let current HP be greater than maxHP
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void ChangeArmour(int armourChange)
    {
        ChangeArmour(armourChange, false);
    }

    public void ChangeArmour(int armourChange, bool multiplier)
    {
        if (multiplier)
        {
            // Damage or Heal?
            if (armourChange > 0)
                armour *= armourChange;
            else
                armour /= Mathf.Abs(armourChange);
        }
        else
        {
            armour += armourChange;
        }
    }

    public void ChangeAttack(int attackChange)
    {
        ChangeAttack(attackChange, false);
    }

    public void ChangeAttack(int attackChange, bool multiplier)
    {
        if (multiplier)
        {
            // Damage or Heal?
            if (attackChange > 0)
                attack *= attackChange;
            else
                attack /= Mathf.Abs(attackChange);
        }
        else
        {
            attack += attackChange;
        }
    }

    public void ChangeSpeed(int speedChange)
    {
        ChangeSpeed(speedChange, false);
    }

    public void ChangeSpeed(int speedChange, bool multiplier)
    {
        if (multiplier)
        {
            // Damage or Heal?
            if (speedChange > 0)
                speed *= speedChange;
            else
                speed /= Mathf.Abs(speedChange);
        }
        else
        {
            speed += speedChange;
        }

        SpawnHitText(Color.blue, speedChange);
    }

    public void ChangeAttackSpeed(int attackSpeedChange)
    {
        ChangeAttackSpeed(attackSpeedChange, false);
    }

    public void ChangeAttackSpeed(int attackSpeedChange, bool multiplier)
    {
        if (multiplier)
        {
            // Damage or Heal?
            if (attackSpeedChange > 0)
                atkSpeed *= attackSpeedChange;
            else
                atkSpeed /= Mathf.Abs(attackSpeedChange);
        }
        else
        {
            atkSpeed += attackSpeedChange;
        }
    }

    private void SpawnHitText(Color newColour, int value)
    {
        GameObject newHitText = Instantiate(hitText);
        newHitText.transform.position = this.transform.position;
        Text TextElement = newHitText.transform.GetChild(0).GetComponentInChildren<Text>();
        TextElement.text = value.ToString();
        TextElement.color = newColour;
    }
}
