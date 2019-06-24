using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAttackScript : MonoBehaviour
{
    public GameObject owner;
    public Base_Stats ownerStats;
    //public string myTeam;
    public float speed = 1;

    public GameManager gameManager;
    private Transform rotPoint;
    private bool inCombat = false;
    // Update is called once per frame
    private void Start()
    {
        //owner = this.transform.root.gameObject;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        owner = this.transform.root.gameObject;
        //myTeam = owner.tag;
        ownerStats = owner.GetComponent<Base_Stats>();
        rotPoint = this.transform;

        if (ownerStats == null)
            Debug.Log("No Owner! WHO AM I?!");
    }

    private void OnEnable()
    {
        GameManager.EnterCardPhase += InitialiseCardPhase;
        GameManager.EnterCombatPhase += InitialiseCombatPhase;
    }

    private void OnDisable()
    {
        GameManager.EnterCardPhase -= InitialiseCardPhase;
        GameManager.EnterCombatPhase -= InitialiseCombatPhase;
    }

    public void InitialiseCardPhase()
    {
        inCombat = false;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void InitialiseCombatPhase()
    {
        speed = ownerStats.atkSpeed;
        inCombat = true;
    }

    void Update()
    {
        if (inCombat)
            rotPoint.Rotate(0, 1 * speed, 0);
    }
}
