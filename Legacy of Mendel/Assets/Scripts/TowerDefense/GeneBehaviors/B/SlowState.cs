using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SlowState : MonoBehaviour
{
    public float existingSlowRatio;
    public float slowDurationLeft;

    private NavMeshAgent navMeshAgent;
    private Coroutine slowCoroutine;
    private float originalSpeed; // Store the original speed of the NavMeshAgent

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalSpeed = navMeshAgent.speed; // Initialize originalSpeed
    }

    public void StartSlowed(float slowRatio, float slowDuration)
    {
        // If already slowed, stop the current slow effect before starting a new one
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
            // Reset speed to original before applying new slow effect
            navMeshAgent.speed = originalSpeed;
        }

        existingSlowRatio = slowRatio;
        slowDurationLeft = slowDuration;

        // Start a new slow effect
        slowCoroutine = StartCoroutine(Slow());
    }

    private IEnumerator Slow()
    {
        // Apply the new slow effect
        navMeshAgent.speed = originalSpeed * existingSlowRatio;

        // Wait for the slow duration to end
        yield return new WaitForSeconds(slowDurationLeft);

        // Reset the speed to original after the slow effect ends
        navMeshAgent.speed = originalSpeed;

        // Clear the reference to the coroutine since it's finished
        slowCoroutine = null;
    }
}
