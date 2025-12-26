using UnityEngine;
using UnityEngine.UI;

public class PlayableSceneSetup : MonoBehaviour
{
    [Header("Auto-Setup Options")]
    public bool createBasicScene = true;
    public bool createTestCharacters = true;
    public bool createBasicUI = true;
    public bool createEnvironment = true;

    void Start()
    {
        if (createBasicScene)
        {
            SetupPlayableScene();
        }
    }

    void SetupPlayableScene()
    {
        Debug.Log("🎮 Setting up 'The 5 Fold Awakening' playable scene...");

        // 1. Setup Camera
        SetupCamera();

        // 2. Setup Player
        SetupPlayer();

        // 3. Setup Game Controller
        SetupGameController();

        // 4. Setup Characters
        if (createTestCharacters)
        {
            SetupCharacters();
        }

        // 5. Setup UI
        if (createBasicUI)
        {
            SetupUI();
        }

        // 6. Setup Environment
        if (createEnvironment)
        {
            SetupEnvironment();
        }

        Debug.Log("✅ Scene setup complete! Press Play to start the game.");
        Debug.Log("🎯 Controls: WASD to move, Mouse to look, E to interact, Tab for story, Esc to pause");
    }

    void SetupCamera()
    {
        GameObject camera = new GameObject("Main Camera");
        Camera cam = camera.AddComponent<Camera>();
        camera.AddComponent<AudioListener>();
        camera.tag = "MainCamera";
        camera.transform.position = new Vector3(0, 5, -10);
        camera.transform.LookAt(Vector3.zero);

        Debug.Log("📷 Camera setup complete");
    }

    void SetupPlayer()
    {
        GameObject player = new GameObject("Player");
        player.AddComponent<CharacterController>();
        player.transform.position = Vector3.zero;

        // Add a visible capsule
        MeshFilter meshFilter = player.AddComponent<MeshFilter>();
        meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Capsule.fbx");
        MeshRenderer renderer = player.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = Color.blue;

        Debug.Log("👤 Player setup complete");
    }

    void SetupGameController()
    {
        GameObject controller = new GameObject("GameController");
        The5FoldAwakening game = controller.AddComponent<The5FoldAwakening>();

        // Configure basic settings
        game.mouseSensitivity = 1f;
        game.enableAudio = true;
        game.masterVolume = 1f;
        game.musicVolume = 0.7f;
        game.sfxVolume = 0.8f;
        game.voiceVolume = 1f;

        Debug.Log("🎮 Game controller setup complete");
    }

    void SetupCharacters()
    {
        // Create Lyra
        GameObject lyra = CreateCharacter("Lyra", new Vector3(3, 0, 3), Color.magenta);
        lyra.AddComponent<CharacterLabel>();

        // Create Eldrin
        GameObject eldrin = CreateCharacter("Eldrin", new Vector3(-3, 0, 3), Color.green);
        eldrin.AddComponent<CharacterLabel>();

        Debug.Log("👥 Characters setup complete");
    }

    GameObject CreateCharacter(string name, Vector3 position, Color color)
    {
        GameObject character = new GameObject(name);

        // Add mesh
        MeshFilter meshFilter = character.AddComponent<MeshFilter>();
        meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Cylinder.fbx");
        MeshRenderer renderer = character.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = color;

        character.transform.position = position;
        character.transform.localScale = new Vector3(0.8f, 1.5f, 0.8f);

        return character;
    }

    void SetupUI()
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

        // Create Main Menu
        CreateMainMenu(canvasObj);

        // Create Dialogue Panel
        CreateDialoguePanel(canvasObj);

        // Create Story Panel
        CreateStoryPanel(canvasObj);

        Debug.Log("🖥️ UI setup complete");
    }

    void CreateMainMenu(GameObject canvas)
    {
        GameObject menu = new GameObject("MainMenu");
        menu.transform.SetParent(canvas.transform, false);

        // Background
        Image bg = menu.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.8f);

        RectTransform rect = menu.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Title
        GameObject titleObj = CreateTextObject("Title", menu.transform, "THE 5 FOLD AWAKENING", 48, TextAnchor.MiddleCenter);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.7f);
        titleRect.anchorMax = new Vector2(0.5f, 0.7f);
        titleRect.sizeDelta = new Vector2(600, 60);

        // Start Button
        GameObject buttonObj = CreateButton("StartButton", menu.transform, "START GAME");
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.4f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.4f);
        buttonRect.sizeDelta = new Vector2(200, 50);

        Button button = buttonObj.GetComponent<Button>();
        The5FoldAwakening game = FindObjectOfType<The5FoldAwakening>();
        if (game != null)
        {
            button.onClick.AddListener(() => game.StartGame());
            game.mainMenuPanel = menu;
        }
    }

    void CreateDialoguePanel(GameObject canvas)
    {
        GameObject panel = new GameObject("DialoguePanel");
        panel.transform.SetParent(canvas.transform, false);
        panel.SetActive(false);

        Image bg = panel.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.9f);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.1f, 0.1f);
        rect.anchorMax = new Vector2(0.9f, 0.4f);

        // Character Name
        GameObject nameObj = CreateTextObject("CharacterName", panel.transform, "Character Name", 24, TextAnchor.MiddleLeft);
        nameObj.GetComponent<Text>().color = Color.yellow;
        RectTransform nameRect = nameObj.GetComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0, 1);
        nameRect.anchorMax = new Vector2(1, 1);
        nameRect.offsetMin = new Vector2(20, -50);
        nameRect.offsetMax = new Vector2(-20, -10);

        // Dialogue Text
        GameObject dialogueObj = CreateTextObject("DialogueText", panel.transform, "Dialogue will appear here...", 18, TextAnchor.UpperLeft);
        RectTransform dialogueRect = dialogueObj.GetComponent<RectTransform>();
        dialogueRect.anchorMin = new Vector2(0, 0.3f);
        dialogueRect.anchorMax = new Vector2(1, 0.9f);
        dialogueRect.offsetMin = new Vector2(20, 0);
        dialogueRect.offsetMax = new Vector2(-20, 0);

        // Next Button
        GameObject buttonObj = CreateButton("NextButton", panel.transform, "NEXT");
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.8f, 0.1f);
        buttonRect.anchorMax = new Vector2(0.95f, 0.25f);

        The5FoldAwakening game = FindObjectOfType<The5FoldAwakening>();
        if (game != null)
        {
            game.dialoguePanel = panel;
            game.dialogueText = dialogueObj.GetComponent<Text>();
            game.characterNameText = nameObj.GetComponent<Text>();
            game.nextDialogueButton = buttonObj.GetComponent<Button>();
        }
    }

    void CreateStoryPanel(GameObject canvas)
    {
        GameObject panel = new GameObject("StoryPanel");
        panel.transform.SetParent(canvas.transform, false);
        panel.SetActive(false);

        Image bg = panel.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.9f);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.1f, 0.1f);
        rect.anchorMax = new Vector2(0.9f, 0.9f);

        // Title
        GameObject titleObj = CreateTextObject("StoryTitle", panel.transform, "Story Title", 32, TextAnchor.MiddleCenter);
        titleObj.GetComponent<Text>().color = Color.yellow;
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.8f);
        titleRect.anchorMax = new Vector2(1, 0.95f);
        titleRect.offsetMin = new Vector2(20, 0);
        titleRect.offsetMax = new Vector2(-20, 0);

        // Content
        GameObject contentObj = CreateTextObject("StoryContent", panel.transform, "Story content will appear here...", 16, TextAnchor.UpperLeft);
        RectTransform contentRect = contentObj.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 0.3f);
        contentRect.anchorMax = new Vector2(1, 0.75f);
        contentRect.offsetMin = new Vector2(20, 0);
        contentRect.offsetMax = new Vector2(-20, 0);

        // Objectives
        GameObject objectivesObj = CreateTextObject("ObjectivesText", panel.transform, "Objectives:\n- Objective 1\n- Objective 2", 14, TextAnchor.UpperLeft);
        objectivesObj.GetComponent<Text>().color = Color.green;
        RectTransform objectivesRect = objectivesObj.GetComponent<RectTransform>();
        objectivesRect.anchorMin = new Vector2(0, 0.05f);
        objectivesRect.anchorMax = new Vector2(1, 0.25f);
        objectivesRect.offsetMin = new Vector2(20, 0);
        objectivesRect.offsetMax = new Vector2(-20, 0);

        The5FoldAwakening game = FindObjectOfType<The5FoldAwakening>();
        if (game != null)
        {
            game.storyPanel = panel;
            game.storyTitleText = titleObj.GetComponent<Text>();
            game.storyContentText = contentObj.GetComponent<Text>();
            game.objectivesText = objectivesObj.GetComponent<Text>();
        }
    }

    GameObject CreateTextObject(string name, Transform parent, string text, int fontSize, TextAnchor alignment)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);

        Text textComponent = obj.AddComponent<Text>();
        textComponent.text = text;
        textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComponent.fontSize = fontSize;
        textComponent.alignment = alignment;
        textComponent.color = Color.white;

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        return obj;
    }

    GameObject CreateButton(string name, Transform parent, string buttonText)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);

        Image image = obj.AddComponent<Image>();
        image.color = Color.gray;

        Button button = obj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.highlightedColor = Color.white;
        colors.pressedColor = Color.gray;
        button.colors = colors;

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(obj.transform, false);

        Text text = textObj.AddComponent<Text>();
        text.text = buttonText;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 16;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return obj;
    }

    void SetupEnvironment()
    {
        // Ground
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = new Vector3(0, -0.5f, 0);
        ground.transform.localScale = new Vector3(10, 1, 10);

        // Some environment objects
        for (int i = 0; i < 8; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = $"Environment_{i}";
            cube.transform.position = new Vector3(
                Random.Range(-8f, 8f),
                Random.Range(0.5f, 3f),
                Random.Range(-8f, 8f)
            );
            cube.transform.localScale = new Vector3(
                Random.Range(0.5f, 2f),
                Random.Range(0.5f, 2f),
                Random.Range(0.5f, 2f)
            );

            // Random colors
            MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }

        Debug.Log("🏞️ Environment setup complete");
    }
}

// Helper component for character labels
public class CharacterLabel : MonoBehaviour
{
    void Start()
    {
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(transform, false);
        labelObj.transform.localPosition = new Vector3(0, 2.5f, 0);

        TextMesh label = labelObj.AddComponent<TextMesh>();
        label.text = $"{gameObject.name}\n(Press E to talk)";
        label.fontSize = 32;
        label.color = Color.yellow;
        label.anchor = TextAnchor.MiddleCenter;
        label.alignment = TextAlignment.Center;
    }
}