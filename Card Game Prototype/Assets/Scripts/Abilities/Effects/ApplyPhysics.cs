using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class ApplyPhysics : MonoBehaviour
{
    public bool destroyOnImpact = false;
    public bool oncePerRound = true;
    public float cooldown;
    private float timer;
    private bool ready;
    private bool disabled;
    private bool triggered;
    GameObject target;

    // Physics
    public PhysicsDirection direction = PhysicsDirection.None;
    public float strength;
    public float explosionSize;
    public Vector3 customDirection;
    // Start is called before the first frame update
    public Effect[] effects;
    public AudioClip audioOnTrigger;
    private AudioSource aS;

    public GameObject triggerNotification;
    public Color on;
    public Color off;

    public bool animated;
    public Animator anim;
    public GameObject particleEffect;

    private void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameManager.EnterCardPhase += DisableObject;
        GameManager.EnterCombatPhase += ResetObject;
    }

    private void OnDisable()
    {
        GameManager.EnterCardPhase -= DisableObject;
        GameManager.EnterCombatPhase -= ResetObject;
    }

    private void Update()
    {
        if(!ready && !disabled)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                ready = true;
                if (triggerNotification != null)
                {
                    triggerNotification.GetComponent<MeshRenderer>().material.color = on;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (triggered && !disabled)
        {
            ready = false;
            timer = cooldown;
            if (animated)
                anim.SetBool("active", true);
            ApplyEffect(target);
            triggered = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ready && !disabled)
        {
            if (other.transform.root.tag == "Team 1" || other.transform.root.tag == "Team 2")
            {
                triggered = true;
                target = other.transform.root.gameObject;
            }
        }
    }

    public void ResetObject()
    {
        disabled = false;
        ready = true;
        timer = 0;
        if (triggerNotification != null)
        {
            triggerNotification.GetComponent<MeshRenderer>().material.color = on;
        }
    }

    public void DisableObject()
    {
        disabled = true;
    }

    public void EndAnimation()
    {
        anim.SetBool("active", false);
    }

    // Needs to be called in fixed update
    protected void ApplyEffect(GameObject target)
    {
        Debug.Log("LETS DO THIS!");

        Rigidbody rb;
        Transform center = null;
        //center = target?.GetComponent<StateController>()?.chest;

        //if (center == null)
        //{
            rb = target.GetComponent<Rigidbody>();
            center = target.transform;
        //}
        //else
        //    rb = target?.GetComponent<StateController>()?.chest?.GetComponent<Rigidbody>();


        if (rb == null)
        {
            Debug.LogError("Failed to find Rigidbody");
            return;
        }

        Vector3 forceDirection = Vector3.zero;

        switch (direction)
        {
            case PhysicsDirection.None:
                break;
            case PhysicsDirection.Away:
                forceDirection = transform.position - center.position;
                break;
            case PhysicsDirection.Towards:
                forceDirection = center.position - transform.position;
                break;
            case PhysicsDirection.Explosion:
                rb.AddExplosionForce(strength, transform.position, explosionSize);
                break;
            case PhysicsDirection.Custom:
                forceDirection = customDirection;
                break;
            default:
                break;
        }
        forceDirection.Normalize();
        //Debug.Log("Applying force to " + target);
        rb.AddForce(forceDirection * strength, ForceMode.Impulse);

        aS.clip = audioOnTrigger;
        aS.Play();
        if (particleEffect != null)
        {
            GameObject particle = Instantiate(particleEffect);
            particle.transform.position = transform.position;
            particle.transform.localScale *= 2;
            particle.GetComponent<ParticleSystem>().Play();
            Destroy(particle, 3);
        }

        foreach (Effect effect in effects)
            effect.TriggerEffect(target);

        if (triggerNotification != null)
        {
            triggerNotification.GetComponent<MeshRenderer>().material.color = off;
        }

        if (destroyOnImpact)
            Destroy(this.gameObject);

        if (oncePerRound)
            DisableObject();
    }
}
