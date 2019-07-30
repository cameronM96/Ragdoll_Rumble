using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoadLobby))]
public class LoadLobbyEditor : Editor
{
    public string[] difficulties = new string[] { "Easy", "Normal", "Hard" };
    public string[] bestOf = new string[] { "1", "3", "5", "7", "9", "11" };

    public override void OnInspectorGUI()
    {
        LoadLobby loadScene = target as LoadLobby;

        loadScene.lobbySceneNumber = EditorGUILayout.IntField(loadScene.lobbySceneNumber);

        loadScene.randomScene = EditorGUILayout.Toggle("Use RandomScene", loadScene.randomScene);

        if (loadScene.randomScene)
        {
            loadScene.sceneNumberRange = EditorGUILayout.Vector2Field("Scene Range:", loadScene.sceneNumberRange);
        }
        else
        {
            loadScene.usingSceneName = EditorGUILayout.Toggle("Use Scene Name", loadScene.usingSceneName);

            if (loadScene.usingSceneName)
                loadScene.nextLevelName = EditorGUILayout.TextField(loadScene.nextLevelName);
            else
                loadScene.nextLevelNumber = EditorGUILayout.IntField(loadScene.nextLevelNumber);
        }

        loadScene.isCampaign = EditorGUILayout.Toggle("Is Campaign?", loadScene.isCampaign);
        if (loadScene.isCampaign)
        {
            loadScene.campaignNumber = EditorGUILayout.IntField(loadScene.campaignNumber);
            loadScene.difficulty = EditorGUILayout.Popup(loadScene.difficulty, difficulties);
            loadScene.rounds = EditorGUILayout.Popup(loadScene.rounds, bestOf);
        }
        else
            loadScene.campaignNumber = -1;

    }
}
