using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CreditsManager : MonoBehaviour
{
    public float scrollSpeed = 30f; // Speed of the scroll
    public GameObject credits;
    public GameObject main;

    void Update()
    {
        // Optionally, reset the position to reuse the credits
        if (transform.localPosition.y >= 4050) // Adjust this value based on your needs
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(GameManager.Instance.isTitleScreen)
                {
                    main.SetActive(true);
                    credits.SetActive(false);
                }
                else
                {
                    credits.SetActive(false);
                    GameEvents.TriggerTitleScreen();
                }
                transform.localPosition = new Vector3(-377, -600, 0);
            }
            return;
        }
        // Move the text upward over time
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

    }
}
