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

    public bool pos;
    public bool rot;
    public bool size;
    
    public string cardName = "New Card";
    public string description = "Enter Card Description";

    public Sprite artwork;
    public Sprite background;

    public int attack;
    public int armour;
    public int hP;
    public float speed;
    public float atkSpeed;

    public string abilityDescription;
    public SO_Ability[] abilities;

    public GameObject item;

    private CardCreationWindowDefaultValues defaultValues;
    private CardLibrary cardLibrary;

    private Card loadedCard;

    //Window Style
    private bool disableButtons = true;
    private bool cardInfoToggle = true;
    private bool cardVisualsToggle = true;
    private bool cardModifersToggle = true;
    private bool baseStatsToggle = true;
    private bool arrangementToggle = true;
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
        cardLibrary = (CardLibrary)AssetDatabase.LoadAssetAtPath(
            "Assets/ScriptableObjects/Cards/CardLibrary.asset", typeof(CardLibrary));
        if (cardLibrary == null)
            Debug.LogWarning("Unable to load Library!");
        else
            Debug.Log("Library Successfully loaded");

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
        if (EditorApplication.isPlaying)
        {
            this.Close();
        }

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
        loadedCard = null;
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
        if (rarity != Rarity.None && currentCardType != CardType.None &&
            playableSlots != PlayableSlot.None && 
            (cardName != "" || cardName != "New Card") && artwork != null &&
            (attack != 0 || armour != 0 || hP != 0 || speed != 0 || 
            atkSpeed != 0 || abilities != null || item != null))
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

            if (AssetDatabase.LoadAssetAtPath(localPath, typeof(Card)))
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
                if (selectedCard != loadedCard)
                {
                    if (EditorUtility.DisplayDialog("Are you sure?",
                    "The card you are trying to update is NOT the same as the loaded card. \n" +
                    "are you sure you still want to update?",
                    "Yes",
                    "No"))
                    {
                        Debug.Log("Updating: " + selectedCard.name);
                        UpdateCard(selectedCard);
                    }
                }
                else
                {
                    Debug.Log("Updating: " + selectedCard.name);
                    UpdateCard(selectedCard);
                }
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
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            richText = true
        };
        GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
        {
            richText = true
        };

        // Card Info
        string cardInfoHeader = "<size=13><b>Card Info:</b></size>";

        if (rarity == Rarity.None || currentCardType == CardType.None || playableSlots == PlayableSlot.None)
            cardInfoHeader = "<size=13><color=red>*</color> <b>Card Info:</b></size>";

        cardInfoToggle = EditorGUILayout.BeginFoldoutHeaderGroup(cardInfoToggle, cardInfoHeader, groupHeaderStyle);
        if (cardInfoToggle)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Rarity
            string rarityLabel = "Rarity:";
            if (rarity == Rarity.None)
                rarityLabel = "<color=red>*</color> " + rarityLabel;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(rarityLabel, labelStyle, GUILayout.Width(146));
            rarity = (Rarity)EditorGUILayout.EnumPopup(rarity);
            EditorGUILayout.EndHorizontal();

            // Card Type
            string cardTypeLabel = "Card Type:";
            if (currentCardType == CardType.None)
                cardTypeLabel = "<color=red>*</color> " + cardTypeLabel;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(cardTypeLabel, labelStyle, GUILayout.Width(146));
            currentCardType = (CardType)EditorGUILayout.EnumPopup(currentCardType);
            EditorGUILayout.EndHorizontal();

            // Playable Slots
            string playableSlotLabel = "Playable Slot(s):";
            if (playableSlots == PlayableSlot.None)
                playableSlotLabel = "<color=red>*</color> " + playableSlotLabel;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(playableSlotLabel, labelStyle, GUILayout.Width(146));
            playableSlots = (PlayableSlot)EditorGUILayout.EnumFlagsField(playableSlots);
            EditorGUILayout.EndHorizontal();
        }
        else
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.EndFoldoutHeaderGroup();

        // Card Visuals
        EditorGUILayout.Space();
        string cardVisualsHeader = "<size=13><b>Card Visuals:</b></size>";

        if ((cardName == "" || cardName == "New Card") || artwork == null)
            cardVisualsHeader = "<size=13><color=red>*</color> <b>Card Visuals:</b></size>";

        cardVisualsToggle = EditorGUILayout.BeginFoldoutHeaderGroup(cardVisualsToggle, cardVisualsHeader, groupHeaderStyle);
        if (cardVisualsToggle)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Card Name
            string cardNameLabel = "Card Name:";
            if (cardName == "" || cardName == "New Card")
                cardNameLabel = "<color=red>*</color> " + cardNameLabel;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(cardNameLabel, labelStyle , GUILayout.Width(146));
            cardName = EditorGUILayout.TextField(cardName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Description:");
            description = EditorGUILayout.TextArea(description);

            // Sprite artwork
            string artworkLabel = "Art Work:";
            if (artwork == null)
                artworkLabel = "<color=red>*</color> " + artworkLabel;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(artworkLabel, labelStyle, GUILayout.Width(70));
            artwork = (Sprite)EditorGUILayout.ObjectField("", artwork, typeof(Sprite), true);
            EditorGUILayout.EndHorizontal();

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

        string cardModiferHeader = "<size=13><b>Card Modifiers:</b></size>";

        if (abilities == null && item == null && attack == 0 && armour == 0 && hP == 0 && speed == 0 && atkSpeed == 0)
            cardModiferHeader = "<size=13><color=red>*</color> <b>Card Modifiers:</b></size>";

        cardModifersToggle = EditorGUILayout.BeginFoldoutHeaderGroup(cardModifersToggle, cardModiferHeader, groupHeaderStyle);
        if (cardModifersToggle)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Add red star if there are no abilities or in-game model or basestats (need at least 1 to make a card)
            string baseStatLabel = "Base Stats:";
            string abilityLabel = "Ability:";
            string itemLabel = "Item:";

            if (abilities == null && item == null && attack == 0 && armour == 0 && hP == 0 && speed == 0 && atkSpeed == 0)
            {
                baseStatLabel = "<color=red>*</color> " + baseStatLabel;
                abilityLabel = "<color=red>*</color> " + abilityLabel; ;
                itemLabel = "<color=red>*</color> " + itemLabel;
            }

            baseStatsToggle = EditorGUILayout.Foldout(baseStatsToggle, baseStatLabel, foldoutStyle);
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
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ability Description", labelStyle, GUILayout.Width(146));
            abilityDescription = EditorGUILayout.TextArea(abilityDescription);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(abilityLabel, labelStyle, GUILayout.Width(146));
            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("abilities");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // In-Game model
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(itemLabel, labelStyle, GUILayout.Width(146));
            item = (GameObject)EditorGUILayout.ObjectField(item, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // OrganisationEditorGUILayout.Space();
        string cardArrangementHeader = "<size=13><b>Card Visuals:</b></size>";

        arrangementToggle = EditorGUILayout.BeginFoldoutHeaderGroup(arrangementToggle, cardArrangementHeader, groupHeaderStyle);
        if (arrangementToggle)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            pos = EditorGUILayout.Toggle("Adjust Position: ", pos);
            rot = EditorGUILayout.Toggle("Adjust Rotation: ", rot);
            size = EditorGUILayout.Toggle("Adjust Size: ", size);
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
            else
                Debug.LogError("Not a card! failed to load!");
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
        newCard.iD = newCard.GetInstanceID();
        newCard.rarity = rarity;
        newCard.currentCardType = currentCardType;
        newCard.playableSlots = playableSlots;

        // Card Visuals
        newCard.cardName = cardName;
        newCard.description = description;
        newCard.artwork = artwork;
        newCard.background = background;

        // Rarity
        Sprite rarityImage = null;
        switch (rarity)
        {
            case Rarity.None:
                rarityImage = null;
                break;
            case Rarity.Common:
                rarityImage = defaultValues.rarity[0];
                break;
            case Rarity.Uncommon:
                rarityImage = defaultValues.rarity[1];
                break;
            case Rarity.Rare:
                rarityImage = defaultValues.rarity[2];
                break;
            default:
                Debug.Log("Unknown rarity");
                break;
        }
        if (rarityImage != null)
            newCard.rarityImage = rarityImage;

        // Card Modifiers
        newCard.attack = attack;
        newCard.armour = armour;
        newCard.hP = hP;
        newCard.speed = speed;
        newCard.atkSpeed = atkSpeed;

        newCard.abilityDescription = abilityDescription;
        newCard.abilities = abilities;

        newCard.item = item;
        newCard.pos = pos;
        newCard.rot = rot;
        newCard.size = size;

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

        loadedCard = newCard;

        EditorUtility.DisplayDialog("Card  Created!", "Card was successfully created!", "ok");
        Debug.Log(newCard.cardName + " was created!");

        if (cardLibrary != null)
            cardLibrary.AddCardToLibrary(newCard);
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
        card.iD = card.GetInstanceID();
        card.rarity = rarity;
        card.currentCardType = currentCardType;
        card.playableSlots = playableSlots;

        // Card Visuals
        card.cardName = cardName;
        card.description = description;
        card.artwork = artwork;
        card.background = background;
        card.rarityImage = previewTarget.transform.GetChild(3).GetComponent<Image>().sprite;

        // Card Modifiers
        card.attack = attack;
        card.armour = armour;
        card.hP = hP;
        card.speed = speed;
        card.atkSpeed = atkSpeed;

        card.abilityDescription = abilityDescription;
        card.abilities = abilities;

        card.item = item;
        card.pos = pos;
        card.rot = rot;
        card.size = size;

        GameObject templateCard = (GameObject)AssetDatabase.LoadAssetAtPath(
            "Assets/Prefabs/Templates(DO NOT TOUCH)/Card_Template.prefab", typeof(GameObject));
        if (templateCard != null)
            card.templateCard = templateCard;
        else
            Debug.Log("Template card not found!");

        EditorUtility.SetDirty(card);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Card  Updated!", card.name + " was successfully updated!", "ok");
    }

    private void LoadNewValues(Card card)
    {
        // Draw selected object
        if (card == null)
        {
            EditorGUILayout.LabelField("No game object selected.");
            return;
        }

        loadedCard = card;

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

        abilityDescription = card.abilityDescription;
        abilities = card.abilities;

        item = card.item;

        pos = card.pos;
        rot = card.rot;
        size = card.size;

        Debug.Log("Card successfully loaded");
    }

    private void UpdateCardDisplay()
    {
        //Debug.Log("Updating Display!");
        // Changes the visuals on the card in the preview window based on data from the options list

        // Set name
        Text nameText = previewTarget.transform.GetChild(0).GetComponent<Text>();
        nameText.text = cardName;

        // Set Description
        Text cardInfoBox = previewTarget.transform.GetChild(4).GetComponent<Text>();
        cardInfoBox.text = description;

        // Set background
        if (background != null)
            previewTarget.GetComponent<Image>().sprite = background;

        // Set Artwork
        if (artwork != null)
            previewTarget.transform.GetChild(1).GetComponent<Image>().sprite = artwork;

        // Set modifer Info
        Text infoText = previewTarget.transform.GetChild(2).GetComponent<Text>();
        infoText.text = "";
        // Set base stats
        if (attack != 0)
            infoText.text += "\nAttack: " + attack;
        if (armour != 0)
            infoText.text += "\nArmour: " + armour;
        if (hP != 0)
            infoText.text += "\nHealth: " + hP;
        if (speed != 0)
            infoText.text += "\nSpeed: " + speed;
        if (atkSpeed != 0)
            infoText.text += "\nAttack Speed: " + atkSpeed;

        // Set ability info
        if (abilities != null)
        {
            if (abilities.Length > 0)
                infoText.text += ("\n" + abilityDescription);
        }

        // Set rarity
        Image rarityImage = previewTarget.transform.GetChild(3).GetComponent<Image>();
        switch (rarity)
        {
            case Rarity.None:
                rarityImage.sprite = null;
                break;
            case Rarity.Common:
                rarityImage.sprite = defaultValues.rarity[0];
                break;
            case Rarity.Uncommon:
                rarityImage.sprite = defaultValues.rarity[1];
                break;
            case Rarity.Rare:
                rarityImage.sprite = defaultValues.rarity[2];
                break;
            default:
                Debug.Log("Unknown rarity");
                break;
        }

        // Set Slots
        // Get all slot image rings
        Image[] rings = new Image[4];
        for (int i = 0; i < rings.Length; i++)
            rings[i] = previewTarget.transform.GetChild(5).GetChild(i).GetChild(0).GetComponent<Image>();

        // Update all slots
        PlayableSlot currentSlot = PlayableSlot.None;
        for (int i = 0; i < rings.Length; i++)
        {
            switch (i)
            {
                case 0:
                    currentSlot = PlayableSlot.Head;
                    break;
                case 1:
                    currentSlot = PlayableSlot.Hand;
                    break;
                case 2:
                    currentSlot = PlayableSlot.Chest;
                    break;
                case 3:
                    currentSlot = PlayableSlot.Feet;
                    break;
                default:
                    currentSlot = PlayableSlot.None;
                    Debug.Log("Unknown Slot!");
                    break;
            }

            if (currentSlot == (currentSlot & playableSlots))
                rings[i].color = Color.red;
            else
                rings[i].color = Color.grey;
        }
    }
}