using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWeaponScript : MonoBehaviour
{
    public GameObject owner;
    public Base_Stats ownerStats;
    public Collider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        owner = this.transform.root.gameObject;
        //myTeam = owner.tag;
        ownerStats = owner.GetComponent<Base_Stats>();

        if (ownerStats == null)
            Debug.Log("No Owner! WHO AM I?! Root: " + owner);

        myCollider = GetComponent<Collider>();
        myCollider.isTrigger = true;
        myCollider.enabled = false;
        StartCoroutine(colliderDisable());
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Whacked " + other.transform.root.gameObject.name + " !");
        if (other.transform.root.gameObject != owner)
        {
            ownerStats.OnHit(other.transform.root.gameObject);
            myCollider.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Whacked " + collision.transform.root.gameObject.name + " !");
        if (collision.transform.root.gameObject != owner)
            ownerStats.OnHit(collision.transform.root.gameObject);
    }
    IEnumerator colliderDisable()
    {
        yield return new WaitForSeconds(0.5f);
        myCollider.enabled = false;
    }
}
