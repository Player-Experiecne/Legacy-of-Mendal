using UnityEngine;
using UnityEngine.UI; // Required for accessing Button components

public class PauseButton : MonoBehaviour
{
    private Button pauseButton;

    void Start()
    {
        // Get the Button component attached to this GameObject
        pauseButton = GetComponent<Button>();
        // Register the OnClick event listener
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OnClickPauseButton);
        }
    }

    void OnClickPauseButton()
    {
        // Call the TriggerPause method on the PauseMenuManager instance
        PauseMenuManager.Instance.TogglePause();
    }

    private void OnDestroy()
    {
        // Make sure to remove the listener when the button is destroyed
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveListener(OnClickPauseButton);
        }
    }
}
