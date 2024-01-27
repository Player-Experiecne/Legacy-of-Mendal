using UnityEngine;
using UnityEngine.EventSystems;

public class CursorHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject uiObject; // Drag your UI object here in the inspector
    private bool isCursorOverButton = false;

    void Start()
    {
        if (uiObject != null)
        {
            uiObject.SetActive(false); // Hide UI object initially
        }
    }

    void Update()
    {
        if (isCursorOverButton)
        {
            uiObject.transform.position = Input.mousePosition; // Update UI object position
        }
        else
        {
            uiObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiObject != null)
        {
            isCursorOverButton = true;
            uiObject.SetActive(true); // Show the UI object
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (uiObject != null)
        {
            isCursorOverButton = false;
            uiObject.SetActive(false); // Hide the UI object
        }
    }
}
