using UnityEngine;
using UnityEngine.UI;

public class TestSceneSetup : MonoBehaviour
{
    [Header("Test Scene Configuration")]
    public bool createTestScene = true;
    public bool includeUI = true;
    public bool includeAudio = true;

    void Awake()
    {
        if (createTestScene)
        {
            SetupTestScene();
        }
    }

    void SetupTestScene()
    {
        Debug.Log("Setting up test scene for 'The 5 Fold Awakening'...");

        // Create main camera if it doesn't exist
        if (Camera.main == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            Camera camera = cameraObj.AddComponent<Camera>();
            cameraObj.AddComponent<AudioListener>();
            cameraObj.tag = "MainCamera";
        }

        // Create player
        CreatePlayer();

        // Create game manager
        CreateGameManager();

        // Create audio manager
        if (includeAudio)
        {
            CreateAudioManager();
        }

        // Create basic UI
        if (includeUI)
        {
            CreateUI();
        }

        // Create basic environment
        CreateEnvironment();

        Debug.Log("Test scene setup complete! Press Play to test the game.");
    }

    void CreatePlayer()
    {
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = Vector3.zero;

        // Add required components
        player.AddComponent<CharacterController>();
        PlayerController playerController = player.AddComponent<PlayerController>();
        CharacterManager characterManager = player.AddComponent<CharacterManager>();
        StorySystem storySystem = player.AddComponent<StorySystem>();

        // Configure player controller
        playerController.moveSpeed = 5f;
        playerController.rotationSpeed = 10f;
        playerController.cameraDistance = 5f;
        playerController.cameraHeight = 2f;

        // Add basic character data
        characterManager.characters = new CharacterManager.Character[]
        {
            new CharacterManager.Character
            {
                name = "Lyra Vale",
                dialogues = new string[]
                {
                    "Welcome to the world of magic and technology, Kael. Your journey begins now.",
                    "Remember, the balance between magic and machines is delicate. Choose wisely.",
                    "The ancient powers are awakening. You must be ready."
                },
                isInteractable = true
            }
        };

        // Add basic story data
        storySystem.storySegments = new StorySystem.StorySegment[]
        {
            new StorySystem.StorySegment
            {
                title = "The Awakening",
                content = "You are Kael Ardin, a young mage in a world where magic and technology coexist. Strange powers have begun to manifest within you, and you must learn to control them before they control you.",
                objectives = new string[]
                {
                    "Explore the village",
                    "Find the ancient tome",
                    "Speak with the village elder"
                }
            }
        };

        Debug.Log("Player created with all necessary components.");
    }

    void CreateGameManager()
    {
        GameObject gmObj = new GameObject("GameManager");
        GameManager gm = gmObj.AddComponent<GameManager>();

        // Configure basic settings
        gm.enableAudio = true;
        gm.enableParticles = false; // Disable for simple test

        Debug.Log("GameManager created.");
    }

    void CreateAudioManager()
    {
        GameObject audioObj = new GameObject("AudioManager");
        AudioManager audio = audioObj.AddComponent<AudioManager>();

        // The AudioManager will create its own AudioSource components
        Debug.Log("AudioManager created. Add audio clips in the inspector for full functionality.");
    }

    void CreateUI()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObj.AddComponent<GraphicRaycaster>();

        // Create Event System
        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // Create Main Menu Panel
        CreateMainMenuPanel(canvasObj);

        // Create Dialogue Panel
        CreateDialoguePanel(canvasObj);

        // Create Story Panel
        CreateStoryPanel(canvasObj);

        Debug.Log("Basic UI created.");
    }

    void CreateMainMenuPanel(GameObject canvas)
    {
        GameObject panel = new GameObject("MainMenuPanel");
        panel.transform.SetParent(canvas.transform, false);

        Image image = panel.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0.8f);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Add title text
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(panel.transform, false);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "THE 5 FOLD AWAKENING";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.fontSize = 48;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = Color.white;

        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.7f);
        titleRect.anchorMax = new Vector2(0.5f, 0.7f);
        titleRect.sizeDelta = new Vector2(600, 60);

        // Add start button
        GameObject buttonObj = new GameObject("StartButton");
        buttonObj.transform.SetParent(panel.transform, false);
        Button button = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = Color.gray;

        GameObject buttonTextObj = new GameObject("ButtonText");
        buttonTextObj.transform.SetParent(buttonObj.transform, false);
        Text buttonText = buttonTextObj.AddComponent<Text>();
        buttonText.text = "START GAME";
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.fontSize = 24;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;

        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.4f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.4f);
        buttonRect.sizeDelta = new Vector2(200, 50);

        RectTransform buttonTextRect = buttonTextObj.GetComponent<RectTransform>();
        buttonTextRect.anchorMin = Vector2.zero;
        buttonTextRect.anchorMax = Vector2.one;
        buttonTextRect.offsetMin = Vector2.zero;
        buttonTextRect.offsetMax = Vector2.zero;

        // Connect button to GameManager
        button.onClick.AddListener(() => {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.StartGame();
        });

        // Assign to GameManager
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null) gm.mainMenuPanel = panel;
    }

    void CreateDialoguePanel(GameObject canvas)
    {
        GameObject panel = new GameObject("DialoguePanel");
        panel.transform.SetParent(canvas.transform, false);
        panel.SetActive(false);

        Image image = panel.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0.9f);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.1f, 0.1f);
        rect.anchorMax = new Vector2(0.9f, 0.4f);

        // Character name text
        GameObject nameObj = new GameObject("CharacterNameText");
        nameObj.transform.SetParent(panel.transform, false);
        Text nameText = nameObj.AddComponent<Text>();
        nameText.text = "Character Name";
        nameText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        nameText.fontSize = 24;
        nameText.color = Color.yellow;

        RectTransform nameRect = nameObj.GetComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0, 1);
        nameRect.anchorMax = new Vector2(1, 1);
        nameRect.offsetMin = new Vector2(20, -50);
        nameRect.offsetMax = new Vector2(-20, -10);

        // Dialogue text
        GameObject dialogueObj = new GameObject("DialogueText");
        dialogueObj.transform.SetParent(panel.transform, false);
        Text dialogueText = dialogueObj.AddComponent<Text>();
        dialogueText.text = "Dialogue text will appear here...";
        dialogueText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        dialogueText.fontSize = 18;
        dialogueText.color = Color.white;

        RectTransform dialogueRect = dialogueObj.GetComponent<RectTransform>();
        dialogueRect.anchorMin = new Vector2(0, 0.3f);
        dialogueRect.anchorMax = new Vector2(1, 0.9f);
        dialogueRect.offsetMin = new Vector2(20, 0);
        dialogueRect.offsetMax = new Vector2(-20, 0);

        // Next button
        GameObject buttonObj = new GameObject("NextButton");
        buttonObj.transform.SetParent(panel.transform, false);
        Button button = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = Color.gray;

        GameObject buttonTextObj = new GameObject("ButtonText");
        buttonTextObj.transform.SetParent(buttonObj.transform, false);
        Text buttonText = buttonTextObj.AddComponent<Text>();
        buttonText.text = "NEXT";
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.fontSize = 16;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;

        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.8f, 0.1f);
        buttonRect.anchorMax = new Vector2(0.95f, 0.25f);

        RectTransform buttonTextRect = buttonTextObj.GetComponent<RectTransform>();
        buttonTextRect.anchorMin = Vector2.zero;
        buttonTextRect.anchorMax = Vector2.one;
        buttonTextRect.offsetMin = Vector2.zero;
        buttonTextRect.offsetMax = Vector2.zero;

        // Assign to CharacterManager
        CharacterManager cm = FindObjectOfType<CharacterManager>();
        if (cm != null)
        {
            cm.dialoguePanel = panel;
            cm.dialogueText = dialogueText;
            cm.characterNameText = nameText;
            cm.nextButton = button;
        }
    }

    void CreateStoryPanel(GameObject canvas)
    {
        GameObject panel = new GameObject("StoryPanel");
        panel.transform.SetParent(canvas.transform, false);
        panel.SetActive(false);

        Image image = panel.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0.9f);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.1f, 0.1f);
        rect.anchorMax = new Vector2(0.9f, 0.9f);

        // Title text
        GameObject titleObj = new GameObject("StoryTitleText");
        titleObj.transform.SetParent(panel.transform, false);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "Story Title";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.fontSize = 32;
        titleText.color = Color.yellow;
        titleText.alignment = TextAnchor.MiddleCenter;

        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.8f);
        titleRect.anchorMax = new Vector2(1, 0.95f);
        titleRect.offsetMin = new Vector2(20, 0);
        titleRect.offsetMax = new Vector2(-20, 0);

        // Content text
        GameObject contentObj = new GameObject("StoryContentText");
        contentObj.transform.SetParent(panel.transform, false);
        Text contentText = contentObj.AddComponent<Text>();
        contentText.text = "Story content will appear here...";
        contentText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        contentText.fontSize = 16;
        contentText.color = Color.white;

        RectTransform contentRect = contentObj.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 0.3f);
        contentRect.anchorMax = new Vector2(1, 0.75f);
        contentRect.offsetMin = new Vector2(20, 0);
        contentRect.offsetMax = new Vector2(-20, 0);

        // Objectives text
        GameObject objectivesObj = new GameObject("ObjectivesText");
        objectivesObj.transform.SetParent(panel.transform, false);
        Text objectivesText = objectivesObj.AddComponent<Text>();
        objectivesText.text = "Objectives:\n- Objective 1\n- Objective 2";
        objectivesText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        objectivesText.fontSize = 14;
        objectivesText.color = Color.green;

        RectTransform objectivesRect = objectivesObj.GetComponent<RectTransform>();
        objectivesRect.anchorMin = new Vector2(0, 0.05f);
        objectivesRect.anchorMax = new Vector2(1, 0.25f);
        objectivesRect.offsetMin = new Vector2(20, 0);
        objectivesRect.offsetMax = new Vector2(-20, 0);

        // Assign to StorySystem
        StorySystem ss = FindObjectOfType<StorySystem>();
        if (ss != null)
        {
            ss.storyPanel = panel;
            ss.storyTitleText = titleText;
            ss.storyContentText = contentText;
            ss.objectivesText = objectivesText;
        }
    }

    void CreateEnvironment()
    {
        // Create ground plane
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = new Vector3(0, -0.5f, 0);
        ground.transform.localScale = new Vector3(10, 1, 10);

        // Create some basic environment objects
        for (int i = 0; i < 5; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = $"EnvironmentObject_{i}";
            cube.transform.position = new Vector3(
                Random.Range(-10f, 10f),
                0.5f,
                Random.Range(-10f, 10f)
            );
            cube.transform.localScale = new Vector3(
                Random.Range(0.5f, 2f),
                Random.Range(0.5f, 2f),
                Random.Range(0.5f, 2f)
            );
        }

        // Create a character to interact with
        GameObject character = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        character.name = "Lyra";
        character.transform.position = new Vector3(3, 0, 3);
        character.transform.localScale = new Vector3(0.8f, 1.5f, 0.8f);

        // Add a simple label
        GameObject labelObj = new GameObject("CharacterLabel");
        labelObj.transform.SetParent(character.transform, false);
        labelObj.transform.localPosition = new Vector3(0, 2, 0);

        TextMesh label = labelObj.AddComponent<TextMesh>();
        label.text = "Lyra Vale\n(Press E to talk)";
        label.fontSize = 24;
        label.color = Color.yellow;
        label.anchor = TextAnchor.MiddleCenter;

        Debug.Log("Basic environment created with ground, objects, and interactable character.");
    }
}