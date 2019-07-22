using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Player player = target as Player;
        base.OnInspectorGUI();

        if (GUILayout.Button("Print Values"))
        {
            player.PrintValues();
        }

        if (GUILayout.Button("Load Profile"))
        {
            player.LoadGame();
        }
    }
}
