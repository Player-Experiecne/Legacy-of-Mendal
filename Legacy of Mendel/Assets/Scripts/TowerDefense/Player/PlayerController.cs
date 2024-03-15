using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance = 1.0f; // Adjust as needed
    [SerializeField] private LayerMask groundLayer; // Set this in the inspector

    // Define the height of player
    public float yOffset = 8.0f;
    // Define the moving speed
    public float speed = 15f;
    // Define boundaries for the player's position
    public float minX = 37f;
    public float maxX = 308f;
    public float minZ = 288f;
    public float maxZ = 416f;

    [HideInInspector] public Vector3 groundPosition;

    void FixedUpdate()
    {
        PlayerMovement();
        ClampPlayerPosition();
    }

    void PlayerMovement()
    {
        float horizontal = InputManager.Instance.GetHorizontal();
        float vertical = InputManager.Instance.GetVertical();

        Vector3 movement = new Vector3(vertical, 0.0f, -horizontal).normalized;
        movement = Quaternion.Euler(0, 135, 0) * movement;
        movement *= speed * Time.deltaTime;

        // Move without adjusting Y position
        Vector3 horizontalMovement = new Vector3(movement.x, 0.0f, movement.z);
        transform.Translate(horizontalMovement, Space.World);

        // Adjust Y position based on terrain/ground
        AlignWithGround();
    }

    void AlignWithGround()
    {
        RaycastHit hit;
        Vector3 raycastStart = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        if (Physics.Raycast(raycastStart, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            // Set the height to align with the ground
            transform.position = new Vector3(transform.position.x, hit.point.y + yOffset, transform.position.z);
            groundPosition = hit.point;
        }
    }

    void ClampPlayerPosition()
    {
        //Clamp the player's position to keep them within the defined bounds
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);

        transform.position = clampedPosition;
    }
}
