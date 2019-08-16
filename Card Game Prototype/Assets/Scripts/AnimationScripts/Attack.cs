using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Rigidbody rb;
    public float acceptanceDistance;
    public Transform target;
    public Transform holsterTarget;
    public float holsterForceScalar = 40f;
    public float attackForceScalar = 100f;
    public float postAttackDelay = 1f;

    public bool attacking = false;
    private bool holstering = true;
    private float timer = 0f;

    public AIController audioController;
    public Collider myCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider>();
        myCollider.isTrigger = true;
        myCollider.enabled = false;
    }

    public void AttackTarget(Transform newTarget)
    {
        StopAllCoroutines();
        myCollider.enabled = true;
        target = newTarget;
        attacking = true;
        audioController.Attack();
        StartCoroutine(ColliderDisable());
    }

    IEnumerator ColliderDisable()
    {
        yield return new WaitForSeconds(0.5f);
        myCollider.enabled = false;
    }

    private void Update()
    {
        if (!holstering)
        {
            timer += Time.deltaTime;
            if (timer >= postAttackDelay)
                holstering = true;
        }
    }

    private void FixedUpdate()
    {
        if (attacking && target == null)
        {
            Debug.LogError("No Target!");
            attacking = false;
        }
        else if (attacking && target != null)
        {
            //float distance = (transform.position - target.position).sqrMagnitude;
            //if (distance < acceptanceDistance * acceptanceDistance)
            //{
            //    // Attack completed
            //    attacking = false;
            //    return;
            //}

            //Vector3 direction = target.position - transform.position;
            //direction.Normalize();
            //direction *= attackForceScalar;
            //rb.AddForce(direction,ForceMode.Impulse);
            attacking = false;
            holstering = false;
            timer = 0;
        }
        //else if (holstering)
        //{
        //    Vector3 direction = holsterTarget.position - transform.position;
        //    direction.Normalize();
        //    direction *= holsterForceScalar;
        //    rb.AddForce(direction);
        //}
    }
}
