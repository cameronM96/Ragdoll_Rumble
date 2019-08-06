using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public GameObject myPlayer;
    public GameObject templateHUD;
    [HideInInspector] public GameObject gameHUD;
    public List<Transform> targets;

    public Vector3 combatOffset = new Vector3(0,20,-60);
    public Vector3 cardOffset = new Vector3(20, 5, 0); // x = forward, y = up, z = right
    public float smoothTime = 0.5f;

    public float minZoom = 10f;
    public float maxZoom = 40f;
    [SerializeField] private float zoomLimiter = 50f;

    private Vector3 velocity;
    [HideInInspector] public Camera cam;

    private bool combatPhase = false;
    private bool manualControl = false;
    public float manualControlDelay = 5f;
    private float manualControlTimer;
    private Vector3 lastMousePosition;
    public float mouseSensitivity = 1f;
    public float manualControlMinDist = 2f;

    public float rotationSpeed = 1f;

    public bool transitioning;
    public float cardTransitionLength = 4f;
    public float combatTransitionLength = 3f;

    public GameManager gameManager;
    public bool draggingSomething = false;

    private void OnEnable()
    {
        GameManager.EnterCombatPhase += InitialiseCombatPhase;
        GameManager.EnterCardPhase += InitialiseCardPhase;
        GameManager.PlayerHasFallen += GetCameraTargets;
    }

    private void OnDisable()
    {
        GameManager.EnterCombatPhase -= InitialiseCombatPhase;
        GameManager.EnterCardPhase -= InitialiseCardPhase;
        GameManager.PlayerHasFallen -= GetCameraTargets;
    }

    public void InitialiseCombatPhase()
    {
        Debug.Log("In Combat Phase");
        combatPhase = true;
        GetCameraTargets();

        // Begin Camera Transition
        transitioning = true;
        StartCoroutine(TransitionSequence(combatTransitionLength));
    }

    public void InitialiseCardPhase()
    {
        Debug.Log("In Card Phase");
        combatPhase = false;
        manualControl = false;
        targets.Clear();
        targets.Add(myPlayer.transform);

        // Begin Camera Transition
        transitioning = true;
        StartCoroutine(TransitionSequence(cardTransitionLength));
    }

    IEnumerator TransitionSequence(float waitTimer)
    {
        // Disable Manual Camera Control until time elapse
        //Debug.Log("Begin Transition");
        yield return new WaitForSeconds(waitTimer);
        transitioning = false;
        //Debug.Log("End Transition");
    }

    private void Start()
    {
        // Initialise
        cam = GetComponent<Camera>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        myPlayer = transform.parent.gameObject;
        transform.SetParent(null);

        gameHUD = Instantiate(templateHUD);
        gameHUD.GetComponent<UIManager>().myCameraController = this;

        myPlayer.GetComponent<Base_Stats>().statDisplay = gameHUD.transform.GetChild(0).GetComponentInChildren<Display_Base_Stats>();
    }

    private void LateUpdate()
    {
        if (!IsMouseOverUI() && !draggingSomething && !transitioning)
        {
            // Key Board Controls
            //if (Input.GetMouseButtonDown(0))
            //    lastMousePosition = Input.mousePosition;

            //if (Input.GetMouseButton(0))
            //{
            //    Vector3 delta = Input.mousePosition - lastMousePosition;

            //    if (manualControl || delta.magnitude > manualControlMinDist)
            //        ManualCameraMove(delta);
            //}

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
                ManualCameraZoom(Input.GetAxis("Mouse ScrollWheel"));
        }

        if (manualControl)
        {
            // Set back to auto camera if camera has not been moved for a while
            manualControlTimer -= Time.deltaTime;
            if (manualControlTimer <= 0)
            {
                manualControlTimer = 0;
                manualControl = false;
            }
        }
        else
        {
            // Auto Camera
            if (targets.Count == 0)
                return;

            Move(combatPhase);
            Zoom();

            if (combatPhase)
            {
                // In Combat Phase
                // Rotate camera
                transform.rotation = Quaternion.RotateTowards(transform.rotation, gameManager.gameObject.transform.rotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // In Card Phase
                // Rotate camera
                Vector3 targetPosition = myPlayer.transform.position;
                targetPosition.y += 1f;
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void ManualCameraMove (Vector3 delta)
    {
        // Zoom Camera based on input
        transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
        lastMousePosition = Input.mousePosition;
        manualControl = true;
        manualControlTimer = manualControlDelay;
    }

    void ManualCameraZoom (float zoom)
    {
        // Move Camera based on input
        float fov = cam.fieldOfView;
        // Might need to add sensitivity
        fov += zoom / zoomLimiter;
        fov = Mathf.Clamp(fov, minZoom, maxZoom);
        cam.fieldOfView = fov;
        manualControl = true;
        manualControlTimer = manualControlDelay;
    }

    void Zoom()
    {
        // Zoom camera to the min fov to keep everyone in frame.
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance()
    {
        // Get the distance between to 2 furthers targets away from each other
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    void Move(bool inCombat)
    {
        // Auto move camera
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition;
        if (inCombat)
            newPosition = centerPoint + combatOffset;
        else
        {
            // Put card in front of the target
            Vector3 targetForward = myPlayer.transform.forward * cardOffset.x;
            // Raise camera up so camera is not in the floor
            centerPoint.y += cardOffset.y;
            newPosition = centerPoint + targetForward;
        }

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    Vector3 GetCenterPoint()
    {
        // Get the center point between all players
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    void GetCameraTargets()
    {
        targets.Clear();
        // Get all team memebers
        for (int i = 0; i < gameManager.numberOfTeams; i++)
        {
            // Get members of team
            GameObject[] teamMembers = GameObject.FindGameObjectsWithTag("Team " + (i + 1));
            foreach (GameObject member in teamMembers)
            {
                // If member is NOT dead add to target
                if (!member.GetComponent<Base_Stats>().dead)
                    targets.Add(member.transform);
            }
        }
    }
}
