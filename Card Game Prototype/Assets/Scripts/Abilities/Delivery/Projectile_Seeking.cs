using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Seeking : Projectile
{
    public bool curve = false;
    public float maxSteerForce = 1;

    public bool uniqueTargetsOnly = true;
    public bool alternateTeams = false;

    public enum TargetSelection { Closest, Farthest, Random };
    public TargetSelection targetSelectionMethod = TargetSelection.Closest;
    public bool getClosestTarget = true;
    
    private List<GameObject> targets = new List<GameObject>();
    [SerializeField] private GameObject currenttarget;
    private Vector3 acceleration;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        curving = curve;

        FindTargets();
        //currenttarget = GetClosestTarget();

        // If not enough targets to bounce to or each target can only be hit once and there aren't enough.
        if (uniqueTargetsOnly && (targets.Count < maxTargetsHit || maxTargetsHit == 0))
            maxTargetsHit = targets.Count;

        base.Initialize();
    }

    private void Update()
    {
        AdditionalUpdates();

        if (constSpeed && curving)
        {
            direction = acceleration;
        }
        else if (constSpeed && !curve)
        {
            direction = Vector3.Normalize(currenttarget.transform.position - transform.position) * speed;
        }

        acceleration *= 0;
    }

    protected override void AdditionalUpdates()
    {
        base.AdditionalUpdates();

        if (currenttarget == null && !uniqueTargetsOnly)
        {
            FindTargets();
            GetNewTarget();
        }

        if (currenttarget != null)
            Seek();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        AdditionalTriggerEvents(other.gameObject);
        //Debug.Log("Trigger Activated!");
        // if hit object is a player
        if ((other.gameObject.tag == "Team 1" || other.gameObject.tag == "Team 2"))
        {
            if (multiTargets)
            {
                if (other.gameObject == currenttarget)
                {
                    if (uniqueTargetsOnly)
                        targets.Remove(currenttarget);

                    GetNewTarget();
                }
            }
        }
    }

    void Seek ()
    {
        // Get Desired Vector
        Vector3 desired = currenttarget.transform.position - transform.position;
        desired.Normalize();
        desired *= speed;

        // Get Steer Vector
        Vector3 steer = desired - rb.velocity;
        steer = Vector3.ClampMagnitude(steer, maxSteerForce);

        ApplyForce(steer);
    }

    void ApplyForce(Vector3 force)
    {
        acceleration += force;
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

    void GetNewTarget()
    {
        //Debug.Log("Finding new Target!");
        List<GameObject> tempList = targets;
        tempList.Remove(currenttarget);

        //if (tempList.Count == 0)
        //{
        //    //Debug.Log("No Targets to check");
        //    FindTargets();
        //    GetNewTarget(++attempts);
        //    return;
        //}

        // Find the new target
        GameObject newTarget = null;
        float closestDistance;

        switch (targetSelectionMethod)
        {
            case TargetSelection.Closest:
                closestDistance = Mathf.Infinity;

                // Find Closest Target
                foreach (GameObject target in targets)
                {
                    if ((transform.position - target.transform.position).sqrMagnitude <
                        (closestDistance * closestDistance))
                    {
                        // alternate between each team.
                        if (alternateTeams && currenttarget != null)
                        {
                            if (currenttarget.tag != target.tag)
                            {
                                // New Closest Target found!
                                newTarget = target;
                                closestDistance =
                                    Mathf.Abs((transform.position - target.transform.position).magnitude);
                            }
                        }
                        else
                        {
                            // New Closest Target found!
                            newTarget = target;
                            closestDistance =
                                Mathf.Abs((transform.position - target.transform.position).magnitude);
                        }
                    }
                }

                break;
            case TargetSelection.Farthest:
                closestDistance = 0;

                // Find farthest target
                foreach (GameObject target in targets)
                {
                    if ((transform.position - target.transform.position).sqrMagnitude >
                        (closestDistance * closestDistance))
                    {
                        // alternate between each team.
                        if (alternateTeams && currenttarget != null)
                        {
                            if (currenttarget.tag != target.tag)
                            {
                                // New Closest Target found!
                                newTarget = target;
                                closestDistance =
                                    Mathf.Abs((transform.position - target.transform.position).magnitude);
                            }
                        }
                        else
                        {
                            // New Closest Target found!
                            newTarget = target;
                            closestDistance =
                                Mathf.Abs((transform.position - target.transform.position).magnitude);
                        }
                    }
                }

                break;
            case TargetSelection.Random:
                // Find a random Target
                List<GameObject> randomTempList = new List<GameObject>();
                foreach (GameObject target in tempList)
                {
                    randomTempList.Add(target);
                }

                GameObject potentialTarget = null;

                while (newTarget == null && randomTempList.Count != 0)
                {
                    potentialTarget = randomTempList[Random.Range(0, randomTempList.Count - 1)];
                    if (alternateTeams && currenttarget != null)
                    {
                        // alternate between each team.
                        if (currenttarget.tag != potentialTarget.tag)
                        {
                            newTarget = potentialTarget;
                        }
                        else
                        {
                            // current target was same team, try again.
                            randomTempList.Remove(potentialTarget);
                        }
                    }
                    else
                    {
                        // No current target so use anytarget.
                        newTarget = potentialTarget;
                    }
                }

                break;
            default:
                Debug.Log("Invalid Target Selection Method!");
                break;
        }

        //if (newTarget == null && attempts < 3)
        //{
        //    FindTargets();
        //    GetNewTarget(++attempts);
        //}

        currenttarget = newTarget;
    }
}