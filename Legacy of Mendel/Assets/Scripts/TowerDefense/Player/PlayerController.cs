using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 15f;

    // Define boundaries for the player's position
    public float minX = 37f;
    public float maxX = 308f;
    public float minZ = 288f;
    public float maxZ = 416f;

    public GameObject menu;
    public GameObject settingsMenu;

    private bool isPaused = false; // Track the pause state

    void Update()
    {
        PlayerMovement();
        //ClampPlayerPosition();
        if (Input.GetKeyDown(KeyCode.Escape))
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

    void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Using GetAxisRaw for direct input
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(-vertical, 0.0f, horizontal).normalized;

        // Rotate the movement direction by 45 degrees around the y-axis
        movement = Quaternion.Euler(0, 135, 0) * movement;

        movement = movement * speed * Time.deltaTime;

        transform.Translate(movement);
    }

    void ClampPlayerPosition()
    {
        //Clamp the player's position to keep them within the defined bounds
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);

        transform.position = clampedPosition;

    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        menu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0 : 1;

    }
}
