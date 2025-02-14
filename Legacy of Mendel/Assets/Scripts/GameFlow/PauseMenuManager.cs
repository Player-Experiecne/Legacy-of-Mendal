using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance;

    public GameObject menu;
    public GameObject Settings;

    private bool isPaused = false; // Track the pause state
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (InputManager.Instance.GetKeyDown("PauseMenu") && !GameManager.Instance.isTitleScreen)
        {
            if (Settings.activeSelf)
            {
                menu.SetActive(true);
                Settings.SetActive(false);
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