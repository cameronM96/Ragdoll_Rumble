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

        if (controller.chest != null)
        {
            Vector3 targetPos = controller.chaseTarget.position;
            targetPos.y = controller.chest.position.y;
            Debug.DrawRay(controller.chest.position, targetPos, Color.red);

            if ((controller.chest.position - targetPos).sqrMagnitude > controller.reach * controller.reach)
            {
                // Attack function
                controller.aiController.Attack();
                controller.Attack();
            }
        }
        else
        {
            Debug.DrawRay(controller.transform.position, (controller.transform.forward.normalized * controller.reach), Color.red);

            if ((controller.transform.position - controller.chaseTarget.position).sqrMagnitude > controller.reach * controller.reach)
            {
                // Attack function
                controller.aiController.Attack();
                controller.Attack();
            }
        }
    }
}
