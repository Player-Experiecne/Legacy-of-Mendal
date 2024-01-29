using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        // Quit the application
        Application.Quit();

        // For debugging: Log a message in the Unity Editor
        Debug.Log("Game is exiting");
    }
}
