using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 15f;

    // Define boundaries for the player's position
    public float minX = 37f;
    public float maxX = 308f;
    public float minZ = 288f;
    public float maxZ = 416f;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Using GetAxisRaw for direct input
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(-vertical, 0.0f, horizontal).normalized;

        // Rotate the movement direction by 45 degrees around the y-axis
        movement = Quaternion.Euler(0, 135, 0) * movement;

        movement = movement * speed * Time.deltaTime;

        transform.Translate(movement);

        /* Clamp the player's position to keep them within the defined bounds
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);

        transform.position = clampedPosition;*/
    }
}
