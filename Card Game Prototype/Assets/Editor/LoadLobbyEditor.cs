using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoadLobby))]
public class LoadLobbyEditor : Editor
{
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
    }
}
