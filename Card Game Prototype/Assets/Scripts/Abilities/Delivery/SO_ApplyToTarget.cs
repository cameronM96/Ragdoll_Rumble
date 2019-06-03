using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Delivery/ApplyToTarget")]
public class SO_ApplyToTarget : DeliverySO
{
    public DesiredTarget desiredTargets = DesiredTarget.Self;
    public int maxTargets = 0;

    private List<GameObject> targets = new List<GameObject>();

    public override void ApplyDelivery()
    {
        base.ApplyDelivery();

        switch (desiredTargets)
        {
            case DesiredTarget.Self:
                if (owner != null)
                {
                    foreach (Effect effect in ability.effects)
                        effect.TriggerEffect(owner);
                }
                break;
            case DesiredTarget.TriggeringTarget:
                if (triggeringTarget != null)
                {
                    foreach (Effect effect in ability.effects)
                        effect.TriggerEffect(triggeringTarget);
                }
                break;
            case DesiredTarget.MultiTarget:
                FindTargets();
                int currentTargetCount = 0;
                foreach (GameObject target in targets)
                {
                    if (currentTargetCount <= 0)
                    {
                        foreach (Effect effect in ability.effects)
                            effect.TriggerEffect(target);

                        if (maxTargets != 0)
                            ++currentTargetCount;
                    }
                }
                break;
            default:
                Debug.Log("Unknown 'Desired Target'!");
                break;
        }
    }

    void FindTargets()
    {
        // Find Targets
        GameObject[] findingTargets;
        targets = new List<GameObject>();
        switch (viableTargetID)
        {
            case TargetID.All:
                findingTargets = GameObject.FindGameObjectsWithTag("Team 1");
                if (findingTargets == null)
                {
                    Debug.Log("No Targets Found!");
                    break;
                }
                foreach (GameObject newTarget in findingTargets)
                    targets.Add(newTarget);

                findingTargets = GameObject.FindGameObjectsWithTag("Team 2");
                if (findingTargets == null)
                {
                    Debug.Log("No Targets Found!");
                    break;
                }
                foreach (GameObject newTarget in findingTargets)
                    targets.Add(newTarget);
                break;

            case TargetID.Enemies:
                if (friendlyTag == "Team 1")
                {
                    findingTargets = GameObject.FindGameObjectsWithTag("Team 2");
                    //Debug.Log(findingTargets.Length);
                    if (findingTargets == null)
                    {
                        Debug.Log("No Targets Found!");
                        break;
                    }
                    foreach (GameObject newTarget in findingTargets)
                        targets.Add(newTarget);
                }
                else
                {
                    findingTargets = GameObject.FindGameObjectsWithTag("Team 1");
                    if (findingTargets == null)
                    {
                        Debug.Log("No Targets Found!");
                        break;
                    }
                    foreach (GameObject newTarget in findingTargets)
                        targets.Add(newTarget);
                }
                break;

            case TargetID.Allies:
                if (friendlyTag != "Team 1")
                {
                    findingTargets = GameObject.FindGameObjectsWithTag("Team 2");
                    if (findingTargets == null)
                    {
                        Debug.Log("No Targets Found!");
                        break;
                    }
                    foreach (GameObject newTarget in findingTargets)
                        targets.Add(newTarget);
                }
                else
                {
                    findingTargets = GameObject.FindGameObjectsWithTag("Team 1");
                    if (findingTargets == null)
                    {
                        Debug.Log("No Targets Found!");
                        break;
                    }
                    foreach (GameObject newTarget in findingTargets)
                        targets.Add(newTarget);
                }
                break;

            default:
                break;
        }
    }
}
