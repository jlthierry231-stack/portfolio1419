using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class The5FoldAwakening : MonoBehaviour
{
    // ===== SINGLETON PATTERN =====
    public static The5FoldAwakening instance;

    // ===== GAME STATE =====
    [Header("Game State")]
    public bool gameStarted = false;
    public bool isPaused = false;
    public bool isInMission = false;

    // ===== PLAYER SETTINGS =====
    [Header("Player Settings")]
    public float mouseSensitivity = 1f;
    public bool invertY = false;
    public bool enableAudio = true;
    public bool enableShadows = true;
    public float masterVolume = 1f;
    public float musicVolume = 0.7f;
    public float sfxVolume = 0.8f;
    public float voiceVolume = 1f;

    // ===== UI REFERENCES =====
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject pausePanel;
    public GameObject dialoguePanel;
    public GameObject storyPanel;

    [Header("UI Elements")]
    public Text dialogueText;
    public Text characterNameText;
    public Button nextDialogueButton;
    public Text storyTitleText;
    public Text storyContentText;
    public Text objectivesText;

    [Header("Settings UI")]
    public Toggle audioToggle;
    public Toggle shadowsToggle;
    public Toggle invertYToggle;
    public Slider sensitivitySlider;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider voiceVolumeSlider;

    // ===== AUDIO SYSTEM =====
    [Header("Audio Clips")]
    public AudioClip adventureMusic;
    public AudioClip peacefulMusic;
    public AudioClip menuMusic;
    public AudioClip[] characterVoices;
    public AudioClip[] narratorVoices;
    public AudioClip interactionSound;
    public AudioClip objectiveCompleteSound;
    public AudioClip storyAdvanceSound;

    // ===== PRIVATE VARIABLES =====
    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource voiceSource;
    private AudioSource narratorSource;
    private AudioClip currentMusic;
    private Coroutine musicFadeCoroutine;

    // ===== STORY DATA =====
    [System.Serializable]
    public class StorySegment
    {
        public string title;
        public string content;
        public string[] objectives;
        public bool isCompleted = false;
    }

    [System.Serializable]
    public class Character
    {
        public string name;
        public GameObject characterObject;
        public string[] dialogues;
        public bool isInteractable = true;
    }

    [Header("Game Data")]
    public StorySegment[] storySegments;
    public Character[] characters;
    public int currentStoryIndex = 0;
    private int currentCharacterIndex = -1;
    private int currentDialogueIndex = 0;
    private bool isInDialogue = false;

    // ===== PLAYER COMPONENTS =====
    private CharacterController playerController;
    private Transform playerTransform;
    private Transform cameraTransform;
    private float moveSpeed = 5f;
    private float rotationSpeed = 10f;
    private float cameraDistance = 5f;
    private float cameraHeight = 2f;
    private float cameraSmoothSpeed = 5f;
    private Vector3 moveDirection;
    private float verticalVelocity;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeAudio();
        LoadSettings();
    }

    void Start()
    {
        InitializePlayer();
        InitializeUI();
        SetupStoryData();
        SetupCharacterData();
        ShowMainMenu();
    }

    void Update()
    {
        if (gameStarted && !isPaused)
        {
            HandlePlayerInput();
            HandlePlayerMovement();
            HandleCamera();
        }

        HandleUIInput();
    }

    // ===== INITIALIZATION =====
    void InitializeAudio()
    {
        // Create audio sources
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = sfxVolume;

        voiceSource = gameObject.AddComponent<AudioSource>();
        voiceSource.volume = voiceVolume;

        narratorSource = gameObject.AddComponent<AudioSource>();
        narratorSource.volume = voiceVolume;
    }

    void InitializePlayer()
    {
        // Find or create player
        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            player = new GameObject("Player");
            player.AddComponent<CharacterController>();
        }

        playerController = player.GetComponent<CharacterController>();
        playerTransform = player.transform;

        // Setup camera
        GameObject cam = GameObject.Find("Main Camera");
        if (cam == null)
        {
            cam = new GameObject("Main Camera");
            cam.AddComponent<Camera>();
            cam.AddComponent<AudioListener>();
            cam.tag = "MainCamera";
        }
        cameraTransform = cam.transform;
    }

    void InitializeUI()
    {
        // Create Canvas if it doesn't exist
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Create Event System
        if (GameObject.Find("EventSystem") == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    void SetupStoryData()
    {
        storySegments = new StorySegment[]
        {
            new StorySegment
            {
                title = "The Awakening",
                content = "You are Kael Ardin, a young mage in a world where magic and technology coexist. Strange powers have begun to manifest within you, and you must learn to control them before they control you.",
                objectives = new string[]
                {
                    "Explore the village",
                    "Find the ancient tome",
                    "Speak with the village elder"
                }
            },
            new StorySegment
            {
                title = "The First Power",
                content = "Your magical abilities are growing stronger. The ancient tome reveals that you possess the rare gift of technomancy - the ability to merge magic with technology.",
                objectives = new string[]
                {
                    "Practice your technomancy",
                    "Help the villagers with your powers",
                    "Discover the source of your abilities"
                }
            }
        };
    }

    void SetupCharacterData()
    {
        characters = new Character[]
        {
            new Character
            {
                name = "Lyra Vale",
                dialogues = new string[]
                {
                    "Welcome to the world of magic and technology, Kael. Your journey begins now.",
                    "Remember, the balance between magic and machines is delicate. Choose wisely.",
                    "The ancient powers are awakening. You must be ready."
                },
                isInteractable = true
            },
            new Character
            {
                name = "Eldrin",
                dialogues = new string[]
                {
                    "Ah, young technomancer. Your powers are both a gift and a burden.",
                    "The ancient machines slumber beneath the earth, waiting for one worthy to awaken them.",
                    "Balance is key. Magic without technology is chaos. Technology without magic is emptiness."
                },
                isInteractable = true
            }
        };
    }

    // ===== SETTINGS MANAGEMENT =====
    void LoadSettings()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
        invertY = PlayerPrefs.GetInt("InvertY", 0) == 1;
        enableAudio = PlayerPrefs.GetInt("EnableAudio", 1) == 1;
        enableShadows = PlayerPrefs.GetInt("EnableShadows", 1) == 1;
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        voiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 1f);
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.SetInt("InvertY", invertY ? 1 : 0);
        PlayerPrefs.SetInt("EnableAudio", enableAudio ? 1 : 0);
        PlayerPrefs.SetInt("EnableShadows", enableShadows ? 1 : 0);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("VoiceVolume", voiceVolume);
        PlayerPrefs.Save();
    }

    // ===== MENU SYSTEM =====
    public void ShowMainMenu()
    {
        gameStarted = false;
        isPaused = false;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (storyPanel != null) storyPanel.SetActive(false);

        PlayMenuMusic();
    }

    public void StartGame()
    {
        gameStarted = true;
        isPaused = false;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayPeacefulMusic();
        ShowStory(currentStoryIndex);
    }

    public void PauseGame()
    {
        if (!gameStarted) return;

        isPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PauseAllAudio();
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ResumeAllAudio();
    }

    public void ShowSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void HideSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (gameStarted)
        {
            if (pausePanel != null) pausePanel.SetActive(true);
        }
        else
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        }
    }

    // ===== PLAYER MOVEMENT =====
    void HandlePlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleStoryPanel();
        }

        if (Input.GetKeyDown(KeyCode.E) && !isInDialogue)
        {
            TryInteract();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isInDialogue)
        {
            EndDialogue();
        }
    }

    void HandlePlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * vertical + right * horizontal).normalized;

        if (playerController.isGrounded)
        {
            verticalVelocity = -0.1f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        Vector3 movement = moveDirection * moveSpeed + Vector3.up * verticalVelocity;
        playerController.Move(movement * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleCamera()
    {
        Vector3 desiredPosition = playerTransform.position - playerTransform.forward * cameraDistance + Vector3.up * cameraHeight;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime);
        cameraTransform.LookAt(playerTransform.position + Vector3.up * cameraHeight * 0.5f);
    }

    // ===== INTERACTION SYSTEM =====
    void TryInteract()
    {
        // Simple interaction - check for nearby objects
        // In a full game, you'd use raycasting or trigger colliders
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].characterObject != null && characters[i].isInteractable)
            {
                float distance = Vector3.Distance(playerTransform.position, characters[i].characterObject.transform.position);
                if (distance <= 3f) // interaction distance
                {
                    StartDialogue(i);
                    break;
                }
            }
        }
    }

    void StartDialogue(int characterIndex)
    {
        currentCharacterIndex = characterIndex;
        currentDialogueIndex = 0;
        isInDialogue = true;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        PlayInteractionSound();
        ShowCurrentDialogue();
    }

    void ShowCurrentDialogue()
    {
        if (currentCharacterIndex >= 0 && currentCharacterIndex < characters.Length)
        {
            Character currentChar = characters[currentCharacterIndex];

            if (characterNameText != null) characterNameText.text = currentChar.name;

            if (currentDialogueIndex < currentChar.dialogues.Length)
            {
                if (dialogueText != null) dialogueText.text = currentChar.dialogues[currentDialogueIndex];
                PlayCharacterVoice(currentCharacterIndex, currentDialogueIndex);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void NextDialogue()
    {
        currentDialogueIndex++;
        ShowCurrentDialogue();
    }

    void EndDialogue()
    {
        isInDialogue = false;
        currentCharacterIndex = -1;
        currentDialogueIndex = 0;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    // ===== STORY SYSTEM =====
    void ShowStory(int index)
    {
        if (index >= 0 && index < storySegments.Length)
        {
            StorySegment segment = storySegments[index];
            if (storyTitleText != null) storyTitleText.text = segment.title;
            if (storyContentText != null) storyContentText.text = segment.content;
            UpdateObjectivesDisplay();
            if (storyPanel != null) storyPanel.SetActive(true);
            PlayNarratorVoice(index);
        }
    }

    void UpdateObjectivesDisplay()
    {
        if (objectivesText != null && storySegments.Length > currentStoryIndex)
        {
            StorySegment currentSegment = storySegments[currentStoryIndex];
            string objectivesStr = "Objectives:\n";
            for (int i = 0; i < currentSegment.objectives.Length; i++)
            {
                string status = currentSegment.isCompleted ? "[✓]" : "[ ]";
                objectivesStr += status + " " + currentSegment.objectives[i] + "\n";
            }
            objectivesText.text = objectivesStr;
        }
    }

    void ToggleStoryPanel()
    {
        if (storyPanel != null)
        {
            bool isActive = storyPanel.activeSelf;
            storyPanel.SetActive(!isActive);
        }
    }

    public void CompleteObjective(int objectiveIndex)
    {
        if (currentStoryIndex < storySegments.Length)
        {
            storySegments[currentStoryIndex].isCompleted = true;
            UpdateObjectivesDisplay();
            PlayObjectiveCompleteSound();
            CheckStoryProgression();
        }
    }

    void CheckStoryProgression()
    {
        if (storySegments[currentStoryIndex].isCompleted && currentStoryIndex < storySegments.Length - 1)
        {
            currentStoryIndex++;
            PlayStoryAdvanceSound();
            ShowStory(currentStoryIndex);
        }
    }

    // ===== AUDIO SYSTEM =====
    void PlayMenuMusic()
    {
        if (menuMusic != null && currentMusic != menuMusic)
        {
            StartMusicTransition(menuMusic);
        }
    }

    void PlayPeacefulMusic()
    {
        if (peacefulMusic != null && currentMusic != peacefulMusic)
        {
            StartMusicTransition(peacefulMusic);
        }
    }

    void PlayAdventureMusic()
    {
        if (adventureMusic != null && currentMusic != adventureMusic)
        {
            StartMusicTransition(adventureMusic);
        }
    }

    void StartMusicTransition(AudioClip newMusic)
    {
        if (musicFadeCoroutine != null) StopCoroutine(musicFadeCoroutine);
        musicFadeCoroutine = StartCoroutine(FadeMusic(newMusic));
    }

    IEnumerator FadeMusic(AudioClip newMusic)
    {
        float startVolume = musicSource.volume;
        float fadeTime = 2f;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        musicSource.clip = newMusic;
        musicSource.Play();
        currentMusic = newMusic;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, musicVolume * masterVolume, t / fadeTime);
            yield return null;
        }
    }

    void PlayCharacterVoice(int characterIndex, int dialogueIndex)
    {
        if (characterVoices != null && characterIndex >= 0 && characterIndex < characterVoices.Length)
        {
            int voiceIndex = (characterIndex * 10 + dialogueIndex) % characterVoices.Length;
            if (voiceIndex < characterVoices.Length && characterVoices[voiceIndex] != null)
            {
                voiceSource.clip = characterVoices[voiceIndex];
                voiceSource.Play();
            }
        }
    }

    void PlayNarratorVoice(int storyIndex)
    {
        if (narratorVoices != null && storyIndex >= 0 && storyIndex < narratorVoices.Length)
        {
            if (narratorVoices[storyIndex] != null)
            {
                narratorSource.clip = narratorVoices[storyIndex];
                narratorSource.Play();
            }
        }
    }

    void PlayInteractionSound()
    {
        if (interactionSound != null)
        {
            sfxSource.PlayOneShot(interactionSound);
        }
    }

    void PlayObjectiveCompleteSound()
    {
        if (objectiveCompleteSound != null)
        {
            sfxSource.PlayOneShot(objectiveCompleteSound);
        }
    }

    void PlayStoryAdvanceSound()
    {
        if (storyAdvanceSound != null)
        {
            sfxSource.PlayOneShot(storyAdvanceSound);
        }
    }

    void PauseAllAudio()
    {
        musicSource.Pause();
        voiceSource.Pause();
        narratorSource.Pause();
    }

    void ResumeAllAudio()
    {
        musicSource.UnPause();
        voiceSource.UnPause();
        narratorSource.UnPause();
    }

    // ===== UI INPUT HANDLING =====
    void HandleUIInput()
    {
        // Settings changes
        if (sensitivitySlider != null) mouseSensitivity = sensitivitySlider.value;
        if (masterVolumeSlider != null) SetMasterVolume(masterVolumeSlider.value);
        if (musicVolumeSlider != null) SetMusicVolume(musicVolumeSlider.value);
        if (sfxVolumeSlider != null) SetSFXVolume(sfxVolumeSlider.value);
        if (voiceVolumeSlider != null) SetVoiceVolume(voiceVolumeSlider.value);
        if (audioToggle != null) enableAudio = audioToggle.isOn;
        if (shadowsToggle != null) enableShadows = shadowsToggle.isOn;
        if (invertYToggle != null) invertY = invertYToggle.isOn;
    }

    // ===== VOLUME CONTROLS =====
    void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume * masterVolume;
    }

    void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume * masterVolume;
    }

    void SetVoiceVolume(float volume)
    {
        voiceVolume = Mathf.Clamp01(volume);
        voiceSource.volume = voiceVolume * masterVolume;
        narratorSource.volume = voiceVolume * masterVolume;
    }

    void UpdateAllVolumes()
    {
        musicSource.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
        voiceSource.volume = voiceVolume * masterVolume;
        narratorSource.volume = voiceVolume * masterVolume;
    }

    // ===== UTILITY METHODS =====
    public void QuitGame()
    {
        SaveSettings();
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        SaveSettings();
    }

    // ===== DEBUG METHODS =====
    public void DebugLog(string message)
    {
        Debug.Log("[The5FoldAwakening] " + message);
    }

    public void TestAllSystems()
    {
        DebugLog("Testing all game systems...");

        // Test audio
        PlayPeacefulMusic();
        DebugLog("Audio system: OK");

        // Test story
        ShowStory(0);
        DebugLog("Story system: OK");

        // Test character interaction
        if (characters.Length > 0)
        {
            StartDialogue(0);
            DebugLog("Dialogue system: OK");
        }

        DebugLog("All systems test completed!");
    }
}