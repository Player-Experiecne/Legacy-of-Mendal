using UnityEngine;

public class HPBarFixedDirection : MonoBehaviour
{
    private Vector3 offset;         // The local offset from the parent (enemy)
    private Transform parentTransform;  // Reference to the parent (enemy) transform
    private Quaternion initialRotation;

    private void Start()
    {
        parentTransform = transform.parent;
        offset = transform.localPosition;  // Store the initial local position as offset
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        // Set the position based on the parent's position and the predetermined offset
        transform.position = parentTransform.TransformPoint(offset);

        // Freeze the rotation
        transform.rotation = initialRotation;
    }
}
