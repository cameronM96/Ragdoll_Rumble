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
    public Animator animController;

    public Transform spawnPoint;
    //public Transform chest;

    [HideInInspector] public AIController aiController;
    public float reach;
    public float reachOffset;
    public NavMeshAgent navMeshAgent;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed;
    public Rigidbody myRB;
    public Attack[] attackPoints;
    public float attackSpeedScalar = 30f;
    public float zeroAtkSpeed = 3.33f;
    public float moveSpeedScalar = 0.25f;

    private Vector3 returnPos;
    private bool aiActive = false;
    private float attackTimer;

    public float turnSpeed = 1f;
    public State[] allstates;

    private void Awake()
    {
        baseStates = GetComponent<Base_Stats>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        aiController = GetComponent<AIController>();
        //if (chest != null)
        //    returnPos = chest.position;
        myRB = GetComponent<Rigidbody>();
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
        if (animController != null)
            animController.SetBool("cardPhase", false);

        aiActive = true;
        baseStates.CalcMoveSpeed();
        SetupAI();
        if (currentState != baseAIState)
            TransitionToState(baseAIState);
    }

    private void InitialiseCardPhase()
    {
        aiActive = false;
        if (animController != null)
        {
            animController.SetBool("dead", false);
            animController.SetBool("reset", true);
            animController.SetBool("cardPhase", true);
        }

        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

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
        navMeshAgent.speed = speed * moveSpeedScalar;
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

        if (animController != null)
        {
            if (currentState == allstates[0])
            {
                // Idle
                if (animController != null)
                    animController.SetBool("moving", false);
            }
            else if (currentState == allstates[1])
            {
                if (animController != null)
                    animController.SetBool("moving", true);
            }
            else if (currentState == allstates[2])
            {
                if (animController != null)
                    animController.SetBool("moving", false);
            }
        }

        FaceTarget(chaseTarget.position);
    }

    private void LateUpdate()
    {
        if (animController != null)
            animController.SetBool("reset", false);
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed);
    }

    private void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            //if (chest != null)
            //    Gizmos.DrawWireSphere(this.chest.position, reach - reachOffset);
            //else
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
            if (animController != null)
                animController.SetBool("attacking", true);

            //Debug.Log("Attack!");
            Random.InitState(System.DateTime.Now.Millisecond);
            int index = Random.Range(0, attackPoints.Length);
            attackPoints[index].AttackTarget(chaseTarget);
            attackTimer = zeroAtkSpeed - (baseStates.atkSpeed / attackSpeedScalar);
            if (attackTimer < 0.1)
                attackTimer = 0.1f;
        }
    }

    public void EndAttack()
    {
        if (animController != null)
            animController.SetBool("attacking", false);
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

    public void RagDoll()
    {
        if (animController != null)
            animController.SetBool("ragDoll", true);
    }

    public void UnRagDoll()
    {
        if (animController != null)
            animController.SetBool("ragDoll", false);
    }

    public void Dead()
    {
        if (animController != null)
            animController.SetBool("dead", true);

        RagDoll();
    }

    public void Stunned()
    {
        if (animController != null)
        {
            animController.SetBool("stunned", true);
            animController.SetBool("applyStun", true);
        }
    }

    public void UnStun()
    {
        if (animController != null)
            animController.SetBool("stunned", false);
    }

    public void UnApplyStun()
    {
        if (animController != null)
            animController.SetBool("applyStun", false);
    }

    // Increase attack range
    public void ChangeReach(float newReach)
    {
        if (newReach - reachOffset > reach)
        {
            reach = newReach - reachOffset;
            navMeshAgent.stoppingDistance = reach;
        }
    }
}