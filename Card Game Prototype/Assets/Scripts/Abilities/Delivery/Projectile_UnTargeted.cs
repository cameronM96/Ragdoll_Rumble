using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_UnTargeted : Projectile
{
    public bool bounce = false;
    public bool includeTargetsInBounce = true;
    public int maxBounces = 0;
    public bool piercing = true;
    public bool includeTargetsInPierce = true;
    public int maxPierces = 0;

    protected int currentBounceNumb = 0;
    protected int currentPierceNumb = 0;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        if (bounce)
            GetComponent<Collider>().isTrigger = false;
        else
            GetComponent<Collider>().isTrigger = true;

        base.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        AdditionalUpdates();

        if (currentBounceNumb >= maxBounces && maxBounces != 0)
            Destroy(this.gameObject, deathTimer);

        if (currentPierceNumb >= maxPierces && maxPierces != 0)
            Destroy(this.gameObject, deathTimer);
    }

    protected override void AdditionalUpdates()
    {
        base.AdditionalUpdates();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if not piercing
        if (!piercing)
            Destroy(this.gameObject, deathTimer);
        
        AdditionalTriggerEvents(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        AdditionalTriggerEvents(collision.gameObject);
    }
}