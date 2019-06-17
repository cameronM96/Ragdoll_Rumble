using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/InRange")]
public class InRangeDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return InRange(controller);
    }

    private bool InRange (StateController controller)
    {
        // Check if target is within attack range.
        if ((controller.chaseTarget.position - controller.transform.position).sqrMagnitude <
            controller.baseStates.attackRange * controller.baseStates.attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
