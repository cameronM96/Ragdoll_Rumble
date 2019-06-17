using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Attack")]
public class AttackAct : Activity
{
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        // Attack if in attack range
        RaycastHit hit;

        Debug.DrawRay(controller.transform.position, (controller.transform.forward.normalized * controller.baseStates.attackRange), Color.red);

        if (Physics.SphereCast (controller.transform.position, controller.baseStates.attackRange/2,controller.transform.forward,out hit,controller.baseStates.attackRange) 
            && hit.collider.CompareTag(controller.chaseTarget.tag))
        {
            if (controller.CheckIfCountDownElapsed (controller.baseStates.atkSpeed))
            {
                // Attack function
                controller.aiController.Attack();
            }
        }
    }
}
