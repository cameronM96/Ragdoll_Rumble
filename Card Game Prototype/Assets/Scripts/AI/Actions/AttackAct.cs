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
            Debug.DrawRay(controller.chest.position, (controller.chest.forward.normalized * controller.reach), Color.red);

            if (Physics.SphereCast(controller.chest.position, controller.reach, controller.chest.forward, out hit, controller.reach))
            {
                // Attack function
                controller.aiController.Attack();
                controller.Attack();
            }
        }
        else
        {
            Debug.DrawRay(controller.transform.position, (controller.transform.forward.normalized * controller.reach), Color.red);

            if (Physics.SphereCast(controller.transform.position, controller.reach, controller.transform.forward, out hit, controller.reach)
                && hit.collider.CompareTag(controller.chaseTarget.tag))
            {
                // Attack function
                controller.aiController.Attack();
                controller.Attack();
            }
        }
    }
}
