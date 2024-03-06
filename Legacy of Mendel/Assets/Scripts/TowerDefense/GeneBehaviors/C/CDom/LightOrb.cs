using System.Collections;
using UnityEngine;

public class LightOrb : MonoBehaviour
{
    public Transform pivot;
    public Vector3 startRotation;
    public ParticleSystem ps;
    public bool playPS = false;

    //Light Orb Move Settings
    public float moveDistance = 15f;
    private float moveSpeed;

    private float duration = 1.25f; // Duration to reach the target
    private Vector3 startPosition;
    private float timer;

    public void PrepareAttack(Vector3 targetPoint)
    {
        moveSpeed = moveDistance / duration;

        if (playPS && ps != null) ps.Play();

        startPosition = pivot.position; // Start from the pivot's position

        // Calculate the initial direction towards the target
        Vector3 directionToTarget = (targetPoint - startPosition).normalized;
        directionToTarget.y = 0; // Ensure movement is horizontal

        // Apply the startRotation as a directional offset
        Quaternion rotationOffset = Quaternion.Euler(startRotation);
        Vector3 rotatedDirection = rotationOffset * directionToTarget;

        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(rotatedDirection);

        timer = 0; // Reset timer
        StartCoroutine(StartMove());
    }


    private IEnumerator StartMove()
    {
        while (timer < duration)
        {
            // Move the GameObject forward at moveSpeed units per second
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // Increment the timer by the time passed since the last frame
            timer += Time.deltaTime;

            // Yield until the next frame
            yield return null;
        }

        // Ensure the coroutine doesn't exit until the end of the frame in which timer exceeds duration
        yield return null;
    }
}
