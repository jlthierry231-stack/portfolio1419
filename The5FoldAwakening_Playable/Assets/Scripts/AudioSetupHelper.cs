using UnityEngine;

public class AudioSetupHelper : MonoBehaviour
{
    [Header("Audio Manager Reference")]
    public AudioManager audioManager;

    [Header("Test Audio Clips")]
    public AudioClip testAdventureMusic;
    public AudioClip testPeacefulMusic;
    public AudioClip testMenuMusic;
    public AudioClip testCharacterVoice;
    public AudioClip testNarratorVoice;
    public AudioClip testInteractionSFX;
    public AudioClip testObjectiveSFX;
    public AudioClip testStoryAdvanceSFX;

    void Start()
    {
        SetupTestAudio();
    }

    void SetupTestAudio()
    {
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }

        if (audioManager != null)
        {
            // Assign test audio clips if they exist
            if (testAdventureMusic != null) audioManager.adventureMusic = testAdventureMusic;
            if (testPeacefulMusic != null) audioManager.peacefulMusic = testPeacefulMusic;
            if (testMenuMusic != null) audioManager.menuMusic = testMenuMusic;

            if (testCharacterVoice != null)
            {
                audioManager.characterVoices = new AudioClip[] { testCharacterVoice };
            }

            if (testNarratorVoice != null)
            {
                audioManager.narratorVoices = new AudioClip[] { testNarratorVoice };
            }

            if (testInteractionSFX != null) audioManager.interactionSound = testInteractionSFX;
            if (testObjectiveSFX != null) audioManager.objectiveCompleteSound = testObjectiveSFX;
            if (testStoryAdvanceSFX != null) audioManager.storyAdvanceSound = testStoryAdvanceSFX;

            Debug.Log("Audio setup complete. You can now test the audio system.");
        }
        else
        {
            Debug.LogError("AudioManager not found! Make sure AudioManager script is attached to a GameObject in the scene.");
        }
    }

    // Test methods that can be called from UI buttons or console
    public void TestAdventureMusic()
    {
        if (audioManager != null) audioManager.PlayAdventureMusic();
    }

    public void TestPeacefulMusic()
    {
        if (audioManager != null) audioManager.PlayPeacefulMusic();
    }

    public void TestMenuMusic()
    {
        if (audioManager != null) audioManager.PlayMenuMusic();
    }

    public void TestCharacterVoice()
    {
        if (audioManager != null) audioManager.PlayCharacterVoice(0, 0);
    }

    public void TestNarratorVoice()
    {
        if (audioManager != null) audioManager.PlayNarratorVoice(0);
    }

    public void TestInteractionSFX()
    {
        if (audioManager != null) audioManager.PlayInteractionSound();
    }

    public void TestObjectiveSFX()
    {
        if (audioManager != null) audioManager.PlayObjectiveCompleteSound();
    }

    public void TestStoryAdvanceSFX()
    {
        if (audioManager != null) audioManager.PlayStoryAdvanceSound();
    }
}