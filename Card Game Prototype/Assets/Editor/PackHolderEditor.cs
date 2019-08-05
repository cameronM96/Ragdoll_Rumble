using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(PacksHolder))]
public class PackHolderEditor : Editor
{
    private PacksHolder packHolderInstance;

    private void OnEnable()
    {
        packHolderInstance = (PacksHolder)target;
    }

    public override void OnInspectorGUI()
    {
        PacksHolder packHolder = target as PacksHolder;

        if (GUILayout.Button("Add Pack"))
        {
            if (Selection.activeObject)
            {
                if (Selection.activeObject is PackOfCards)
                {
                    packHolderInstance.AddCardToLibrary((PackOfCards)Selection.activeObject);
                    EditorUtility.SetDirty(packHolder);
                    AssetDatabase.SaveAssets();
                }
            }
        }

        if (GUILayout.Button("Remove Pack"))
        {
            if (Selection.activeObject)
            {
                if (Selection.activeObject is PackOfCards)
                {
                    packHolderInstance.RemoveCardFromLibrary((PackOfCards)Selection.activeObject);
                    EditorUtility.SetDirty(packHolder);
                    AssetDatabase.SaveAssets();
                }
            }
        }

        if (GUILayout.Button("Clear Library"))
        {
            packHolderInstance.ClearLibrary();
            EditorUtility.SetDirty(packHolder);
            AssetDatabase.SaveAssets();
        }

        GUILayout.Space(10);

        packHolderInstance.storedPath = EditorGUILayout.TextField("Folder Path", packHolderInstance.storedPath);

        packHolderInstance.newLibrary = EditorGUILayout.Toggle("Load New Library", packHolderInstance.newLibrary);

        if (GUILayout.Button("Load Library"))
        {
            if (packHolderInstance.storedPath != "")
            {
                LoadLibraryFromFolder(packHolderInstance.newLibrary, packHolderInstance.storedPath);
                EditorUtility.SetDirty(packHolder);
                AssetDatabase.SaveAssets();
            }
            else
                Debug.Log("Need to add a file path!");
        }

        GUILayout.Space(10);
        base.OnInspectorGUI();
    }

    public void LoadLibraryFromFolder(bool newLibrary, string path)
    {
        if (newLibrary)
            packHolderInstance.packs.Clear();

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
            object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(PackOfCards));

            if (t != null)
                al.Add(t);
        }

        // Load cards into card library
        for (int i = 0; i < al.Count; i++)
        {
            packHolderInstance.packs.Add((PackOfCards)al[i]);
        }

        Debug.Log("Packs Updated");
    }
}
