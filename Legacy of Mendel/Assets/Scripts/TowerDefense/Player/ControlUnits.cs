using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ControlUnits : MonoBehaviour
{
    public Image collectCircleUI; // Assign in inspector, fixed size for collect mode
    public Image controlCircleUI; // Assign in inspector, hidden initially
    public List<GameObject> controlledDefenders = new();
    private bool isInControlMode = false;
    private bool isCollecting = false; // Flag to control ongoing collection
    private bool canStartNewCollection = true; // Flag to prevent immediate re-collection
    private float collectCooldown = 0.5f; // Cooldown duration in seconds before allowing another collect

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
        float radius = collectCircleUI.rectTransform.sizeDelta.x / 2 * 0.8f; // Get radius from collect circle UI size, considering the scale of player gameobject
        foreach (var defender in DefenderManager.Instance.defenders)
        {
            if (Vector3.Distance(transform.position, defender.transform.position) <= radius)
            {
                if (!defender.GetComponent<DefenderController>().isControlled)
                {
                    defender.GetComponent<DefenderController>().EnterControlledMode(transform);
                    controlledDefenders.Add(defender);
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
        DistributeDefendersEvenly(controlCircleUI.rectTransform.sizeDelta.x / 2 * 0.8f);

        // Exit controlled mode for each defender
        foreach (var defender in controlledDefenders)
        {
            defender.GetComponent<DefenderController>().ExitControlledMode();
        }

        // Reset the controlled list
        controlledDefenders.Clear();

        // Reset flags and UI for exiting control mode
        isCollecting = false;
        isInControlMode = false;
        controlCircleUI.gameObject.SetActive(false);
    }

    private void DistributeDefendersEvenly(float radius)
    {
        for (int i = 0; i < controlledDefenders.Count; i++)
        {
            float angle = i * (360f / controlledDefenders.Count);
            Vector3 position = CalculatePositionOnCircle(transform.position, radius, angle);
            controlledDefenders[i].transform.position = position;
        }
    }

    private Vector3 CalculatePositionOnCircle(Vector3 center, float radius, float angleInDegrees)
    {
        // Angle in radians
        float angleInRadians = angleInDegrees * (Mathf.PI / 180f);
        float x = center.x + radius * Mathf.Cos(angleInRadians);
        float z = center.z + radius * Mathf.Sin(angleInRadians);
        return new Vector3(x, center.y, z); // Assuming y is the ground level, adjust as needed
    }
}
