using UnityEngine;
using UnityEngine.UI; // Necessary for UI components

public class SoundScrollBar : MonoBehaviour
{
    private Scrollbar musicVolumeScrollbar;

    private void Start()
    {
        musicVolumeScrollbar = GetComponent<Scrollbar>();
        // Initialize the scrollbar value to the current music volume
        musicVolumeScrollbar.value = SoundManager.Instance.GetMusicVolume();

        // Add a listener to the scrollbar to handle volume changes
        musicVolumeScrollbar.onValueChanged.AddListener(HandleMusicVolumeChange);
    }

    private void HandleMusicVolumeChange(float value)
    {
        // Update the music volume in the SoundManager
        SoundManager.Instance.SetMusicVolume(value);
    }
}
