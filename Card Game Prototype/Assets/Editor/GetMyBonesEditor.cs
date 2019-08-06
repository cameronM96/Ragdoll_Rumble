using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(GetMeParent))]
public class GetMyBonesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GetMeParent setbonesscript = target as GetMeParent;

        if(GUILayout.Button("Set ME MA BNONEZ"))
        {
            setbonesscript.SetMyBones();
            Debug.Log("Settings Bonezzsdf");
        }
    }
}
