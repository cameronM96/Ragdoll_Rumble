using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class ApplyPhysics : MonoBehaviour
{
    public bool destroyOnImpact = false;
    public float cooldown;
    private float timer;
    private bool ready;
    private bool disabled;
    private bool triggered;
    GameObject target;

    // Physics
    public bool applyPhysics;
    public PhysicsDirection direction = PhysicsDirection.None;
    public float strength;
    public Vector3 customDirection;
    // Start is called before the first frame update
    public Effect effect;

    private void OnEnable()
    {
        GameManager.EnterCombatPhase += ResetObject;
    }

    private void OnDisable()
    {
        GameManager.EnterCombatPhase -= ResetObject;
    }

    private void Update()
    {
        if(!ready)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                ready = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (triggered)
        {
            ready = false;
            timer = cooldown;
            ApplyEffect(target);
            triggered = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ready)
        {
            triggered = true;
            target = other.transform.root.gameObject;
        }
    }

    public void ResetObject()
    {
        ready = true;
        timer = 0;
    }

    // Needs to be called in fixed update
    protected void ApplyEffect(GameObject target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb == null)
            return;

        Vector3 forceDirection = Vector3.zero;

        switch (direction)
        {
            case PhysicsDirection.None:
                break;
            case PhysicsDirection.Away:
                forceDirection =
                break;
            case PhysicsDirection.Towards:
                break;
            case PhysicsDirection.Custom:
                break;
            default:
                break;
        }

        effect.TriggerEffect(target);
    }
}
