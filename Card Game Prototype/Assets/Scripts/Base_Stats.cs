using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
using UnityEngine.AI;

[RequireComponent(typeof(StateController))]
public class Base_Stats : MonoBehaviour
{
    public bool isAI = false;

    public int attack = 2;
    public int armour;
    public int maxHP = 25;
    public float speed = 20f;
    // Need to figure out how to turn this into an attackrate (as atkspeed increases, time between attack decrease)
    public float atkSpeed = 5;

    public float attackRange = 1f;

    private float[] storedBaseStats = new float[5];

    [SerializeField] private int currentHP;
    public bool dead = false;
    public GameObject hitText;

    public Display_Base_Stats statDisplay;
    private List<GameObject> hitList = new List<GameObject>();
    public float hitInterval = 1;

    public List<OnTimer> onTimerList = new List<OnTimer>();
    public List<DeliverySO> onHitEffectsList = new List<DeliverySO>();
    public List<DeliverySO> onGetHitEffectsList = new List<DeliverySO>();
    public List<OnDamaged> onDamagedList = new List<OnDamaged>();
    public List<OnHealth> onHealthList = new List<OnHealth>();

    public GameManager gameManager;
    public StateController stateController;
    public AIController audioController;
    public GameObject[] slots;

    [HideInInspector] public bool stunned = false;
    private float stunLength;
    [HideInInspector] public bool snared = false;
    private float snareLength;

    private void Start()
    {
        stateController = GetComponent<StateController>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioController = GetComponent<AIController>();
        gameManager.PlayerJoined();
    }

    private void Update()
    {
        if(stunned)
        {
            if(stunLength <= 0)
            {
                stunned = false;
                // Undo stun here
                GetComponent<NavMeshAgent>().speed = speed;
            }
        }

        if (snared)
        {
            if (snareLength <= 0)
            {
                snared = false;
                // Undo snare here
                GetComponent<NavMeshAgent>().speed = speed;
            }
        }
    }

    private void OnEnable()
    {
        GameManager.EnterCombatPhase += InitialiseCombatPhase;
        GameManager.EnterCardPhase += InitialiseCardPhase;
        GameManager.EndGame += GameIsOver;
    }

    private void OnDisable()
    {
        GameManager.EnterCombatPhase -= InitialiseCombatPhase;
        GameManager.EnterCardPhase -= InitialiseCardPhase;
        GameManager.EndGame -= GameIsOver;
    }

    public int GetHealth()
    {
        return currentHP;
    }

    public void InitialiseCombatPhase()
    {
        //Debug.Log("Initialising Player for Combat Phase");
        // Save current Stats to set back to in cardphase (Stops in combat changes from being permanent
        storedBaseStats[0] = attack; 
        storedBaseStats[1] = armour;
        storedBaseStats[2] = maxHP;
        storedBaseStats[3] = speed;
        storedBaseStats[4] = atkSpeed;

        stunned = false;
        snared = false;
        GetComponent<NavMeshAgent>().speed = speed;
        // Unkill Player
        currentHP = maxHP;
        dead = false;

        // Initialise Triggers
        if (onTimerList.Count > 0)
        {
            foreach (OnTimer onTimer in onTimerList)
            {
                onTimer.ability.deliveryMethod.owner = this.gameObject;
                onTimer.StartTimer(this.gameObject);
            }
        }

        if (onDamagedList.Count > 0)
        {
            foreach (OnDamaged onDamaged in onDamagedList)
            {
                onDamaged.ability.deliveryMethod.owner = this.gameObject;
                onDamaged.Initialise();
            }
        }

        if (onHealthList.Count > 0)
        {
            foreach (OnHealth onHealth in onHealthList)
            {
                onHealth.ability.deliveryMethod.owner = this.gameObject;
                onHealth.Initialise();
            }
        }

        if (onHitEffectsList.Count > 0)
        {
            foreach (DeliverySO delivery in onHitEffectsList)
                delivery.owner = this.gameObject;
        }

        if (onGetHitEffectsList.Count > 0)
        {
            foreach (DeliverySO delivery in onGetHitEffectsList)
                delivery.owner = this.gameObject;
        }
    }

    public void InitialiseCardPhase()
    {
        StopAllCoroutines();
        //Debug.Log("Initialising Player to Card Phase");
        // Reload Saved stats (Reset stats back to end of last cardPhase)
        if (gameManager.currentRound > 1)
        {
            attack = Mathf.RoundToInt(storedBaseStats[0]);
            armour = Mathf.RoundToInt(storedBaseStats[1]);
            maxHP = Mathf.RoundToInt(storedBaseStats[2]);
            speed = storedBaseStats[3];
            atkSpeed = storedBaseStats[4];
        }

        dead = false;
    }

    public void GameIsOver()
    {
        if ("Team: " + gameManager.winningTeam == this.tag)
            audioController.Victory();
        else
            audioController.Defeat();
    }

    public void UpdateStatDisplay()
    {
        if (!isAI)
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

    public void TakeDamage(int damage, bool ability, GameObject target)
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
            OnGetHit(target);
        }

        // Play Hurt Animation/Audio
        audioController.Hurt();

        // Apply OnHit Events
        OnDamaged(damageTaken, target);
        OnHealth(target);
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
        if (!dead)
        {
            Debug.Log(this.gameObject.name + " has Died!");
            dead = true;
            //GetComponent<Renderer>().material.color = Color.black;

            gameManager.PlayerDied(this.gameObject);

            audioController.Die();

            StopAllCoroutines();
            //GetComponent<Collider>().enabled = false;
        }
    }

    // *************************** Trigger Events ***************************
    public void OnHit(GameObject target)
    {
        // Don't hit same object multiple times in a single attack
        if (!hitList.Contains(target))
        {
            StartCoroutine(RecentlyHit(hitInterval, target));

            // Deal damage if it is hurtable
            if (target.GetComponent<Base_Stats>() != null)
            target.GetComponent<Base_Stats>().TakeDamage(attack, false, target);

            // Apply on hit effects
            if (onHitEffectsList.Count > 0)
            {
                foreach (DeliverySO deliveryMethod in onHitEffectsList)
                {
                    deliveryMethod.triggeringTarget = target;
                    deliveryMethod.ApplyDelivery(target);
                }
            }
        }
    }

    private void OnGetHit(GameObject target)
    {
        if (onGetHitEffectsList.Count > 0)
        {
            foreach (DeliverySO deliveryMethod in onHitEffectsList)
            {
                deliveryMethod.ApplyDelivery(target);
            }
        }
    }

    private void OnDamaged(int damage, GameObject target)
    {
        if (onDamagedList.Count > 0)
        {
            foreach (OnDamaged trigger in onDamagedList)
            {
                trigger.ability.deliveryMethod.triggeringTarget = target;
                trigger.CheckIfEnoughDamaged(this.gameObject, damage, maxHP);
            }
        }
    }

    private void OnHealth(GameObject target)
    {
        if (onHealthList.Count > 0)
        {
            foreach (OnHealth trigger in onHealthList)
            {
                trigger.ability.deliveryMethod.triggeringTarget = target;
                trigger.CheckIfTriggered(this.gameObject, currentHP, maxHP);
            }
        }
    }

    // *************************** Buffs/Debuffs/Card playing events ***************************
    public void ChangeMaxHealth(int healthChange)
    {
        ChangeMaxHealth(healthChange, false, false, null);
    }

    public void ChangeMaxHealth(int healthChange, bool multiplier)
    {
        ChangeMaxHealth(healthChange, false, multiplier, null);
    }

    public void ChangeMaxHealth(int healthChange, bool changeCurrentHealth, GameObject target)
    {
        ChangeMaxHealth(healthChange, changeCurrentHealth, false, target);
    }

    public void ChangeMaxHealth(int healthChange, bool multiplier, bool changeCurrentHealth, GameObject target)
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
                    TakeDamage(newHealth, true, target);
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
                    TakeDamage(Mathf.Abs(healthChange), true, target);
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

        if (armour < 0)
            armour = 0;
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

        if (attack < 2)
            attack = 2;
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

        if (speed < 0)
            speed = 0;
        SpawnHitText(Color.blue, speedChange);
    }

    public void CalcMoveSpeed()
    {
        // Put move speed function here!
        float moveSpeed = speed;
        stateController.SetAISpeed(moveSpeed);
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

        if (atkSpeed < 1)
            atkSpeed = 1;
    }

    public void ApplyCC(CC ccType, float duration)
    {
        ApplyCC(ccType, duration, 0);
    }

    public void ApplyCC (CC ccType, float duration, int value)
    {
        Debug.Log("Applying CC!");
        switch (ccType)
        {
            case CC.Stun:
                stunned = true;
                stunLength += duration;
                GetComponent<NavMeshAgent>().speed = 0f;
                break;
            case CC.Snare:
                snared = true;
                snareLength += duration;
                GetComponent<NavMeshAgent>().speed = 0f;
                break;
            case CC.Slow:
                ChangeSpeed(value);
                GetComponent<NavMeshAgent>().speed = speed;
                StartCoroutine(SlowTimer(duration, value));
                break;
            case CC.SlowAttack:
                ChangeAttackSpeed(value);
                StartCoroutine(SlowAttackTimer(duration, value));
                break;
            default:
                Debug.Log("Unknown CC type");
                break;
        }
    }

    private IEnumerator SlowTimer(float duration, int value)
    {
        yield return new WaitForSeconds(duration);
        // Unslow
        ChangeSpeed(value * -1);
        GetComponent<NavMeshAgent>().speed = speed;
    }

    private IEnumerator SlowAttackTimer(float duration, int value)
    {
        yield return new WaitForSeconds(duration);
        // Unslow
        ChangeAttackSpeed(value * -1);
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
