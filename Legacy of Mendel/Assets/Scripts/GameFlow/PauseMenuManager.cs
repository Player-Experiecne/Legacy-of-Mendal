using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject settingsMenu;


    private bool isPaused = false; // Track the pause state
    void Update()
    {
        if (InputManager.Instance.GetKeyDown("PauseMenu") && !GameManager.Instance.isTitleScreen)
        {
            if (settingsMenu.activeSelf)
            {
                menu.SetActive(true);
                settingsMenu.SetActive(false);
            }
            else
            {
                TogglePause();
            }
        }
    }
    public void TogglePause()
    {
        isPaused = !isPaused;

        menu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0 : 1;

        InputManager.Instance.SetInputEnabled(!isPaused);
    }
}