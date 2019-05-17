using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base_Stats : MonoBehaviour
{
    public int damage = 2;
    public int armour;
    public int maxHP = 25;
    public float speed = 20f;
    public float atkSpeed = 1;

    private int currentHP;
    public bool dead;
    public GameObject hitText;

    public Display_Base_Stats statDisplay;
    private List<GameObject> hitList = new List<GameObject>();
    public float hitInterval = 1;

    public List<OnTimer> onTimerList = new List<OnTimer>();
    public List<Effect> onHitEffectsList = new List<Effect>();
    public List<Effect> onGetHitEffectsList = new List<Effect>();
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
        }

        // Apply OnHit Events
        OnGetHit();
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
        dead = true;
    }

    // *************************** Trigger Events ***************************
    public void OnHit(GameObject target)
    {
        // Deal damage if it is hurtable
        if (target.GetComponent<Base_Stats>() != null)
            target.GetComponent<Base_Stats>().TakeDamage(damage, false);

        // Apply on hit effects
        if (!hitList.Contains(target))
        {
            StartCoroutine(RecentlyHit(hitInterval, target));

            if (onHitEffectsList.Count > 0)
            {
                foreach (Effect effect in onHitEffectsList)
                    effect.TriggerEffect(target);
            }
        }
    }

    private void OnGetHit()
    {
        if (onGetHitEffectsList.Count > 0)
        {
            foreach (Effect effect in onGetHitEffectsList)
                effect.TriggerEffect(this.gameObject);
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
    public void ChangeMaxHealth (int healthChange, bool changeCurrentHealth)
    {
        maxHP += healthChange;
        if (changeCurrentHealth) {
            if (healthChange > 0)
                TakeHeal(healthChange);
            else
                TakeDamage(healthChange, true);
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

    private void SpawnHitText(Color newColour, int value)
    {
        GameObject newHitText = Instantiate(hitText);
        newHitText.transform.position = this.transform.position;
        Text TextElement = newHitText.transform.GetChild(0).GetComponentInChildren<Text>();
        TextElement.text = value.ToString();
        TextElement.color = newColour;
    }
}
