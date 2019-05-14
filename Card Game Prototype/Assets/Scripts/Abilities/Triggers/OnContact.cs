using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContact : Triggers
{
    private void OnTriggerEnter(Collider other)
    {
        ApplyEffect(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ApplyEffect(collision.gameObject);
    }
}
