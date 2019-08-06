using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMeParent : MonoBehaviour
{
    public void SetMyBones()
    {
        GameObject parent = transform.parent.gameObject;
        HingeJoint joint = GetComponent<HingeJoint>();
        joint.connectedBody = parent.GetComponent<Rigidbody>();
    }
}
