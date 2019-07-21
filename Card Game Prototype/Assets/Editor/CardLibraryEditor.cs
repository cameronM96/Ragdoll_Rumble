using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System.IO;

[CustomEditor(typeof(CardLibrary))]
public class CardLibraryEditor : Editor
{
    private CardLibrary cardLibraryInstance;

    private void OnEnable()
    {
        cardLibraryInstance = (CardLibrary)target;
    }

    public override void OnInspectorGUI()
    {

        if (GUILayout.Button("Add Card"))
        {
            if(Selection.activeObject)
            {
                if (Selection.activeObject is Card)
                    cardLibraryInstance.AddCardToLibrary((Card)Selection.activeObject);
            }
        }

        if (GUILayout.Button("Remove Card"))
        {
            if (Selection.activeObject)
            {
                if (Selection.activeObject is Card)
                    cardLibraryInstance.RemoveCardFromLibrary((Card)Selection.activeObject);
            }
        }

        if (GUILayout.Button("Clear Library"))
        {
            cardLibraryInstance.ClearLibrary();
        }

        GUILayout.Space(10);

        cardLibraryInstance.storedPath = EditorGUILayout.TextField("Folder Path", cardLibraryInstance.storedPath);
        string path = cardLibraryInstance.storedPath;

        if (path != "") {
            cardLibraryInstance.newLibrary = EditorGUILayout.Toggle("Load New Library", cardLibraryInstance.newLibrary);

            if (GUILayout.Button("Load Library"))
            {
                LoadLibraryFromFolder(cardLibraryInstance.newLibrary, path);
            }
        }

        GUILayout.Space(10);
        base.OnInspectorGUI();
    }

    public void LoadLibraryFromFolder(bool newLibrary, string path)
    {
        if (newLibrary)
            cardLibraryInstance.cardLibrary.Clear();

        ArrayList al = new ArrayList();
        // Find all folders in path
        Debug.Log(Application.dataPath + "/" + path + "/");
        string[] folders = Directory.GetDirectories(Application.dataPath + "/" + path + "/");
        if (folders != null)
        {
            foreach (string folder in folders)
            {
                // Make path fit the project folder scheme
                int index = folder.LastIndexOf("/");
                string localPath = path;

                if (index > 0)
                    localPath += folder.Substring(index);

                // Recursively loop through each folder
                LoadLibraryFromFolder(false, localPath);
            }
        }

        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path + "/");

        foreach (string fileName in fileEntries)
        {
            // Make path fit the project folder scheme
            int index = fileName.LastIndexOf("/");
            string localPath = "Assets/" + path;

            if (index > 0)
                localPath += fileName.Substring(index);

            // load the object if it is a card
            object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(Card));

            if (t != null)
                al.Add(t);
        }

        // Load cards into card library
        for (int i = 0; i < al.Count; i++)
        {
            cardLibraryInstance.cardLibrary.Add((Card)al[i]);
        }

        Debug.Log("Card Library Updated");
    }
}