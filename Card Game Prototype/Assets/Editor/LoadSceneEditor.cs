using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoadQuickScene))]
public class LoadSceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LoadQuickScene loadScene = target as LoadQuickScene;

        loadScene.sceneNameCheck = EditorGUILayout.Toggle("Use Scene Name", loadScene.sceneNameCheck);

        if (loadScene.sceneNameCheck)
            loadScene.sceneName = EditorGUILayout.TextField(loadScene.sceneName);
        else
            loadScene.sceneBuildNumber = EditorGUILayout.IntField(loadScene.sceneBuildNumber);
    }
}
