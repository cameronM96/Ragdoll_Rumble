using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTesting : MonoBehaviour
{
    public GameObject owner;
    public Base_Stats ownerStats;
    public string myTeam;
    public float speed = 1;

    private Transform rotPoint;
    // Update is called once per frame
    private void Start()
    {
        //owner = this.transform.root.gameObject;
        owner = this.transform.parent.parent.gameObject;
        myTeam = owner.tag;
        ownerStats = owner.GetComponent<Base_Stats>();
        rotPoint = this.transform.parent;

        if (ownerStats == null)
            Debug.Log("No Owner! WHO AM I?!");
    }

    void Update()
    {
        rotPoint.Rotate(0, 1 * speed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Weapons Triggered!");
        ownerStats.OnHit(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Weapons Triggered!");
        ownerStats.OnHit(collision.gameObject);
    }
}
