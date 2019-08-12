using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUp : MonoBehaviour
{
    public ConstantForce cF;
    public float desiredHeight;
    public float rayCastAddition = 2f;
    public float forceScalar = 10f;
    public float baseForce = 620;

    public float currentDistance;

    public LayerMask layerMask;

    public GameObject refBall;

    private void Start()
    {
        desiredHeight = transform.position.y;
    }

    private void Update()
    {
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
                cF.force = new Vector3(0, baseForce + (currentDistance * forceScalar), 0);
        }
        else
            cF.force = Vector3.zero;
    }
}
