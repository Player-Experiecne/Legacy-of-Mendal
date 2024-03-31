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
    void Start()
    {

        StartCoroutine(BeginTutorial());

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public 
    IEnumerator BeginTutorial()
    {
        
        yield return new WaitForSeconds(3);
        PauseNavMeshAgent(agent);
        LockMovement(movementScript);

        Image1.SetActive(true);
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
