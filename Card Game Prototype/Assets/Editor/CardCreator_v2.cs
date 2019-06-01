using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CardCreator_v2 : EditorWindow
{
    public enum CardType { Weapon, Armour, Ability, Behaviour, Environmental };
    public CardType currentCardType = CardType.Weapon;
    public enum PlayableSlot { Head, Chest, Hand, Feet };
    public PlayableSlot playableSlots = PlayableSlot.Head;

    public int rarity = 1;
    string[] rarityNames = new string[] { "Common", "Uncommon", "Rare" };
    int[] rarityValues = { 1, 2, 3 };

    public string cardName = "New Card";
    public string description = "Enter Card Description";

    public Sprite artwork;
    public Sprite background;

    public int attack;
    public int armour;
    public int hP;
    public float speed;
    public float atkSpeed;

    public TriggerCondition triggerCondition;

    public Ability2 ability;

    public GameObject item;

    private CardCreationWindowDefaultValues defaultValues;

    //PreviewWindow
    private GameObject previewTarget;
    private GameObject previewCanvas;
    private GameObject previewCamera;

    Rect windowGroup;
    Rect optionsGroup;
    Rect buttonGroup;

// Initialise Window
[MenuItem("My Tools/Card Creation Tool")]
    static void InitialiseWindow()
    {
        CardCreator_v2 window = GetWindow<CardCreator_v2>("Card Creation Tool", true);
        window.titleContent.tooltip = "Card Creation Tool";
        window.autoRepaintOnSceneChange = true;
        window.Show();
    }

    private void OnEnable()
    {
        //Debug.Log("Enabled!");
        // Load Defaults
        GameObject defaultValuesObject = (GameObject)AssetDatabase.LoadAssetAtPath(
            "Assets/Prefabs/Templates(DO NOT TOUCH)/CardCreationWindowDefaults.prefab", typeof(GameObject));
        defaultValues = defaultValuesObject.GetComponent<CardCreationWindowDefaultValues>();
        //Debug.Log(defaultValues);

        // Set up Preview Camera
        previewCamera = EditorUtility.CreateGameObjectWithHideFlags("PreviewCamera", HideFlags.DontSave);
        previewCamera.AddComponent<Camera>();
        previewCamera.GetComponent<Camera>().targetTexture = defaultValues.previewWindowTexture;
        previewCamera.gameObject.layer = 9;
        //Debug.Log("Camera: " + previewCamera.name);

        // Set up Preview Canvas
        previewCanvas = EditorUtility.CreateGameObjectWithHideFlags("PreviewCanvas",HideFlags.DontSave, typeof(RectTransform));
        previewCanvas.AddComponent<Canvas>();
        previewCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        previewCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(125, 180);
        previewCanvas.GetComponent<Canvas>().worldCamera = previewCamera.GetComponent<Camera>();
        previewCanvas.layer = 9;
        //Debug.Log("Canvas: " + previewCanvas.name);

        // Set up Card Template
        previewTarget = (GameObject)PrefabUtility.InstantiatePrefab(defaultValues.cardTemplate);
        previewTarget.transform.SetParent(previewCanvas.transform);
        previewTarget.GetComponent<RectTransform>().position = new Vector3(0, 0, 175);
        foreach (Transform child in previewTarget.transform)
        {
            child.gameObject.layer = 9;
        }
        //Debug.Log("Target: " + previewTarget.name);

        InitialisePreviewRenderer();

        // Hide everything from sight
        Tools.visibleLayers &= ~((int)Mathf.Pow(2, LayerMask.NameToLayer("CardCreation")));
    }

    private void OnDisable()
    {
        // Destroy defaults
        previewCamera.hideFlags = HideFlags.HideAndDontSave;
        previewCanvas.hideFlags = HideFlags.HideAndDontSave;
        previewTarget.hideFlags = HideFlags.HideAndDontSave;
        DestroyImmediate(previewCamera,true);
        DestroyImmediate(previewTarget, true);
        DestroyImmediate(previewCanvas, true);

        Tools.visibleLayers |= ((int)Mathf.Pow(2, LayerMask.NameToLayer("CardCreation")));
    }

    private void Update()
    {

        Repaint();
    }

    public void OnGUI()
    {
        EditorGUILayout.Space();
        windowGroup = (Rect)EditorGUILayout.BeginHorizontal();
        optionsGroup = (Rect)EditorGUILayout.BeginVertical(GUILayout.MinWidth(250), GUILayout.MaxWidth(500));
        //Put buttons next to each other
        buttonGroup = (Rect)EditorGUILayout.BeginHorizontal();
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
            foreach (GameObject card in Selection.gameObjects)
            {
                Debug.Log("Updating: " + card.name);
                UpdateCard(card);
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //Card Rarity
        rarity = EditorGUILayout.IntPopup("Rarity: ", rarity, rarityNames, rarityValues);

        EditorGUILayout.Space();

        // Card Info
        currentCardType = (CardType)EditorGUILayout.EnumPopup("Card Type: ", currentCardType);
        playableSlots = (PlayableSlot)EditorGUILayout.EnumPopup("PlayableSlots: ", playableSlots);

        cardName = EditorGUILayout.TextField("Card Name: ", cardName);
        description = EditorGUILayout.TextField("Descption: ", description);

        EditorGUILayout.Space();

        // Sprite artwork
        artwork = (Sprite)EditorGUILayout.ObjectField("Art Work: ", artwork, typeof(Sprite), true);

        // Find Background based on card type
        switch (currentCardType)
        {
            case CardType.Weapon:
                background = defaultValues.backgrounds[0];
                break;
            case CardType.Armour:
                background = defaultValues.backgrounds[1];
                break;
            case CardType.Ability:
                background = defaultValues.backgrounds[2];
                break;
            case CardType.Behaviour:
                background = defaultValues.backgrounds[3];
                break;
            case CardType.Environmental:
                background = defaultValues.backgrounds[4];
                break;
            default:
                Debug.Log("Unknown Card Type!");
                break;
        }
        background = (Sprite)EditorGUILayout.ObjectField("Background: ", background, typeof(Sprite), true);

        EditorGUILayout.Space();

        // Base Stats
        attack = EditorGUILayout.IntField("Damage: ", attack);
        armour = EditorGUILayout.IntField("Armour: ", armour);
        hP = EditorGUILayout.IntField("Health: ", hP);
        speed = EditorGUILayout.FloatField("Movement Speed: ", speed);
        atkSpeed = EditorGUILayout.FloatField("Attack Speed: ", atkSpeed);

        EditorGUILayout.Space();

        // Trigger Condition (What triggers the ability)
        triggerCondition = (TriggerCondition)EditorGUILayout.ObjectField("Trigger Condition: ", triggerCondition, typeof(TriggerCondition), true);

        EditorGUILayout.Space();

        // Abilities
        ability = (Ability2)EditorGUILayout.ObjectField("Ability: ", ability, typeof(Ability2), true);

        EditorGUILayout.Space();

        // In-Game model
        item = (GameObject)EditorGUILayout.ObjectField("Item: ", item, typeof(GameObject), true);

        EditorGUILayout.Space();

        // Load Selected Card button
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
                Debug.Log("Updating: " + Selection.activeGameObject.name);
                LoadNewValues(Selection.activeGameObject);
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUILayout.MinWidth(300), GUILayout.MaxWidth(800));
        // Preview Window
        if (previewCamera == null)
        {
            InitialisePreviewRenderer();
        }

        //Debug.Log("Width: " + (windowGroup.width - optionsGroup.width) + "\nHeight: " + windowGroup.height);
        if (windowGroup.width != 0 || optionsGroup.width != 0)
            DrawCard(optionsGroup, windowGroup);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        UpdateCardDisplay();
    }

    private void InitialisePreviewRenderer()
    {
        previewCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        previewCamera.transform.position = new Vector3(0, -1000, -10);
    }

    private void DrawCard(Rect optionsRect ,Rect windowRect)
    {
        //Draws the preview window into the card building window
        //Debug.Log("Rendering Preview Window!");
        GUI.DrawTexture(new Rect(optionsRect.width + 5, 20, windowRect.width - optionsRect.width - 10, windowRect.height), previewCamera.GetComponent<Camera>().targetTexture);
    }

    private void CreateNewCard(GameObject card, string localPath)
    {
        // Creates a new prefab for the card they made
    }

    private void UpdateCard(GameObject card)
    {
        // Updates an old prefab with new values
    }

    private void LoadNewValues(GameObject card)
    {
        // Draw selected object
        if (card == null)
        {
            EditorGUILayout.LabelField("No game object selected.");
            return;
        }

        if (card.GetComponent<CardDisplay>() == null)
        {
            EditorGUILayout.LabelField("Game object does not contain the required components.");
            return;
        }

        if (card.GetComponent<CardDisplay>().card == null)
        {
            EditorGUILayout.LabelField("Game object does not contain the required components.");
            return;
        }

        // Update to values stored on selected card
        Card cardData = card.GetComponent<CardDisplay>().card;
        EditorGUILayout.LabelField("Selected: " + cardData.cardName);

        switch (cardData.getCardType())
        {
            case 0:
                currentCardType = CardType.Weapon;
                break;
            case 1:
                currentCardType = CardType.Armour;
                break;
            case 2:
                currentCardType = CardType.Ability;
                break;
            case 3:
                currentCardType = CardType.Behaviour;
                break;
            case 4:
                currentCardType = CardType.Environmental;
                break;
            default:
                break;
        }

        switch (cardData.getPlayableSlot())
        {
            case 0:
                playableSlots = PlayableSlot.Head;
                break;
            case 1:
                playableSlots = PlayableSlot.Chest;
                break;
            case 2:
                playableSlots = PlayableSlot.Hand;
                break;
            case 3:
                playableSlots = PlayableSlot.Feet;
                break;
            default:
                break;
        }

        cardName = cardData.name;
        description = cardData.description;

        artwork = cardData.artwork;
        background = cardData.background;

        attack = cardData.attack;
        armour = cardData.armour;
        hP = cardData.hP;
        speed = cardData.speed;
        atkSpeed = cardData.atkSpeed;

        triggerCondition = cardData.triggerCondition;

        ability = cardData.ability;

        item = cardData.item[0];


}

    private void UpdateCardDisplay()
    {
        // Changes the visuals on the card in the preview window based on data from the options list
        Text nameText = previewTarget.transform.GetChild(0).GetComponent<Text>();
        nameText.text = cardName;

        if (background != null)
            previewTarget.GetComponent<Image>().sprite = background;

        if (artwork != null)
            previewTarget.transform.GetChild(1).GetComponent<Image>().sprite = artwork;

        if (triggerCondition != null && ability != null)
        {
            Text infoText = previewTarget.transform.GetChild(2).GetComponent<Text>();
            infoText.text = "<b>" + triggerCondition.tcName + ":</b>\n" + ability.aDescription;
        }
    }
}
