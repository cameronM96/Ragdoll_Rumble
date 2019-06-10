using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using EnumTypes;

public class CardCreator_v2 : EditorWindow
{
    public Rarity rarity = Rarity.None;
    public CardType currentCardType = CardType.None;
    public PlayableSlot playableSlots = PlayableSlot.None;
    
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

    public SO_Ability ability;

    public GameObject item;

    private CardCreationWindowDefaultValues defaultValues;

    //Window Style
    private bool disableButtons = true;
    private bool cardInfoToggle = true;
    private bool cardVisualsToggle = true;
    private bool cardModifersToggle = true;
    private bool baseStatsToggle = true;
    public Vector2 scrollPos;

    //PreviewWindow
    private GameObject previewTarget;
    private GameObject previewCanvas;
    private GameObject cameraObject;
    private Camera previewCamera;
    RenderTexture renderTexture;

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
        cameraObject = EditorUtility.CreateGameObjectWithHideFlags("PreviewCamera", HideFlags.DontSave);
        cameraObject.SetActive(false);
        cameraObject.AddComponent<Camera>();
        previewCamera = cameraObject.GetComponent<Camera>();
        previewCamera.targetDisplay = 7;
        cameraObject.SetActive(true);

        renderTexture = new RenderTexture(
            Mathf.RoundToInt(windowGroup.width - optionsGroup.width) - 10,
            Mathf.RoundToInt(Mathf.Clamp(windowGroup.height, 300, 450)), 
            (int)RenderTextureFormat.ARGB32);

        cameraObject.gameObject.layer = 9;
        //Debug.Log("Camera: " + previewCamera.name);

        // Set up Preview Canvas
        previewCanvas = EditorUtility.CreateGameObjectWithHideFlags("PreviewCanvas",HideFlags.DontSave, typeof(RectTransform));
        previewCanvas.AddComponent<Canvas>();
        previewCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        previewCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(125, 180);
        previewCanvas.GetComponent<Canvas>().worldCamera = previewCamera;
        previewCanvas.layer = 9;
        //Debug.Log("Canvas: " + previewCanvas.name);

        // Set up Card Template
        //previewTarget = (GameObject)PrefabUtility.InstantiatePrefab(defaultValues.cardTemplate);
        previewTarget = Instantiate(defaultValues.cardTemplate);
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

        // Draw the card preivew window
        //DrawCard();
    }

    public void Update()
    {
        if (previewCamera != null && renderTexture.width > 0 && renderTexture.height > 0)
        {
            previewCamera.targetTexture = renderTexture;
            previewCamera.Render();
            previewCamera.targetTexture = null;
        }

        if ((renderTexture.width != Mathf.RoundToInt(windowGroup.width - optionsGroup.width) - 10 ||
            renderTexture.height != Mathf.RoundToInt(Mathf.Clamp(windowGroup.height, 300, 450))))
            renderTexture = new RenderTexture(
                Mathf.RoundToInt(windowGroup.width - optionsGroup.width) - 10,
                Mathf.RoundToInt(Mathf.Clamp(windowGroup.height, 300, 450)),
                (int)RenderTextureFormat.ARGB32);
    }

    private void OnDisable()
    {
        // Destroy defaults
        cameraObject.hideFlags = HideFlags.HideAndDontSave;
        previewCanvas.hideFlags = HideFlags.HideAndDontSave;
        previewTarget.hideFlags = HideFlags.HideAndDontSave;

        Tools.visibleLayers |= ((int)Mathf.Pow(2, LayerMask.NameToLayer("CardCreation")));
        previewCamera = null;

        //Debug.Log("Ignore the error below this comment. I think it's a bug which has be filed with Unity - Cameron M");
        DestroyImmediate(previewTarget, true);
        DestroyImmediate(previewCanvas, true);
        DestroyImmediate(cameraObject, true);

        cameraObject = null;
        previewCanvas = null;
        previewTarget = null;
    }

    public void OnGUI()
    {
        LoadUI();
    }

    private void LoadUI()
    {
        EditorGUILayout.Space();
        windowGroup = (Rect)EditorGUILayout.BeginHorizontal();
        optionsGroup = (Rect)EditorGUILayout.BeginVertical(GUILayout.MinWidth(250), GUILayout.MaxWidth(500));
        //Put buttons next to each other
        buttonGroup = (Rect)EditorGUILayout.BeginHorizontal();

        GUIContent createCardButton;
        GUIContent updateCardButton;
        // Check if all required fields have been filled out.
        if (rarity != Rarity.None &&
            currentCardType != CardType.None &&
            playableSlots != PlayableSlot.None &&
            (cardName != "" || cardName != "New Card") &&
            artwork != null)
        {
            disableButtons = false;
            createCardButton = new GUIContent("Create Card");
            updateCardButton = new GUIContent("Update Card");
        }
        else
        {
            disableButtons = true;
            createCardButton = new GUIContent("Create Card", "Not all required fields have been filled out!");
            updateCardButton = new GUIContent("Update Card", "Not all required fields have been filled out!");
        }

        // Disable buttons unless all required fields have been filled out.
        EditorGUI.BeginDisabledGroup(disableButtons);
        // Create Card button
        if (GUILayout.Button(createCardButton))
        {
            // Create save path
            string localPath = "Assets/ScriptableObjects/Cards/" + currentCardType.ToString() + "/" + cardName + ".asset";

            if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
            {
                // Card already exists, double check with user.
                if (EditorUtility.DisplayDialog("Are you sure?",
                    "The Card already exists. Do you want to overwrite it?",
                    "Yes",
                    "No"))
                {
                    // Create Card
                    Debug.Log("Creating: " + cardName);
                    CreateNewCard(localPath);
                }
            }
            else
            {
                // Create Card
                Debug.Log("Creating: " + cardName);
                CreateNewCard(localPath);
            }
        }

        // Update Card button
        if (GUILayout.Button(updateCardButton))
        {
            if (Selection.activeObject is Card selectedCard)
            {
                Debug.Log("Updating: " + selectedCard.name);
                UpdateCard(selectedCard);
            }
        }

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();

        // Scroll group
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUIStyle groupHeaderStyle = new GUIStyle(EditorStyles.foldoutHeader)
        {
            richText = true
        };

        // Card Info
        cardInfoToggle = EditorGUILayout.BeginFoldoutHeaderGroup(cardInfoToggle, "<size=13><b>Card Info:</b></size>", groupHeaderStyle);
        if (cardInfoToggle)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            rarity = (Rarity)EditorGUILayout.EnumPopup("Rarity: ", rarity);
            currentCardType = (CardType)EditorGUILayout.EnumPopup("Card Type: ", currentCardType);
            playableSlots = (PlayableSlot)EditorGUILayout.EnumFlagsField("PlayableSlots: ", playableSlots);
        }
        else
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.EndFoldoutHeaderGroup();

        // Card Visuals
        EditorGUILayout.Space();
        cardVisualsToggle = EditorGUILayout.BeginFoldoutHeaderGroup(cardVisualsToggle, "<size=13><b>Card Visuals:</b></size>", groupHeaderStyle);
        if (cardVisualsToggle)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            cardName = EditorGUILayout.TextField("Card Name: ", cardName);
            EditorGUILayout.LabelField("Description:");
            description = EditorGUILayout.TextArea(description);

            // Sprite artwork
            artwork = (Sprite)EditorGUILayout.ObjectField("Art Work: ", artwork, typeof(Sprite), true);

            // Find Background based on card type/Rarity
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
                case CardType.None:
                    background = null;
                    break;
                default:
                    Debug.Log("Unknown Card Type!");
                    break;
            }
            background = (Sprite)EditorGUILayout.ObjectField("Background: ", background, typeof(Sprite), true);
        }
        else
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.EndFoldoutHeaderGroup();

        // Card Modifiers
        EditorGUILayout.Space();
        cardModifersToggle = EditorGUILayout.BeginFoldoutHeaderGroup(cardModifersToggle, "<size=13><b>Card Modifiers:</b></size>", groupHeaderStyle);
        if (cardModifersToggle)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            baseStatsToggle = EditorGUILayout.Foldout(baseStatsToggle, "Base Stats:");
            // Base Stats
            if (baseStatsToggle)
            {
                EditorGUI.indentLevel++;
                attack = EditorGUILayout.IntField("Damage: ", attack);
                armour = EditorGUILayout.IntField("Armour: ", armour);
                hP = EditorGUILayout.IntField("Health: ", hP);
                speed = EditorGUILayout.FloatField("Movement Speed: ", speed);
                atkSpeed = EditorGUILayout.FloatField("Attack Speed: ", atkSpeed);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            // Abilities
            ability = (SO_Ability)EditorGUILayout.ObjectField("Ability: ", ability, typeof(SO_Ability), true);

            EditorGUILayout.Space();

            // In-Game model
            item = (GameObject)EditorGUILayout.ObjectField("Item: ", item, typeof(GameObject), true);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.EndScrollView();

        GUILayout.FlexibleSpace();

        // Load Selected Card button
        if (GUILayout.Button("Load Card", GUILayout.Height(40)))
        {
            Debug.Log("Loading: " + Selection.activeObject.name);
            if (Selection.activeObject is Card selectedCard)
                LoadNewValues(selectedCard);
        }

        EditorGUILayout.Space();

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUILayout.MinWidth(300), GUILayout.MaxWidth(800));
        // Preview Window
        if (cameraObject == null)
        {
            InitialisePreviewRenderer();
        }

        //Debug.Log("Width: " + (windowGroup.width - optionsGroup.width) + "\nHeight: " + windowGroup.height);
        if (windowGroup.width != 0 || optionsGroup.width != 0)
            DrawCard(optionsGroup, windowGroup);
        
        EditorGUILayout.Space();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        //New Selection (updates the card visuals)
        //Object[] currentSelection = Selection.objects;
        //Object[] newSelection = new Object[1];
        //newSelection[0] = previewTarget;
        //Selection.objects = newSelection;

        UpdateCardDisplay();

        //Selection.objects = currentSelection;
    }

    private void InitialisePreviewRenderer()
    {
        cameraObject.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        cameraObject.transform.position = new Vector3(0, -1000, -10);
    }

    private void DrawCard(Rect optionsRect ,Rect windowRect)
    {
        //Draws the preview window into the card building window
        //Debug.Log("Rendering Preview Window!");
        GUI.DrawTexture(new Rect(
            optionsRect.width + 5, 
            20, 
            windowRect.width - optionsRect.width - 10, 
            Mathf.Clamp(windowRect.height,300,450)),
            renderTexture);
    }

    private void CreateNewCard(string filePath)
    {
        // Creates a new prefab for the card they made
        Card newCard = ScriptableObject.CreateInstance<Card>();

        // Card Info
        newCard.rarity = rarity;
        newCard.currentCardType = currentCardType;
        newCard.playableSlots = playableSlots;

        // Card Visuals
        newCard.cardName = cardName;
        newCard.description = description;
        newCard.artwork = artwork;
        newCard.background = background;

        // Card Modifiers
        newCard.attack = attack;
        newCard.armour = armour;
        newCard.hP = hP;
        newCard.speed = speed;
        newCard.atkSpeed = atkSpeed;

        newCard.ability = ability;

        newCard.item = item;


        GameObject templateCard = (GameObject)AssetDatabase.LoadAssetAtPath(
            "Assets/Prefabs/Templates(DO NOT TOUCH)/Card_Template.prefab", typeof(GameObject));
        if (templateCard != null)
            newCard.templateCard = templateCard;
        else
            Debug.Log("Template card not found!");

        // Save Card
        AssetDatabase.CreateAsset(newCard, filePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(newCard.cardName + " was created!");
    }

    private void UpdateCard(Card card)
    {
        // Updates an old prefab with new values
        if (card == null)
        {
            EditorGUILayout.LabelField("No game object selected.");
            return;
        }

        // Card Info
        card.rarity = rarity;
        card.currentCardType = currentCardType;
        card.playableSlots = playableSlots;

        // Card Visuals
        card.cardName = cardName;
        card.description = description;
        card.artwork = artwork;
        card.background = background;

        // Card Modifiers
        card.attack = attack;
        card.armour = armour;
        card.hP = hP;
        card.speed = speed;
        card.atkSpeed = atkSpeed;

        card.ability = ability;

        card.item = item;

        GameObject templateCard = (GameObject)AssetDatabase.LoadAssetAtPath(
            "Assets/Prefabs/Templates(DO NOT TOUCH)/Card_Template.prefab", typeof(GameObject));
        if (templateCard != null)
            card.templateCard = templateCard;
        else
            Debug.Log("Template card not found!");

        EditorUtility.SetDirty(card);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void LoadNewValues(Card card)
    {
        // Draw selected object
        if (card == null)
        {
            EditorGUILayout.LabelField("No game object selected.");
            return;
        }

        // Card Info
        rarity = card.rarity;
        currentCardType = card.currentCardType;
        playableSlots = card.playableSlots;  
        
        // Card Visuals
        cardName = card.name;
        description = card.description;
        artwork = card.artwork;
        background = card.background;

        // Card Modifiers
        attack = card.attack;
        armour = card.armour;
        hP = card.hP;
        speed = card.speed;
        atkSpeed = card.atkSpeed;

        ability = card.ability;

        item = card.item;
}

    private void UpdateCardDisplay()
    {
        //Debug.Log("Updating Display!");
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
            infoText.text = "<b>" + triggerCondition.tcName + ":</b>\n" + ability.abDescription;
        }
    }
}
