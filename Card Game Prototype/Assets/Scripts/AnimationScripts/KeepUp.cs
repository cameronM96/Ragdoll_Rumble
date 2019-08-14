using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUp : MonoBehaviour
{
    public Rigidbody rb;
    public float desiredHeight;
    public float rayCastAddition = 2f;
    public float forceScalar = 10f;
    public float baseForce = 620;

    public float currentDistance;

    public LayerMask layerMask;

    public GameObject refBall;

    public bool keepRot;
    public bool rightHand;
    public StateController sC;
    public float rotForce;

    private Vector3 torqueForce;
    private Vector3 force;

    private void Start()
    {
        desiredHeight = transform.position.y;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.isKinematic)
            return;

        //Vertical Force
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, desiredHeight + rayCastAddition, layerMask))
        {
            if (refBall != null)
                refBall.transform.position = hit.point;

            float yHit = hit.point.y;
            float targetY = yHit + desiredHeight;
            currentDistance = targetY - transform.position.y;
            // Only add force up don't try and dive down
            if (currentDistance > 0)
                force = new Vector3(0, baseForce + (currentDistance * forceScalar), 0);
        }
        else
            force = Vector3.zero;

        // Rotational Force
        if (keepRot)
        {
            if (sC.chaseTarget != null)
            {
                Vector3 targetDelta = sC.chaseTarget.position - transform.position;

                //get the angle between transform.forward and target delta
                float angleDiff;
                if (rightHand)
                    angleDiff = Vector3.Angle(transform.right, targetDelta);
                else
                    angleDiff = Vector3.Angle(-transform.right, targetDelta);

                // get its cross product, which is the axis of rotation to
                // get from one vector to the other
                Vector3 cross = Vector3.Cross(transform.forward, targetDelta);

                // apply torque along that axis according to the magnitude of the angle.
                torqueForce = (cross * angleDiff * rotForce);
            }
            else
                torqueForce = Vector3.zero;
        }
        else
            torqueForce = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (rb.isKinematic)
            return;
        // Apply force
        rb.AddForce(force, ForceMode.Force);
        rb.AddTorque(torqueForce);
    }
}
