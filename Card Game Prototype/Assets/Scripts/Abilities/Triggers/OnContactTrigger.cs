using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContactTrigger : MonoBehaviour
{
    [HideInInspector] public OnContact targetTrigger;

    private void OnTriggerEnter(Collider other)
    {
        targetTrigger.ApplyEffect(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        targetTrigger.ApplyEffect(collision.gameObject);
    }
}
