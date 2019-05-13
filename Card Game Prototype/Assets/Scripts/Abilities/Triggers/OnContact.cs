using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContact : Triggers
{
    private void OnTriggerEnter(Collider other)
    {
        Initialise();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Initialise();
    }
}
