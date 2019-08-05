using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PackOfCards))]
public class PackEditor : Editor
{
    SerializedProperty m_gCardList;
    SerializedProperty m_rCardList;

    private void OnEnable()
    {
        m_gCardList = serializedObject.FindProperty("gSpecificCards");
        m_rCardList = serializedObject.FindProperty("rSpecificCards");
    }

    public override void OnInspectorGUI()
    {
        PackOfCards pack = target as PackOfCards;

        // get pack size
        EditorGUILayout.LabelField(new GUIContent("Pack Size: " + pack.packSize));

        pack.iD = EditorGUILayout.IntField(new GUIContent("ID: "), pack.iD);
        pack.packName = EditorGUILayout.TextField("Pack Name: ", pack.packName);
        pack.packImage = (Sprite)EditorGUILayout.ObjectField("", pack.packImage, typeof(Sprite), true);
        pack.coinCost = EditorGUILayout.IntField(new GUIContent("Coin Cost: "), pack.coinCost);
        pack.gemCost = EditorGUILayout.IntField(new GUIContent("Gem Cost: "), pack.gemCost);

        // Guaranteed Specific Cards
        pack.guaranteedSpecificCards = EditorGUILayout.Toggle("Specific Cards: ", pack.guaranteedSpecificCards);
        if (pack.guaranteedSpecificCards)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_gCardList, new GUIContent("Cards: "), true);
            EditorGUI.indentLevel--;
        }

        // Random Specific Cards
        pack.randomSpecificCards = EditorGUILayout.Toggle("Random Card Set: ", pack.randomSpecificCards);
        if (pack.randomSpecificCards)
        {
            EditorGUI.indentLevel++;
            pack.setName = EditorGUILayout.TextField("Set Name: ", pack.setName);
            pack.guaranteedFromSet = EditorGUILayout.IntField(new GUIContent("Guaranteed from set: "), pack.guaranteedFromSet);
            EditorGUILayout.PropertyField(m_rCardList, new GUIContent("Cards: "), true);
            EditorGUI.indentLevel--;
        }

        // Random Rarity
        pack.guaranteedRandom = EditorGUILayout.Toggle("Guaranteed Rarities: ", pack.guaranteedRandom);
        if (pack.guaranteedRandom)
        {
            EditorGUI.indentLevel++;
            pack.numberOfRares = EditorGUILayout.IntField(new GUIContent("Rares: "), pack.numberOfRares);
            pack.numberOfUncommons = EditorGUILayout.IntField(new GUIContent("Uncommons: "), pack.numberOfUncommons);
            pack.numberOfCommons = EditorGUILayout.IntField(new GUIContent("Commons: "), pack.numberOfCommons);
            EditorGUI.indentLevel--;
        }

        // Random Cards
        pack.randomCard = EditorGUILayout.Toggle("Random Cards: ", pack.randomCard);
        if (pack.randomCard)
        {
            EditorGUI.indentLevel++;
            pack.numberOfRandomCards = EditorGUILayout.IntField(new GUIContent("Random #: "), pack.numberOfRandomCards);
            EditorGUI.indentLevel--;
        }

        if (GUILayout.Button("Update ID"))
        {
            pack.iD = pack.GetInstanceID();
            EditorUtility.SetDirty(pack);
            AssetDatabase.SaveAssets();
        }
        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();


        int size = 0;
        if (pack.gSpecificCards != null)
            size += pack.gSpecificCards.Count;
        size += pack.guaranteedFromSet;
        size += pack.numberOfRares;
        size += pack.numberOfUncommons;
        size += pack.numberOfCommons;
        size += pack.numberOfRandomCards;

        pack.packSize = size;
    }
}
