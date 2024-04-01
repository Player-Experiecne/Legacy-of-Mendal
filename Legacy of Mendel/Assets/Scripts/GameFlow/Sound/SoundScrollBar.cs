using UnityEngine;
using UnityEngine.UI; // Necessary for UI components

public class SoundScrollBar : MonoBehaviour
{
    private Scrollbar volumeScrollbar;

    public string volumeType;

    private void Start()
    {
        volumeScrollbar = GetComponent<Scrollbar>();
        volumeScrollbar.onValueChanged.AddListener(HandleVolumeChange);
        switch (volumeType)
        {
            case "Master":
                volumeScrollbar.value = SoundManager.Instance.GetMasterVolume();
                break;
            case "Music":
                volumeScrollbar.value = SoundManager.Instance.GetMusicVolume();
                break;
            case "SFX":
                volumeScrollbar.value = SoundManager.Instance.GetSFXVolume();
                break;
        }
    }

    private void HandleVolumeChange(float value)
    {
        // Update the music volume in the SoundManager
        switch (volumeType)
        {
            case "Master":
                SoundManager.Instance.SetMasterVolume(value);
                break;
            case "Music":
                SoundManager.Instance.SetMusicVolume(value);
                break;
            case "SFX":
                SoundManager.Instance.SetSFXVolume(value);
                break;
        }
    }
}
