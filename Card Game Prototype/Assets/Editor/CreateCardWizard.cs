using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateCardWizard : ScriptableWizard
{
    public enum CardType { Weapon, Armour, Ability, Behaviour, Environmental};
    public CardType currentCardType = CardType.Weapon;
    public enum PlayableSlot { Head, Chest, Hand, Feet};
    public PlayableSlot playableSlots;

    public string cardName = "New Card";
    public string description = "Enter Card Description";

    public Sprite artwork;
    public Sprite backgrounds;

    public int damage;
    public int armour;
    public int hP;
    public float speed;
    public float atkSpeed;

    public Ability ability;
    public string abilityDescription = "Enter Ability Description";

    public GameObject[] item;

    [MenuItem("My Tools/Create Card Wizard...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<CreateCardWizard>("Create Card", "Create New", "Update Selected");
    }

    private void OnWizardCreate()
    {
        //Create Card Template
        GameObject newCard = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Card_Template.prefab", typeof(GameObject)));
        newCard.transform.SetParent(GameObject.FindGameObjectWithTag("CardCanvas").transform);
        newCard.name = cardName;

        string localPath = "Assets/Prefabs/" + currentCardType + "/" + cardName + ".prefab";

        if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
        {
            if (EditorUtility.DisplayDialog("Are you sure?",
                "The Prefab already exists. Do you want to overwrite it?",
                "Yes",
                "No"))
            {
                CreateNewCard(newCard, localPath);
            }
        }
    }

    private void OnWizardOtherButton()
    {
        
    }

    private void OnWizardUpdate()
    {
        helpString = "Enter card details";
    }

    private void CreateNewCard(GameObject card, string localPath)
    {

    }
}