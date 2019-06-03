﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearOnTargets : Delivery
{
    public enum DesiredTarget { Self, TriggeringTarget, MultiTarget};
    public DesiredTarget desiredTargets = DesiredTarget.Self;


    public Effect[] effects;

    private List<GameObject> targets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        switch (desiredTargets)
        {
            case DesiredTarget.Self:
                if (owner != null)
                {
                    foreach (Effect effect in effects)
                        effect.TriggerEffect(owner);
                }
                break;
            case DesiredTarget.TriggeringTarget:
                if (TriggeringTarget != null)
                {
                    foreach (Effect effect in effects)
                        effect.TriggerEffect(TriggeringTarget);
                }
                break;
            case DesiredTarget.MultiTarget:
                FindTargets();
                foreach (GameObject target in targets)
                {
                    foreach (Effect effect in effects)
                        effect.TriggerEffect(target);
                }
                break;
            default:
                Debug.Log("Unknown 'Desired Target'!");
                break;
        }

        base.Initialize();

        Destroy(this.gameObject);
    }

    void FindTargets()
    {
        // Find Targets
        GameObject[] findingTargets;
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
                    Debug.Log(findingTargets.Length);
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
