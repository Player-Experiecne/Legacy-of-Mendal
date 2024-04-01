using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeDelayBeforeStart = 5;
    public GameObject Image1;
    public GameObject Image2;
    
    public GameObject Image3;
    public NavMeshAgent agent;
    private float originalSpeed;
    public MonoBehaviour movementScript;
    public TutorialDefenderBackpack defenderBackpack;
    public GameObject[] tutorialSteps; // UI Elements for each step
    public TutorialBuildManager tbm;
    public int currentStep = 0;
    void Start()
    {
        LockMovement(movementScript);
        StartCoroutine(PrepareTutorial());
        //StartCoroutine(BeginTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public
    IEnumerator PrepareTutorial()
    {
        // 等待3秒开始教程
        yield return new WaitForSeconds(3);
        PauseNavMeshAgent(agent);

        // 显示提示选择Defender的图片
        Image1.SetActive(true);

        yield return new WaitUntil(() => defenderBackpack.HasActiveDefender());

        Debug.Log(defenderBackpack.HasActiveDefender());

        // 选中Defender后隐藏提示图片
        Image1.SetActive(false);
        Image2.SetActive(true); // 假设Image2是提示玩家放置Defender的图片

        // 等待直到玩家放置了Defender
        yield return new WaitUntil(() => tbm.HasPlacedDefender());

        Debug.Log("Defender has been placed, continuing tutorial...");

        // 放置Defender后隐藏提示图片
        Image2.SetActive(false);
        ResumeNavMeshAgent(agent);
        yield return new WaitUntil(() => GameObject.FindWithTag("Enemy") == null);
        Image3.SetActive(true);

    }


    IEnumerator BeginTutorial()
    {
        
        yield return new WaitForSeconds(1); 
        if (defenderBackpack.activeDefender!=null)
        {
            Image2.SetActive(true);
            Image1.SetActive(false);
        }
        else
        {
            
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
