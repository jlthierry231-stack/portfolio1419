using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    public bool enableAudio = true;
    public bool enableParticles = true;
    public float mouseSensitivity = 1f;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject pausePanel;

    [Header("Settings UI")]
    public Toggle audioToggle;
    public Toggle particlesToggle;
    public Slider sensitivitySlider;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider voiceVolumeSlider;

    private bool isPaused = false;
    private bool gameStarted = false;
    private bool isInMission = false;

    private bool isPaused = false;
    private bool gameStarted = false;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeSettings();
        ShowMainMenu();
    }

    void InitializeSettings()
    {
        // Load saved settings or use defaults
        enableAudio = PlayerPrefs.GetInt("EnableAudio", 1) == 1;
        enableParticles = PlayerPrefs.GetInt("EnableParticles", 1) == 1;
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);

        // Load audio settings
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        float voiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 1f);

        // Apply settings to UI
        if (audioToggle != null) audioToggle.isOn = enableAudio;
        if (particlesToggle != null) particlesToggle.isOn = enableParticles;
        if (sensitivitySlider != null) sensitivitySlider.value = mouseSensitivity;

        // Apply audio settings to UI and AudioManager
        if (masterVolumeSlider != null) masterVolumeSlider.value = masterVolume;
        if (musicVolumeSlider != null) musicVolumeSlider.value = musicVolume;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = sfxVolume;
        if (voiceVolumeSlider != null) voiceVolumeSlider.value = voiceVolume;

        // Initialize AudioManager if it exists
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMasterVolume(masterVolume);
            AudioManager.instance.SetMusicVolume(musicVolume);
            AudioManager.instance.SetSFXVolume(sfxVolume);
            AudioManager.instance.SetVoiceVolume(voiceVolume);
        }

        // Add listeners
        if (audioToggle != null) audioToggle.onValueChanged.AddListener(OnAudioToggleChanged);
        if (particlesToggle != null) particlesToggle.onValueChanged.AddListener(OnParticlesToggleChanged);
        if (sensitivitySlider != null) sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        // Add audio listeners
        if (masterVolumeSlider != null) masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        if (musicVolumeSlider != null) musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (sfxVolumeSlider != null) sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        if (voiceVolumeSlider != null) voiceVolumeSlider.onValueChanged.AddListener(OnVoiceVolumeChanged);
    }

    void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

        // Transition to peaceful exploration music
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayPeacefulMusic();
        }
    }

    public void ShowSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(false);
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

    public void PauseGame()
    {
        if (!gameStarted) return;

        isPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause audio
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PauseAllAudio();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Resume audio
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ResumeAllAudio();
        }
    }

    public void QuitGame()
    {
        // Save settings before quitting
        SaveSettings();
        Application.Quit();
    }

    void Update()
    {
        // Handle pause input
        if (Input.GetKeyDown(KeyCode.Escape) && gameStarted)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void OnAudioToggleChanged(bool value)
    {
        enableAudio = value;
        PlayerPrefs.SetInt("EnableAudio", value ? 1 : 0);
    }

    void OnParticlesToggleChanged(bool value)
    {
        enableParticles = value;
        PlayerPrefs.SetInt("EnableParticles", value ? 1 : 0);
    }

    void OnSensitivityChanged(float value)
    {
        mouseSensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", value);
    }

    void OnMasterVolumeChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMasterVolume(value);
        }
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicVolume(value);
        }
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetSFXVolume(value);
        }
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void OnVoiceVolumeChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVoiceVolume(value);
        }
        PlayerPrefs.SetFloat("VoiceVolume", value);
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("EnableAudio", enableAudio ? 1 : 0);
        PlayerPrefs.SetInt("EnableParticles", enableParticles ? 1 : 0);
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);

        // Save audio settings
        if (AudioManager.instance != null)
        {
            PlayerPrefs.SetFloat("MasterVolume", AudioManager.instance.masterVolume);
            PlayerPrefs.SetFloat("MusicVolume", AudioManager.instance.musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", AudioManager.instance.sfxVolume);
            PlayerPrefs.SetFloat("VoiceVolume", AudioManager.instance.voiceVolume);
        }

        PlayerPrefs.Save();
    }

    public void SetMissionMode(bool inMission)
    {
        isInMission = inMission;

        if (AudioManager.instance != null)
        {
            if (inMission)
            {
                AudioManager.instance.PlayAdventureMusic();
            }
            else
            {
                AudioManager.instance.PlayPeacefulMusic();
            }
        }
    }

    public bool IsInMission()
    {
        return isInMission;
    }