using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject myPlayer;
    public List<Transform> targets;

    public Vector3 offset;
    public float smoothTime = 0.5f;

    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private Vector3 velocity;
    private Camera cam;

    private bool combatPhase = false;
    private bool manualControl = false;
    public float manualControlDelay = 5f;
    private float manualControlTimer;
    private Vector3 lastMousePosition;
    public float mouseSensitivity = 1f;
    public float manualControlMinDist = 2f;

    public GameManager gameManager;

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
        combatPhase = true;
        GetCameraTargets();
    }

    public void InitialiseCardPhase()
    {
        combatPhase = false;
        targets.Clear();
        targets.Add(myPlayer.transform);
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void LateUpdate()
    {
        // Key Board Controls
        if (Input.GetMouseButtonDown(0))
            lastMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            if (manualControl || delta.magnitude > manualControlMinDist)
                ManualCameraMove(delta);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            ManualCameraZoom(Input.GetAxis("Mouse ScrollWheel"));

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

            Move();
            Zoom();
        }
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
        fov += zoom;
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

    void Move()
    {
        // Auto move camera
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

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
