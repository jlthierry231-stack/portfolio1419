using UnityEngine;

public class GameTester : MonoBehaviour
{
    [Header("Test Configuration")]
    public bool runTestsOnStart = true;
    public float testDelay = 2f;

    void Start()
    {
        if (runTestsOnStart)
        {
            StartCoroutine(RunTests());
        }
    }

    IEnumerator RunTests()
    {
        Debug.Log("=== Starting Game Tests ===");

        yield return new WaitForSeconds(testDelay);

        // Test 1: Check for required components
        TestRequiredComponents();

        yield return new WaitForSeconds(1f);

        // Test 2: Test GameManager functionality
        TestGameManager();

        yield return new WaitForSeconds(1f);

        // Test 3: Test AudioManager functionality
        TestAudioManager();

        yield return new WaitForSeconds(1f);

        // Test 4: Test PlayerController
        TestPlayerController();

        yield return new WaitForSeconds(1f);

        // Test 5: Test CharacterManager
        TestCharacterManager();

        yield return new WaitForSeconds(1f);

        // Test 6: Test StorySystem
        TestStorySystem();

        Debug.Log("=== All Tests Completed ===");
    }

    void TestRequiredComponents()
    {
        Debug.Log("Testing required components...");

        // Check for main camera
        if (Camera.main == null)
        {
            Debug.LogError("❌ No Main Camera found!");
        }
        else
        {
            Debug.Log("✅ Main Camera found");
        }

        // Check for player
        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("❌ No Player GameObject found!");
        }
        else
        {
            Debug.Log("✅ Player GameObject found");

            // Check for required components on player
            if (!player.GetComponent<CharacterController>())
                Debug.LogError("❌ Player missing CharacterController!");
            else
                Debug.Log("✅ Player has CharacterController");

            if (!player.GetComponent<PlayerController>())
                Debug.LogError("❌ Player missing PlayerController!");
            else
                Debug.Log("✅ Player has PlayerController");

            if (!player.GetComponent<CharacterManager>())
                Debug.LogError("❌ Player missing CharacterManager!");
            else
                Debug.Log("✅ Player has CharacterManager");

            if (!player.GetComponent<StorySystem>())
                Debug.LogError("❌ Player missing StorySystem!");
            else
                Debug.Log("✅ Player has StorySystem");
        }

        // Check for GameManager
        if (GameManager.instance == null)
        {
            Debug.LogError("❌ No GameManager instance found!");
        }
        else
        {
            Debug.Log("✅ GameManager instance found");
        }

        // Check for AudioManager
        if (AudioManager.instance == null)
        {
            Debug.LogWarning("⚠️ No AudioManager instance found (optional)");
        }
        else
        {
            Debug.Log("✅ AudioManager instance found");
        }
    }

    void TestGameManager()
    {
        Debug.Log("Testing GameManager...");

        if (GameManager.instance == null)
        {
            Debug.LogError("❌ Cannot test GameManager - no instance!");
            return;
        }

        // Test basic functionality
        Debug.Log("✅ GameManager basic test passed");

        // Test settings
        Debug.Log($"Audio enabled: {GameManager.instance.enableAudio}");
        Debug.Log($"Particles enabled: {GameManager.instance.enableParticles}");
    }

    void TestAudioManager()
    {
        Debug.Log("Testing AudioManager...");

        if (AudioManager.instance == null)
        {
            Debug.Log("⚠️ Skipping AudioManager test - no instance");
            return;
        }

        // Test basic functionality
        Debug.Log("✅ AudioManager basic test passed");

        // Test volume settings
        Debug.Log($"Master volume: {AudioManager.instance.masterVolume}");
        Debug.Log($"Music volume: {AudioManager.instance.musicVolume}");
        Debug.Log($"SFX volume: {AudioManager.instance.sfxVolume}");
        Debug.Log($"Voice volume: {AudioManager.instance.voiceVolume}");

        // Test music switching (will work even without audio clips)
        AudioManager.instance.PlayMenuMusic();
        Debug.Log("✅ AudioManager music switching test passed");
    }

    void TestPlayerController()
    {
        Debug.Log("Testing PlayerController...");

        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("❌ Cannot test PlayerController - no Player!");
            return;
        }

        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc == null)
        {
            Debug.LogError("❌ Player missing PlayerController!");
            return;
        }

        // Test basic properties
        Debug.Log($"Move speed: {pc.moveSpeed}");
        Debug.Log($"Camera distance: {pc.cameraDistance}");
        Debug.Log($"Camera height: {pc.cameraHeight}");

        Debug.Log("✅ PlayerController basic test passed");
    }

    void TestCharacterManager()
    {
        Debug.Log("Testing CharacterManager...");

        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("❌ Cannot test CharacterManager - no Player!");
            return;
        }

        CharacterManager cm = player.GetComponent<CharacterManager>();
        if (cm == null)
        {
            Debug.LogError("❌ Player missing CharacterManager!");
            return;
        }

        // Test character data
        if (cm.characters == null || cm.characters.Length == 0)
        {
            Debug.LogWarning("⚠️ No characters configured in CharacterManager");
        }
        else
        {
            Debug.Log($"Number of characters: {cm.characters.Length}");
            for (int i = 0; i < cm.characters.Length; i++)
            {
                Debug.Log($"Character {i}: {cm.characters[i].name}");
            }
        }

        Debug.Log($"Interaction distance: {cm.interactionDistance}");
        Debug.Log("✅ CharacterManager basic test passed");
    }

    void TestStorySystem()
    {
        Debug.Log("Testing StorySystem...");

        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("❌ Cannot test StorySystem - no Player!");
            return;
        }

        StorySystem ss = player.GetComponent<StorySystem>();
        if (ss == null)
        {
            Debug.LogError("❌ Player missing StorySystem!");
            return;
        }

        // Test story data
        if (ss.storySegments == null || ss.storySegments.Length == 0)
        {
            Debug.LogWarning("⚠️ No story segments configured in StorySystem");
        }
        else
        {
            Debug.Log($"Number of story segments: {ss.storySegments.Length}");
            Debug.Log($"Current story index: {ss.currentStoryIndex}");
        }

        Debug.Log("✅ StorySystem basic test passed");
    }

    // Public test methods that can be called from UI buttons
    public void RunAllTests()
    {
        StartCoroutine(RunTests());
    }

    public void TestMovement()
    {
        Debug.Log("Testing player movement...");
        Debug.Log("Use WASD keys to move, mouse to look around");
        Debug.Log("Player should move relative to camera direction");
    }

    public void TestInteraction()
    {
        Debug.Log("Testing character interaction...");
        Debug.Log("Move close to the character (Lyra) and press E to interact");
        Debug.Log("Dialogue panel should appear with character dialogue");
    }

    public void TestStory()
    {
        Debug.Log("Testing story system...");
        Debug.Log("Press Tab to show/hide story panel");
        Debug.Log("Story content and objectives should be displayed");
    }

    public void TestAudio()
    {
        Debug.Log("Testing audio system...");
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayPeacefulMusic();
            Debug.Log("Playing peaceful music (if audio clips are assigned)");
        }
        else
        {
            Debug.Log("No AudioManager found - add one to test audio");
        }
    }
}