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

    [HideInInspector] public AIController aiController;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed; 

    private bool aiActive;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        baseStates = GetComponent<Base_Stats>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        aiController = GetComponent<AIController>();
    }

    public void SetupAI ()
    {
        if (aiActive)
            navMeshAgent.enabled = true;
        else
            navMeshAgent.enabled = false;
    }

    private void Update()
    {
        if (!aiActive)
            return;

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
            currentState = overrideState;
            OnExitState();
            return true;
        }

        return false;
    }
}
