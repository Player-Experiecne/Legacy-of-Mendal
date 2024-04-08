using UnityEngine;
using System.Collections.Generic;

public enum MusicTrack
{
    TitleScreen,
    StartLevel,
    StartBreeding,
    PrepareBattle,
    Victory,
    Defeat,
    FinalVictory
    // Add more tracks as needed
}

public enum SoundEffect
{
    SummonerHeal,
    SummonerSmite,
    SummonerShield,
    LootGene,
    LootCultureMedium,
    Typing
    // Add more effects as needed
}

[System.Serializable]
public class MusicClipEntry
{
    public MusicTrack track;
    public AudioClip clip;
}

[System.Serializable]
public class SFXClipEntry
{
    public SoundEffect effect;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "AudioData", menuName = "Audio/AudioData", order = 1)]
public class AudioData : ScriptableObject
{
    public List<MusicClipEntry> musicClips;
    public List<SFXClipEntry> sfxClips;

    public AudioClip GetMusicClip(MusicTrack track)
    {
        foreach (var musicClip in musicClips)
        {
            if (musicClip.track == track)
                return musicClip.clip;
        }
        return null;
    }

    public AudioClip GetSFXClip(SoundEffect effect)
    {
        foreach (var sfxClip in sfxClips)
        {
            if (sfxClip.effect == effect)
                return sfxClip.clip;
        }
        return null;
    }
}

