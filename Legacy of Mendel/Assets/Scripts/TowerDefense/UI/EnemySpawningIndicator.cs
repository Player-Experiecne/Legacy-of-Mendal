using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class EnemySpawningIndicator : MonoBehaviour
{
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI levelText;
    public float timeToMove = 5.0f; // Duration of the move and shrink animation
    public float timeBeforeMove = 2.0f;
    // Adjust these in the Inspector or directly here
    private Vector3 targetPosition = new Vector3(-500, 425, 0);
    private Vector3 targetScale = new Vector3(0.25f, 0.25f, 1f);
    private float timeDelayBeforeStart;

    void Start()
    {
        timeDelayBeforeStart = GameManager.Instance.timeDelayBeforeStart; // This depends on your GameManager setup

        // Initialize with exact starting position and scale if needed
        transform.localPosition = new Vector3(0, 100, 0); // Use localPosition if this object is a child
        transform.localScale = new Vector3(0.7f, 0.7f, 1);

        StartCoroutine(MoveAndShrinkPanel());
    }

    void Update()
    {
        // Update countdown in the text
        if (timeDelayBeforeStart > 0)
        {
            warningText.text = $"{timeDelayBeforeStart.ToString("F1")} seconds!";
            levelText.text = "Level " + GameManager.Instance.currentLevelIndex + 1;
            timeDelayBeforeStart -= Time.deltaTime;
        }
        else { Destroy(gameObject); }
    }

    IEnumerator MoveAndShrinkPanel()
    {
        yield return new WaitForSeconds(timeBeforeMove);

        Vector3 startPosition = transform.localPosition; // Use localPosition for child objects
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < timeToMove)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / timeToMove));
            transform.localScale = Vector3.Lerp(startScale, targetScale, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure exact target position and scale are set after animation
        transform.localPosition = targetPosition;
        transform.localScale = targetScale;
    }
}

