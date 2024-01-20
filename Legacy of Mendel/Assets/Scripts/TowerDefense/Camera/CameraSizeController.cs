using UnityEngine;

public class CameraSizeController : MonoBehaviour
{
    public float zoomSpeed = 0.5f;    // The speed at which the camera zooms in or out.
    public float minZoom = 2f;        // The minimum zoom level (smaller size means more zoomed in).
    public float maxZoom = 10f;       // The maximum zoom level.

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the orthographic size based on the mouse scroll input.
        cam.orthographicSize -= scrollInput * zoomSpeed;

        // Clamp the orthographic size between the min and max zoom levels.
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }
}
