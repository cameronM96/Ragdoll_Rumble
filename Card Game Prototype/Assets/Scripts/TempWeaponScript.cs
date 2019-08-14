using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWeaponScript : MonoBehaviour
{
    public GameObject owner;
    public Base_Stats ownerStats;

    // Start is called before the first frame update
    void Start()
    {
        owner = this.transform.root.gameObject;
        //myTeam = owner.tag;
        ownerStats = owner.GetComponent<Base_Stats>();

        if (ownerStats == null)
            Debug.Log("No Owner! WHO AM I?! Root: " + owner);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Whacked " + other.transform.root.gameObject.name + " !");
        if (other.transform.root.gameObject != owner)
            ownerStats.OnHit(other.transform.root.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Whacked " + collision.transform.root.gameObject.name + " !");
        if (collision.transform.root.gameObject != owner)
            ownerStats.OnHit(collision.transform.root.gameObject);
    }
}
