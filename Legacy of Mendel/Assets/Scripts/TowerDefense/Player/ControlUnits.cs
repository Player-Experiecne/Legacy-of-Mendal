using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using HighlightPlus;

public class ControlUnits : MonoBehaviour
{
    public Image collectCircleUI; // Assign in inspector, fixed size for collect mode
    public Image controlCircleUI; // Assign in inspector, hidden initially
    public HighlightProfile highlightProfile; // For highlight effect, assign in inspector
    public List<GameObject> controlledDefenders = new();
    private bool isInControlMode = false;
    private bool isCollecting = false; // Flag to control ongoing collection
    private bool canStartNewCollection = true; // Flag to prevent immediate re-collection
    private float collectCooldown = 0.1f; // Cooldown duration in seconds before allowing another collect
    private float positionTransitionTime = 0.5f;

    private void Update()
    {
        if (InputManager.Instance.GetKeyDown("ControlUnits") && canStartNewCollection)
        {
            if (!isInControlMode)
            {
                collectCircleUI.gameObject.SetActive(true);
                isCollecting = true;
                StartCoroutine(CollectDefendersContinuously()); // Start collecting defenders continuously
            }
            else
            {
                // Exit control mode and start cooldown
                StartCoroutine(ExitControlModeAndCooldown());
            }
        }

        if (InputManager.Instance.GetKeyUp("ControlUnits") && isCollecting)
        {
            // Stop the collect process
            isCollecting = false;
            StopCollectMode();
        }
    }

    private IEnumerator CollectDefendersContinuously()
    {
        while (isCollecting)
        {
            CollectDefendersWithinRadius();
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds, adjust as needed
        }
    }

    private void CollectDefendersWithinRadius()
    {
        float radius = collectCircleUI.rectTransform.sizeDelta.x / 2 * 0.8f;
        for (int i = DefenderManager.Instance.defenders.Count - 1; i >= 0; i--)
        {
            GameObject defender = DefenderManager.Instance.defenders[i];
            if (Vector3.Distance(transform.position, defender.transform.position) <= radius && !defender.CompareTag("Base"))
            {
                if (!defender.GetComponent<DefenderController>().isControlled)
                {
                    defender.GetComponent<DefenderController>().EnterControlledMode(transform);
                    controlledDefenders.Add(defender);
                    defender.transform.parent = transform;
                    UpdateDefenderPositions(true);
                    AddHighlightEffect(defender);
                    DefenderManager.Instance.defenders.RemoveAt(i); // Safely remove the defender
                }
            }
        }
    }


    private IEnumerator ExitControlModeAndCooldown()
    {
        ExitControlMode();
        canStartNewCollection = false;
        yield return new WaitForSeconds(collectCooldown);
        canStartNewCollection = true;
    }

    private void StopCollectMode()
    {
        collectCircleUI.gameObject.SetActive(false); // Hide collect circle immediately after release

        if (controlledDefenders.Count > 0)
        {
            isInControlMode = true;
            controlCircleUI.gameObject.SetActive(true); // Show control circle indicating control mode
        }

        // If no defenders are controlled, also exit control mode and hide the control circle
        if (controlledDefenders.Count == 0)
        {
            isInControlMode = false;
            controlCircleUI.gameObject.SetActive(false);
        }
    }

    private void ExitControlMode()
    {
        // Distribute these defenders evenly within the control circle
        UpdateDefenderPositions(false);

        // Exit controlled mode for each defender
        foreach (var defender in controlledDefenders)
        {
            defender.transform.SetParent(null); // Remove the parent immediately
            StartCoroutine(TransitionToGround(defender, positionTransitionTime)); // Smoothly transition each defender to the ground
            ClearHighlightEffect(defender);
        }

        // Reset the controlled list
        controlledDefenders.Clear();

        // Reset flags and UI for exiting control mode
        isCollecting = false;
        isInControlMode = false;
        controlCircleUI.gameObject.SetActive(false);
    }

    /*private void DistributeDefendersEvenly(float radius, float yOffset = 0)
    {
        for (int i = 0; i < controlledDefenders.Count; i++)
        {
            float angle = i * (360f / controlledDefenders.Count);
            Vector3 position = CalculatePositionOnCircle(transform.position, radius, angle);
            position.y += yOffset;
            controlledDefenders[i].transform.position = position;
        }
    }*/

    /*private Vector3 CalculatePositionOnCircle(Vector3 center, float radius, float angleInDegrees)
    {
        // Angle in radians
        float angleInRadians = angleInDegrees * (Mathf.PI / 180f);
        float x = center.x + radius * Mathf.Cos(angleInRadians);
        float z = center.z + radius * Mathf.Sin(angleInRadians);
        return new Vector3(x, center.y, z); // Assuming y is the ground level, adjust as needed
    }*/

    private void AddHighlightEffect(GameObject target)
    {
        HighlightEffect highlight = target.AddComponent<HighlightEffect>();
        highlight.ProfileLoad(highlightProfile);
        highlight.highlighted = true;
    }

    private void ClearHighlightEffect(GameObject target)
    {
        HighlightEffect highlight = target.GetComponent<HighlightEffect>();
        Destroy(highlight);
    }

    private IEnumerator SmoothTransition(GameObject defender, Vector3 startLocalPosition, Vector3 endLocalPosition, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            defender.transform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        defender.transform.localPosition = endLocalPosition; // Ensure it reaches the final local position
    }


    private void UpdateDefenderPositions(bool toAir)
    {
        if(toAir) { }
        float radius = controlCircleUI.rectTransform.sizeDelta.x / 2 * 0.8f;
        float yOffset = toAir ? 10f : 0f; // Adjust this value to control the height offset when floating

        for (int i = 0; i < controlledDefenders.Count; i++)
        {
            GameObject defender = controlledDefenders[i];
            float angle = i * (360f / controlledDefenders.Count);
            Vector3 newPosition = CalculateLocalPositionOnCircle(radius, angle, yOffset);
            StartCoroutine(SmoothTransition(defender, defender.transform.localPosition, newPosition, positionTransitionTime));
        }
    }

    private Vector3 CalculateLocalPositionOnCircle(float radius, float angleInDegrees, float yOffset)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(angleInRadians);
        float z = radius * Mathf.Sin(angleInRadians);
        return new Vector3(x, yOffset, z); // Use local coordinates relative to the parent
    }

    private IEnumerator TransitionToGround(GameObject defender, float duration)
    {
        Vector3 startWorldPosition = defender.transform.position;
        Vector3 endWorldPosition = new Vector3(startWorldPosition.x, transform.position.y, startWorldPosition.z); // Assuming y=0 is ground level

        float elapsed = 0f;
        while (elapsed < duration)
        {
            defender.transform.position = Vector3.Lerp(startWorldPosition, endWorldPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        defender.transform.position = endWorldPosition; // Ensure it's exactly at ground level at the end

        // Defender exit controll mode logic
        defender.GetComponent<DefenderController>().ExitControlledMode();
        DefenderManager.Instance.defenders.Add(defender);
    }


}
