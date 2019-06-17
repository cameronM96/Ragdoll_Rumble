using System.Collections;
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

    [HideInInspector] public AIController aiController;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] Rigidbody rb;

    private bool aiActive = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        baseStates = GetComponent<Base_Stats>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        aiController = GetComponent<AIController>();
        rb = GetComponent<Rigidbody>();
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
        SetupAI();
        rb.isKinematic = true;
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
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
    }

    private void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(this.transform.position, 5f);
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
}
