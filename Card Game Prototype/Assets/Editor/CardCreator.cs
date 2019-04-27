using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CardCreator : EditorWindow
{
    public enum CardType { Weapon, Armour, Ability, Behaviour, Environmental};
    public CardType currentCardType = CardType.Weapon;
    public enum PlayableSlot { Head, Chest, Hand, Feet};
    public PlayableSlot playableSlots = PlayableSlot.Head;

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

    public GameObject item;

    Editor gameObjectEditor;
    private CardCreationWindowDefaultValues defaultValues;
    private GameObject previewTarget;
    private GameObject previewWindowCanvas;

    [MenuItem("Window/Card Creation")]
    static void ShowWindow()
    {
        var window = GetWindow<CardCreator>("Card Creation Tool");
        window.titleContent.tooltip = "Card Creation Tool";
        window.autoRepaintOnSceneChange = true;
        window.Show();
    }

    private void OnEnable()
    {
        // Load Defaults
        GameObject defaultValuesObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/CardCreationWindowDefaults.prefab", typeof(GameObject));
        defaultValues = defaultValuesObject.GetComponent<CardCreationWindowDefaultValues>();
        previewTarget = defaultValues.cardTemplate;
        //previewWindowCanvas = ;
    }

    private void OnDisable()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        //Put buttons next to each other
        EditorGUILayout.BeginHorizontal();
        // Create Card button
        if (GUILayout.Button("Create Card"))
        {
            //Create Card Template
            GameObject newCard = Instantiate(defaultValues.cardTemplate);
            newCard.transform.SetParent(GameObject.FindGameObjectWithTag("CardCanvas").transform);
            newCard.name = cardName;

            // Create save path
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

        // Update Card button
        if (GUILayout.Button("Update Card"))
        {
            foreach(GameObject card in Selection.gameObjects)
            {
                Debug.Log("Updating: " + card.name);
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        // Card Info
        currentCardType = (CardType)EditorGUILayout.EnumPopup("Card Type: ", currentCardType);
        playableSlots = (PlayableSlot)EditorGUILayout.EnumPopup("PlayableSlots: " , playableSlots);

        cardName = EditorGUILayout.TextField("Card Name: ", cardName);
        description = EditorGUILayout.TextField("Descption: ", description);

        EditorGUILayout.Space();

        // Sprite artwork
        artwork = (Sprite)EditorGUILayout.ObjectField("Art Work: ", artwork, typeof(Sprite), true);

        // Find Background based on card type
        switch(currentCardType)
        {
            case CardType.Weapon:
                backgrounds = defaultValues.backgrounds[0];
                break;
            case CardType.Armour:
                backgrounds = defaultValues.backgrounds[1];
                break;
            case CardType.Ability:
                backgrounds = defaultValues.backgrounds[2];
                break;
            case CardType.Behaviour:
                backgrounds = defaultValues.backgrounds[3];
                break;
            case CardType.Environmental:
                backgrounds = defaultValues.backgrounds[4];
                break;
            default:
                Debug.Log("Unknown Card Type!");
                break;
        }
        backgrounds = (Sprite)EditorGUILayout.ObjectField("Background: ", backgrounds, typeof(Sprite), true);

        EditorGUILayout.Space();

        // Base Stats
        damage = EditorGUILayout.IntField("Damage: ", damage);
        armour = EditorGUILayout.IntField("Armour: ", armour);
        hP = EditorGUILayout.IntField("Health: ", hP);
        speed = EditorGUILayout.FloatField("Movement Speed: ", speed);
        atkSpeed = EditorGUILayout.FloatField("Attack Speed: ", atkSpeed);

        EditorGUILayout.Space();

        // Abilities
        abilityDescription = EditorGUILayout.TextField("Ability Description", abilityDescription);

        EditorGUILayout.Space();

        // In-Game model
        item = (GameObject)EditorGUILayout.ObjectField("Item: ", item, typeof(GameObject), true);

        EditorGUILayout.Space();

        // Update Card button
        if (GUILayout.Button("Load Card"))
        {
            // No card selected
            if (Selection.gameObjects.Length == 0)
            {
                Debug.Log("No Card was selected. Please select a card and try again.");
            }
            // Multiple cards are selected
            else if (Selection.gameObjects.Length > 1)
            {
                Debug.Log("Too many cards selected! Please choose 1 and try again.");
            }
            // Just right
            else
            {
                Debug.Log("Updating: " + Selection.gameObjects[0].name);
                LoadNewValues(Selection.gameObjects[0]);
            }
        }

        EditorGUILayout.EndVertical();

        // Preview Window
        if (previewTarget != null)
        {
            if (gameObjectEditor == null)
                gameObjectEditor = Editor.CreateEditor(previewTarget);

            gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect((position.width * 2)/5, position.height), EditorStyles.whiteLabel);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void CreateNewCard(GameObject card, string localPath)
    {

    }

    private void LoadNewValues(GameObject card)
    {

    }
}