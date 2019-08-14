﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIController))]
public class StateController : MonoBehaviour
{
    // Manages the AI state machine
    public State currentState;
    public State baseAIState;
    public State remainState;

    public Base_Stats baseStates;
    public GameManager gameManager;

    public Transform spawnPoint;
    public Transform chest;

    [HideInInspector] public AIController aiController;
    public float reach;
    public float reachOffset;
    public NavMeshAgent navMeshAgent;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed;
    public Transform[] ragDollTransforms;
    [HideInInspector] public Rigidbody[] rbs;
    private Vector3[] returnPoints;
    private Quaternion[] returnRots;
    public Attack[] attackPoints;
    public float attackSpeedScalar = 30f;
    public float zeroAtkSpeed = 3.33f;

    private Vector3 returnPos;
    private bool aiActive = false;
    private float attackTimer;

    private void Awake()
    {
        baseStates = GetComponent<Base_Stats>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        aiController = GetComponent<AIController>();
        if (chest != null)
            returnPos = chest.position;

        rbs = new Rigidbody[ragDollTransforms.Length];
        returnPoints = new Vector3[ragDollTransforms.Length];
        returnRots = new Quaternion[ragDollTransforms.Length];
        for (int i = 0; i < ragDollTransforms.Length; i++)
        {
            rbs[i] = ragDollTransforms[i]?.GetComponent<Rigidbody>();
            returnPoints[i] = ragDollTransforms[i].position;
            returnRots[i] = ragDollTransforms[i].rotation;
        }
    }

    private void OnEnable()
    {
        GameManager.EnterCombatPhase += InitialiseCombatPhase;
        GameManager.EnterCardPhase += InitialiseCardPhase;
    }

    private void OnDisable()
    {
        GameManager.EnterCombatPhase -= InitialiseCombatPhase;
        GameManager.EnterCardPhase -= InitialiseCardPhase;
    }

    private void InitialiseCombatPhase ()
    {
        foreach(Rigidbody rb in rbs) 
            rb.isKinematic = false;

        aiActive = true;
        baseStates.CalcMoveSpeed();
        SetupAI();
        if (currentState != baseAIState)
            TransitionToState(baseAIState);
    }

    private void InitialiseCardPhase()
    {
        aiActive = false;
        if (chest != null)
            TPose();
        else
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }

        if (navMeshAgent!= null)
            SetupAI();

        TransitionToState(baseAIState);
    }

    public void SetupAI ()
    {
        if (aiActive)
            navMeshAgent.enabled = true;
        else
            navMeshAgent.enabled = false;
    }

    public void SetAISpeed (float speed)
    {
        navMeshAgent.speed = speed;
    }

    private void Update()
    {
        if (!aiActive)
            return;

        if (currentState != null)
            currentState.UpdateState(this);

        if (attackTimer > zeroAtkSpeed - (baseStates.atkSpeed / attackSpeedScalar))
            attackTimer = zeroAtkSpeed - (baseStates.atkSpeed / attackSpeedScalar);

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            if (chest != null)
                Gizmos.DrawWireSphere(this.chest.position, reach - reachOffset);
            else
                Gizmos.DrawWireSphere(this.transform.position, reach);
        }
    }

    // Transition to 
    public void TransitionToState (State nextState)
    {
        if (nextState != remainState)
        {
            //Debug.Log(this.tag + " has transitioned to " + nextState.name);
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed (float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    public void Attack()
    {
        if (attackTimer <= 0)
        {
            Debug.Log("Attack!");
            Random.InitState(System.DateTime.Now.Millisecond);
            int index = Random.Range(0, attackPoints.Length);
            attackPoints[index].AttackTarget(chaseTarget);
            attackTimer = zeroAtkSpeed - (baseStates.atkSpeed / attackSpeedScalar);
            if (attackTimer < 0.1)
                attackTimer = 0.1f;
        }
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

    public bool OverRideState (State overrideState)
    {
        if (currentState.overRideAble && overrideState != null && overrideState != remainState)
        {
            Debug.Log(this.tag + " state was overriden by " + overrideState.name);
            currentState = overrideState;
            SetupAI();
            OnExitState();
            return true;
        }

        return false;
    }

    public void TPose()
    {
        Debug.Log("Executing T pose");
        foreach (Rigidbody rb in rbs)
            rb.isKinematic = true;

        //chest.position = returnPos;

        for (int i = 0; i < ragDollTransforms.Length; i++)
        {
            ragDollTransforms[i].position = returnPoints[i];
            ragDollTransforms[i].rotation = returnRots[i];
        }
    }

    // Increase attack range
    public void ChangeReach(float newReach)
    {
        if (newReach - reachOffset > reach)
        {
            reach = newReach - reachOffset;
            navMeshAgent.stoppingDistance = reach/4;
        }
    }
}
