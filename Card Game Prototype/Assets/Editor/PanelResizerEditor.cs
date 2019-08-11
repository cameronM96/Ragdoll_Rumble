using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PanelResizer))]
public class PanelResizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PanelResizer panelResizer = target as PanelResizer;

        if (GUILayout.Button("Resize Panel"))
        {
            panelResizer.Resize();
        }

        base.OnInspectorGUI();
    }
}
