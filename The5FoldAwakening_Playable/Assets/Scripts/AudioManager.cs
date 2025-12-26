using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource voiceSource;
    public AudioSource narratorSource;

    [Header("Music Clips")]
    public AudioClip adventureMusic;
    public AudioClip peacefulMusic;
    public AudioClip menuMusic;

    [Header("Voice Clips")]
    public AudioClip[] characterVoices;
    public AudioClip[] narratorVoices;

    [Header("SFX Clips")]
    public AudioClip interactionSound;
    public AudioClip objectiveCompleteSound;
    public AudioClip storyAdvanceSound;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.7f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;
    [Range(0f, 1f)] public float voiceVolume = 1f;

    private AudioClip currentMusic;
    private Coroutine musicFadeCoroutine;

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

        InitializeAudioSources();
    }

    void InitializeAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = musicVolume;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.volume = sfxVolume;
        }

        if (voiceSource == null)
        {
            voiceSource = gameObject.AddComponent<AudioSource>();
            voiceSource.volume = voiceVolume;
        }

        if (narratorSource == null)
        {
            narratorSource = gameObject.AddComponent<AudioSource>();
            narratorSource.volume = voiceVolume;
        }
    }

    void Start()
    {
        PlayMenuMusic();
    }

    // Music Management
    public void PlayAdventureMusic()
    {
        if (adventureMusic != null && currentMusic != adventureMusic)
        {
            StartMusicTransition(adventureMusic);
        }
    }

    public void PlayPeacefulMusic()
    {
        if (peacefulMusic != null && currentMusic != peacefulMusic)
        {
            StartMusicTransition(peacefulMusic);
        }
    }

    public void PlayMenuMusic()
    {
        if (menuMusic != null && currentMusic != menuMusic)
        {
            StartMusicTransition(menuMusic);
        }
    }

    void StartMusicTransition(AudioClip newMusic)
    {
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }
        musicFadeCoroutine = StartCoroutine(FadeMusic(newMusic));
    }

    IEnumerator FadeMusic(AudioClip newMusic)
    {
        // Fade out current music
        float startVolume = musicSource.volume;
        float fadeTime = 2f;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        // Switch to new music
        musicSource.clip = newMusic;
        musicSource.Play();
        currentMusic = newMusic;

        // Fade in new music
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t / fadeTime);
            yield return null;
        }
    }

    // Voice Management
    public void PlayCharacterVoice(int characterIndex, int dialogueIndex)
    {
        if (characterVoices != null && characterIndex >= 0 && characterIndex < characterVoices.Length)
        {
            // For now, we'll use a simple mapping. In a real game, you'd have specific voice clips for each dialogue
            int voiceIndex = (characterIndex * 10 + dialogueIndex) % characterVoices.Length;
            if (voiceIndex < characterVoices.Length && characterVoices[voiceIndex] != null)
            {
                voiceSource.clip = characterVoices[voiceIndex];
                voiceSource.Play();
            }
        }
    }

    public void PlayNarratorVoice(int storyIndex)
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

    public void StopVoice()
    {
        voiceSource.Stop();
        narratorSource.Stop();
    }

    // SFX Management
    public void PlayInteractionSound()
    {
        if (interactionSound != null)
        {
            sfxSource.PlayOneShot(interactionSound);
        }
    }

    public void PlayObjectiveCompleteSound()
    {
        if (objectiveCompleteSound != null)
        {
            sfxSource.PlayOneShot(objectiveCompleteSound);
        }
    }

    public void PlayStoryAdvanceSound()
    {
        if (storyAdvanceSound != null)
        {
            sfxSource.PlayOneShot(storyAdvanceSound);
        }
    }

    // Volume Controls
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }

    public void SetVoiceVolume(float volume)
    {
        voiceVolume = Mathf.Clamp01(volume);
        voiceSource.volume = voiceVolume;
        narratorSource.volume = voiceVolume;
    }

    void UpdateAllVolumes()
    {
        musicSource.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
        voiceSource.volume = voiceVolume * masterVolume;
        narratorSource.volume = voiceVolume * masterVolume;
    }

    // Utility Methods
    public bool IsVoicePlaying()
    {
        return voiceSource.isPlaying || narratorSource.isPlaying;
    }

    public void PauseAllAudio()
    {
        musicSource.Pause();
        voiceSource.Pause();
        narratorSource.Pause();
    }

    public void ResumeAllAudio()
    {
        musicSource.UnPause();
        voiceSource.UnPause();
        narratorSource.UnPause();
    }
}