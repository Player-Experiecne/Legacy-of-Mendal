using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private AudioData audioData;

    private float masterVolume = 1f;
    private float musicVolume = 0.5f;
    private float sfxVolume = 0.5f;

    void Awake()
    {
        SetupSingleton();
    }
    private void Start()
    {
        RegisterAudioOnOccasions();
        GameEvents.TriggerTitleScreen();
    }

    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(MusicTrack track, bool loop = true)
    {
        AudioClip clip = audioData.GetMusicClip(track);
        if (clip != null)
        {
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = musicVolume * masterVolume;
            musicSource.Play();
        }
    }

    public void PlaySFX(SoundEffect effect)
    {
        AudioClip clip = audioData.GetSFXClip(effect);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        // No need to update volumes here since SFX volume is applied when played
    }

    private void UpdateVolumes()
    {
        musicSource.volume = musicVolume * masterVolume; // Update music volume
        // Note: As mentioned, adjusting the volume of currently playing SFX is not straightforward due to PlayOneShot. Future SFX will use the new volume.
    }

    private void RegisterAudioOnOccasions()
    {
        GameEvents.OnTitleScreen += () => PlayMusic(MusicTrack.TitleScreen);
        GameEvents.OnLevelStart += () => PlayMusic(MusicTrack.StartLevel);
        GameEvents.OnBreedingStart += () => PlayMusic(MusicTrack.StartBreeding);
        GameEvents.OnBreedingComplete += () => PlayMusic(MusicTrack.PrepareBattle);
        GameEvents.OnLevelComplete += () => PlayMusic(MusicTrack.Victory);
        GameEvents.OnLevelFail += () => PlayMusic(MusicTrack.Defeat);
    }
}


