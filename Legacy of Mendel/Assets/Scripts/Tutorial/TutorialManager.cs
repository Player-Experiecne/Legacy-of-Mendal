using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeDelayBeforeStart = 5;
    public GameObject Image1;
    public NavMeshAgent agent;
    private float originalSpeed;
    public MonoBehaviour movementScript;

    public GameObject[] tutorialSteps; // UI Elements for each step
    public int currentStep = 0;
    void Start()
    {
        LockMovement(movementScript);
        StartCoroutine(PrepareTutorial());
        StartCoroutine(BeginTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public 
    IEnumerator PrepareTutorial()
    {
        
        yield return new WaitForSeconds(3);
        PauseNavMeshAgent(agent);
        

        Image1.SetActive(true);
    }

    IEnumerator BeginTutorial()
    {
        foreach (GameObject step in tutorialSteps)
        {
            step.SetActive(true); // Show current step
            HighlightUIElement(step); // Highlight the UI element if needed
            yield return new WaitForSeconds(1); // Wait for a second before checking for input
            yield return new WaitUntil(() => PlayerMadeCorrectInput(step)); // Wait until player interacts correctly
            step.SetActive(false); // Hide current step after interaction
        }
        // End of tutorial
    }
    void HighlightUIElement(GameObject element)
    {
        // Implement the logic to highlight the UI element
        // This could be a simple animation, a change of color, a frame around the element, etc.
    }

    bool PlayerMadeCorrectInput(GameObject step)
    {
        // Implement the logic to check if player made the correct interaction
        // This might involve subscribing to button events or other input detection methods.
        return true; // Replace this with actual check
    }

    // Call this from the UI button's onClick event
    public void PlayerClickedButton()
    {
        if (currentStep < tutorialSteps.Length)
        {
            currentStep++;
        }
    }
    public void PauseNavMeshAgent(NavMeshAgent agent)
    {
        originalSpeed = agent.speed;
        agent.speed = 0;
        Debug.Log("NavMeshAgent speed set to 0");
    }

    public void ResumeNavMeshAgent(NavMeshAgent agent)
    {
        agent.speed = originalSpeed;
        Debug.Log("NavMeshAgent speed restored");
    }
    public void LockMovement(MonoBehaviour movementScript)
    {
        movementScript.enabled = false;
    }

    public void UnlockMovement(MonoBehaviour movementScript)
    {
        movementScript.enabled = true;
    }

}
