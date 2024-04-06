using System.Collections.Generic;
using UnityEngine;

public class SFXVolumeAdjuster : MonoBehaviour
{
    private AudioSource audioSource;
    public static List<SFXVolumeAdjuster> AllSFXSources = new List<SFXVolumeAdjuster>();

    void OnEnable()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        AllSFXSources.Add(this);
        AdjustVolume(SoundManager.Instance.GetSFXVolume()); // Initial adjustment based on current settings
    }

    void OnDisable()
    {
        AllSFXSources.Remove(this);
    }

    public void AdjustVolume(float volume)
    {
        if (audioSource != null) audioSource.volume = volume;
    }
}
