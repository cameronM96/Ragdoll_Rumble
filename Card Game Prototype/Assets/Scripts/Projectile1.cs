using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile1 : MonoBehaviour
{
    public bool seeking = false;
    public bool curve = false;
    public float maxSteerForce = 1;
    public enum TargetID {All, Enemies, Allies};
    public TargetID viableTargetID = TargetID.All;

    public bool constSpeed = true;
    public bool gravity = false;
    public bool bounce = false;
    public bool multiTargets = false;
    public int MaxTargets = 0;
    public bool piercing = true;
    public bool hitTargetOnly = false;
    public bool uniqueTargetsOnly = true;
    public bool alternateTeams = false;

    public Vector3 direction;
    public float speed;
    public float lifeSpan = 5;

    public string friendlyTag = "Team 1";

    private Rigidbody rb;
    private bool forceapplied;
    private int targetContactCount = 0;
    private List<GameObject> targets = new List<GameObject>();
    [SerializeField] private GameObject currenttarget;
    private Vector3 acceleration;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        forceapplied = false;
        rb.useGravity = gravity;

        FindTargets();
        //currenttarget = GetClosestTarget();

        // If not enough targets to bounce to or each target can only be hit once and there aren't enough.
        if ((uniqueTargetsOnly && targets.Count < MaxTargets) || targets.Count < 2)
            MaxTargets = targets.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (currenttarget == null && !uniqueTargetsOnly)
        {
            FindTargets();
            GetClosestTarget();
        }

        if(seeking && currenttarget != null)
            Seek();

        if (lifeSpan != 0)
        {
            timer += Time.deltaTime;
            if (timer >= lifeSpan)
                Destroy(this.gameObject);
        }

        if (targetContactCount >= MaxTargets && MaxTargets != 0)
            Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        if (constSpeed && curve)
        {
            rb.AddForce(acceleration, ForceMode.Acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        }
        else if (constSpeed && !curve)
        {
            rb.velocity = Vector3.Normalize(currenttarget.transform.position - transform.position) * speed;
        }
        else if (!constSpeed && !forceapplied)
        {
            // Apply force once
            rb.AddForce(direction * speed, ForceMode.VelocityChange);
            forceapplied = true;
        }

        acceleration *= 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Activated!");
        // if hit object is a player
        if ((other.gameObject.tag == "Team 1" || other.gameObject.tag == "Team 2"))
        {
            if (multiTargets)
            {
                if (other.gameObject == currenttarget)
                {
                    Debug.Log("Hit Current Object!");
                    // Apply Effect here
                    other.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    TestEffect();

                    if (MaxTargets != 0)
                        targetContactCount++;

                    if (uniqueTargetsOnly)
                        targets.Remove(currenttarget);

                    GetClosestTarget();
                }
                else if (!hitTargetOnly)
                {
                    Debug.Log("Hit other Object!");
                    // Apply Effect here
                    if (!uniqueTargetsOnly)
                        other.gameObject.GetComponent<Renderer>().material.color = Color.grey;

                    if (MaxTargets != 0)
                        targetContactCount++;

                    TestEffect();
                }
            }
            else
            {
                //Apply Effect here
                other.gameObject.GetComponent<Renderer>().material.color = Color.red;
                TestEffect();
                Destroy(this.gameObject);
            }
        }
        else
        {
            Debug.Log("Unknown Object!");
            // if not piercing
            if (!piercing)
                Destroy(this.gameObject);
            
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

    void TestEffect ()
    {
        // Create Simple Cube
        Debug.Log("Creating Cube!");
        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newObject.GetComponent<Collider>().enabled = false;
        newObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        newObject.transform.position = this.transform.position;
        newObject.AddComponent<Rigidbody>();
        Destroy(newObject, 3);
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

    void GetClosestTarget()
    {
        List<GameObject> tempList = targets;
        tempList.Remove(currenttarget);

        if (tempList.Count == 0)
        {
            Debug.Log("No Targets to check");
            FindTargets();
            GetClosestTarget();
            return;
        }

        // Find the closest target
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject target in targets)
        {
            if ((transform.position - target.transform.position).sqrMagnitude < 
                (closestDistance * closestDistance))
            {
                if (alternateTeams && currenttarget != null)
                {
                    if (currenttarget.tag != target.tag)
                    {
                        // New Closest Target found!
                        closestTarget = target;
                        closestDistance = 
                            Mathf.Abs((transform.position - target.transform.position).magnitude);
                    }
                }
                else
                {
                    // New Closest Target found!
                    closestTarget = target;
                    closestDistance = 
                        Mathf.Abs((transform.position - target.transform.position).magnitude);
                }
            }
        }

        if (closestTarget == null)
        {
            FindTargets();
            GetClosestTarget();
        }

        currenttarget = closestTarget;
    }
}
