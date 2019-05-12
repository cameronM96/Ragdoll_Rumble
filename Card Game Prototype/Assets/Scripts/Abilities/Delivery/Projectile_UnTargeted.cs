using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_UnTargeted : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        AdditionalUpdates();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        AdditionalTriggerEvents(other);
    }

    protected override void AdditionalUpdates()
    {
        base.AdditionalUpdates();
    }
}