using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Delivery/Environmental")]
public class EnvironmentalSO : DeliverySO
{
    public GameObject templateObj;
    public int numberOfProjectiles = 10;
    public bool destroyWhenTriggered = false;
    public float radiusOffset = 2f;
    public float verticalOffset = 2f;

    private List<GameObject> targets = new List<GameObject>();

    public override void ApplyDelivery(GameObject target)
    {
        base.ApplyDelivery(target);
        FindTargets();

        // Find Center
        Vector3 center = Vector3.zero;
        foreach(GameObject unit in targets)
        {
            center += unit.transform.position;
        }

        center /= targets.Count;

        float radius = -radiusOffset;
        float distance = 0f;
        foreach(GameObject unit in targets)
        {
            if ((center - unit.transform.position).sqrMagnitude > distance * distance)
                distance = Vector3.Distance(center,unit.transform.position);
        }

        radius += distance;

        Debug.Log("Environmental Radius: " + radius);

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject newObject = Instantiate(templateObj);
            Vector3 newPos = Random.insideUnitSphere * radius;
            newPos.y = verticalOffset;
            newObject.transform.position = newPos;
            Random.InitState(System.DateTime.Now.Millisecond);
            newObject.transform.Rotate(0, Random.Range(-180, 180), 0);
            newObject.GetComponent<ApplyPhysics>().effects = ability.effects;
        }
    }

    public override void Initialise(GameObject targetObject)
    {
        base.Initialise(targetObject);
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
