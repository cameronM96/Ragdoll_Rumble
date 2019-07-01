using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMover : MonoBehaviour
{
    public Camera myCamera;
    public Vector3 offset;
    [SerializeField] private GameObject[] target;
    private bool moveSlot;
    private Rect myRect;

    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>().rect;
        StartCoroutine(Initialise());
    }

    IEnumerator Initialise()
    {
        yield return new WaitForSeconds(1f);
        target = GetComponent<DropZone>().slot;
        myCamera = transform.root.GetComponent<UIManager>().myCameraController.cam;
    }


    private void OnEnable()
    {
        GameManager.EnterCardPhase += EnableSlotMover;
        GameManager.EnterCombatPhase += DisableSlotMover;
    }

    private void OnDisable()
    {
        GameManager.EnterCardPhase -= EnableSlotMover;
        GameManager.EnterCombatPhase -= DisableSlotMover;
    }

    public void EnableSlotMover()
    {
        moveSlot = true;
    }

    public void DisableSlotMover()
    {
        moveSlot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveSlot && myCamera != null && target != null)
        {
            // Get Average Position
            Vector3 targetPos = Vector3.zero;
            foreach (GameObject tar in target)
                targetPos += tar.transform.position;

            targetPos /= target.Length;

            // Get screen points
            Vector3 newPosition = myCamera.WorldToScreenPoint(targetPos);
            newPosition -= offset;

            // Move
            this.transform.position = newPosition;
        }
    }
}
