using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    string tempName;

    private void OnEnable()
    {
        Player player = target as Player;
        tempName = player.playerName;
    }

    public override void OnInspectorGUI()
    {
        Player player = target as Player;

        //tempName = player.playerName;
        tempName = GUILayout.TextField(tempName);
        if (GUILayout.Button("New Profile"))
        {
            player.NewProfile(tempName);
        }

        base.OnInspectorGUI();

        if (GUILayout.Button("Update Name"))
        {
            player.playerName = tempName;
        }

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
