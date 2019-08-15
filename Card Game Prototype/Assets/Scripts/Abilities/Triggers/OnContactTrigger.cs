using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContactTrigger : MonoBehaviour
{
    [HideInInspector] public OnContact targetTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Team 1" || other.transform.root.tag == "Team 2")
            targetTrigger.ApplyEffect(other.gameObject);
    }
}
