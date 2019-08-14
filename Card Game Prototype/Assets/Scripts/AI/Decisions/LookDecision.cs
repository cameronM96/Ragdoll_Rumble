using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return Look(controller);
    }

    private bool Look (StateController controller)
    {
        // Find all enemies
        List<Transform> potentialTargets = new List<Transform>();
        for (int i = 0; i < controller.gameManager.numberOfTeams; i ++)
        {
            string teamTag = "Team " + (i + 1);
            if (teamTag != controller.tag)
            {
                GameObject[] teamMemebers = GameObject.FindGameObjectsWithTag(teamTag);
                foreach (GameObject memeber in teamMemebers)
                {
                    // Only add alive players
                    if (!memeber.GetComponent<Base_Stats>().dead)
                    {
                        StateController targetController = memeber.GetComponent<StateController>();
                        if (targetController?.chest != null)
                        {
                            potentialTargets.Add(targetController.chest);
                        }
                        else
                            potentialTargets.Add(memeber.transform);
                    }
                }
            }
        }

        // Find Closest Enemy
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = controller.transform.position;

        foreach(Transform target in potentialTargets)
        {
            Vector3 diff = target.position - currentPosition;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < closestDistance)
            {
                closestTarget = target;
                closestDistance = curDistance;
            }
        }

        // Tell controller that target has been found.
        if (closestTarget != null)
        {
            controller.chaseTarget = closestTarget;
            return true;
        }
        else
            return false;
    }
}
