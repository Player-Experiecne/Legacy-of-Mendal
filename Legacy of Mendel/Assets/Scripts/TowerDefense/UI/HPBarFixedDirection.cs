using UnityEngine;

public class HPBarFixedDirection : MonoBehaviour
{
    private Vector3 offset;         // The local offset from the parent (enemy)
    private Transform parentTransform;  // Reference to the parent (enemy) transform
    private Quaternion fixedRotation;

    private void Start()
    {
        parentTransform = transform.parent;
        offset = transform.localPosition;  // Store the initial local position as offset
        fixedRotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        // Set the position based on the parent's position and the predetermined offset
        transform.position = parentTransform.TransformPoint(offset);

        // Freeze the rotation
        transform.rotation = fixedRotation;

        // Rotate with the camera

        // Get the current rotation angles of the object in Euler angles for easier manipulation
        Vector3 currentRotationEulerAngles = transform.rotation.eulerAngles;
        // Get the camera's y rotation
        float cameraYRotation = Camera.main.transform.rotation.eulerAngles.y;
        // Construct a new Quaternion for the rotation, combining the original x, z with the camera's y
        Quaternion newRotation = Quaternion.Euler(currentRotationEulerAngles.x, cameraYRotation + 135, currentRotationEulerAngles.z);
        // Apply the new rotation to the transform
        transform.rotation = newRotation;

    }
}
