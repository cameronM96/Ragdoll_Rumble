﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetTest : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public GameObject target;

    private void Update()
    {
        if (navAgent.enabled)
            navAgent.destination = target.transform.position;
    }
}
