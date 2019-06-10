using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour { 

    public GameObject target;
    public float RotationSpeed = 10;

    void Start()
    {
        
    }

    void Update()
    {
        target.transform.Rotate(new Vector3(0, Time.deltaTime, 0) * RotationSpeed);
    }
}
