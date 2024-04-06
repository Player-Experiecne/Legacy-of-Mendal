using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CocoAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    private NavMeshAgent agent; 

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>(); 

        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        
        UpdateMovementAnimation();
    }

    private void UpdateMovementAnimation()
    {
        if (agent != null && animator != null)
        {
           
            bool isMoving = agent.velocity.magnitude > 0.1f;
            animator.SetBool("IsMoving", isMoving);
        }
    }
}
