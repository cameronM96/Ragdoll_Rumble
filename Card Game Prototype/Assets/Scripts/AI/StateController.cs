using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public State currentState;
    public State remainState;

    public Base_Stats baseStates;
    public GameManager gameManager;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed; 

    private bool aiActive;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        baseStates = GetComponent<Base_Stats>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
}
