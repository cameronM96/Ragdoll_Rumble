﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Chase")]
public class ChaseAct : Activity
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase (StateController controller)
    {
        if (controller.navMeshAgent.enabled)
        {
            controller.navMeshAgent.destination = controller.chaseTarget.position;
            controller.navMeshAgent.isStopped = false;
        }
    }
}
