using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachSkeleton : MonoBehaviour
{
    public Transform skeletonBase;
    public Transform[] matchingSkeleton;
    public Transform[] mySkeleton;
    char[] deliminators = { ':', '_' };

    private void Awake()
    {
        string[] testString = skeletonBase.name.Split(deliminators[1]);
        for (int i = 0; i < testString.Length; i++)
        {
            Debug.Log(testString[i]);
        }
        GetMySkeleton();
        MatchSkeleton();
    }

    private void Update()
    {
        for (int i = 0; i < mySkeleton.Length; i++)
        {
            Transform fakeParent = matchingSkeleton[i];

            if (fakeParent == null)
                return;

            var targetPos = fakeParent.position;
            var targetRot = fakeParent.localRotation;

            mySkeleton[i].position = RotatePointAroundPivot(targetPos, fakeParent.position, targetRot);
            mySkeleton[i].localRotation = targetRot;
        }
    }

    public void GetMySkeleton()
    {
        mySkeleton = skeletonBase.GetComponentsInChildren<Transform>();
    }

    public void MatchSkeleton()
    {
        if (transform?.root?.GetComponent<Base_Stats>() == null)
            return;

        Transform[] playerSkeleton = transform.root.GetComponent<Base_Stats>().myRig;
        List<Transform> pSkeleton = new List<Transform>();
        foreach (Transform bone in playerSkeleton)
            pSkeleton.Add(bone);

        matchingSkeleton = new Transform[mySkeleton.Length];
        for (int i = 0; i < mySkeleton.Length; i++)
        {
            Transform mySkelly = mySkeleton[i];
            Transform removeBone = null;
            string[] myBones = mySkelly.name.Split(deliminators[1]);
            Debug.Log("Looking for " + myBones[myBones.Length - 1]);
            foreach (Transform pSkelly in playerSkeleton)
            {
                string[] pBones = pSkelly.name.Split(deliminators[0]);
                if (pBones.Length > 0 && myBones.Length > 0)
                {
                    if (pBones[pBones.Length - 1] == myBones[myBones.Length - 1])
                    {
                        removeBone = pSkelly;
                        matchingSkeleton[i] = pSkelly;
                        Debug.Log(mySkelly.name.Split(deliminators[1]) + " was found with " + pSkelly.name);
                        break;
                    }
                }
            }

            if (removeBone != null)
                pSkeleton.Remove(removeBone);
        }
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        //Get a direction from the pivot to the point
        Vector3 dir = point - pivot;
        //Rotate vector around pivot
        dir = rotation * dir;
        //Calc the rotated vector
        point = dir + pivot;
        //Return calculated vector
        return point;
    }
}
