using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateCharacterWizard : ScriptableWizard
{
    public Texture2D portraitTexture;
    public Color color = Color.white;
    public string nickname = "Default nickname";

    [MenuItem ("My Tools/Create Character Wizard...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<CreateCharacterWizard>("Create Character", "Create New", "Update Selected");
    }

    private void OnWizardCreate()
    {
        GameObject characterGO = new GameObject();
        Character characterComponent = characterGO.AddComponent<Character>();
        characterComponent.portrait = portraitTexture;
        characterComponent.color = color;
        characterComponent.nickname = nickname;
    }

    private void OnWizardOtherButton()
    {
        if (Selection.activeTransform != null)
        {
            Character characterComponent = Selection.activeTransform.GetComponent<Character>();

            if (characterComponent != null)
            {
                characterComponent.portrait = portraitTexture;
                characterComponent.color = color;
                characterComponent.nickname = nickname;
            }
        }
    }

    private void OnWizardUpdate()
    {
        helpString = "Enter Character details";
    }
}
