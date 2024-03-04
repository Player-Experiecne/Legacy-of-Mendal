using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset;
    public float rotationSpeed = 1.0f; // Speed of rotation

    void Update()
    {
        if (InputManager.Instance.GetKey("CameraTurnLeft"))
        {
            RotateAroundPlayer(-rotationSpeed);
        }
        else if (InputManager.Instance.GetKey("CameraTurnRight"))
        {
            RotateAroundPlayer(rotationSpeed);
        }

        transform.position = player.position + offset;
        transform.LookAt(player);
    }

    void RotateAroundPlayer(float rotationAmount)
    {
        // Rotate the offset
        offset = Quaternion.Euler(0, rotationAmount, 0) * offset;

        // Optional: You can also rotate the camera itself if you want it to tilt or roll
        // transform.Rotate(0, rotationAmount, 0);
    }
}
