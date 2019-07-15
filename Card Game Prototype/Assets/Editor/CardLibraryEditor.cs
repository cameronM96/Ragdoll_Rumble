using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

[CustomEditor(typeof(CardLibrary))]
public class CardLibraryEditor : Editor
{
    private CardLibrary cardLibrary;
    private bool newLibrary;
    private string path;

    private void OnEnable()
    {
        cardLibrary = (CardLibrary)target;
    }

    public override void OnInspectorGUI()
    {

        if (GUILayout.Button("Add Card"))
        {
            if(Selection.activeObject)
            {
                if (Selection.activeObject is Card)
                    cardLibrary.AddCardToLibrary((Card)Selection.activeObject);
            }
        }

        if (GUILayout.Button("Remove Card"))
        {
            if (Selection.activeObject)
            {
                if (Selection.activeObject is Card)
                    cardLibrary.RemoveCardFromLibrary((Card)Selection.activeObject);
            }
        }

        if (GUILayout.Button("Clear Library"))
        {
            cardLibrary.ClearLibrary();
        }

        GUILayout.Space(10);

        path = EditorGUILayout.TextField("Folder Path", path);

        if (path != "") {
            newLibrary = EditorGUILayout.Toggle("Load New Library", newLibrary);

            if (GUILayout.Button("Load Library"))
            {
                cardLibrary.LoadLibraryFromFolder(newLibrary, path);
            }
        }

        GUILayout.Space(10);
        base.OnInspectorGUI();
    }
}
