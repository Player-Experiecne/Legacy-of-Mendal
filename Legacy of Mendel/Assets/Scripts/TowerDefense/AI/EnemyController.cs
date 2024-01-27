using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;

    public float alertRadius = 10f;

    private GameObject Mendelbase;
    public GameObject targetDefender;

    public List<GeneInfo.geneTypes> lootGeneTypes;
    public int lootCultureMedium;

    public float attackPower = 1f;
    public float attackRange = 5f;
    public float attackSpeed = 1f;
    private bool isAttacking = false;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //Set first target to base
        Mendelbase = GameObject.FindGameObjectWithTag("Base");
        agent.destination = Mendelbase.transform.position;
    }

    void Update()
    {
        // If the enemy doesn't have a target or if its target was destroyed
        if (targetDefender == null || !DefenderManager.Instance.defenders.Contains(targetDefender))
        {
            FindClosestDefender();
        }

        if (targetDefender != null)
        {
            Collider targetCollider = targetDefender.GetComponent<Collider>();
            Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);

            float distanceToDefender = Vector3.Distance(transform.position, closestPoint);

            // Check if within attack range
            if (distanceToDefender <= attackRange)
            {
                StopMovement();
                RotateTowards(targetDefender.transform.position);
                if (IsFacingTarget(targetDefender.transform.position))
                {
                    Attack(targetDefender);
                }
            }
            // Check if within alert radius but outside attack range
            else if (distanceToDefender <= alertRadius && distanceToDefender > attackRange)
            {
                MoveTowardsTarget(targetDefender);
            }
            // If the defender is outside the alert radius
            else
            {
                targetDefender = null;  // Lose the target if it's outside the alert range
                MoveTowardsTarget(Mendelbase);
            }
        }
        // If there's no defender to target, move towards the base
        else
        {
            MoveTowardsTarget(Mendelbase);
        }
    }

    private void FindClosestDefender()
    {
        float closestDistance = alertRadius;
        GameObject closestDefender = null;
        foreach (GameObject defender in DefenderManager.Instance.defenders)
        {
            float currentDistance = Vector3.Distance(transform.position, defender.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestDefender = defender;
            }
        }
        targetDefender = closestDefender;
    }

    private void Attack(GameObject target)
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackRoutine(target));
        }
    }

    private IEnumerator AttackRoutine(GameObject target)
    {
        isAttacking = true;

        while (target != null)
        {
            HP hP = target.GetComponent<HP>();
            if (hP != null)
            {
                hP.TakeDamage(attackPower);
            }

            yield return new WaitForSeconds(1 / attackSpeed);  // Delay between attacks
        }

        isAttacking = false;
    }

    private void MoveTowardsTarget(GameObject target) 
    {
        if (agent && target)
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
        }
    }

    void StopMovement()
    {
        if (agent)
        {
            agent.isStopped = true;
        }
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0; // Keep rotation horizontal
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Calculate rotation speed fraction
        float rotationFraction = (agent.angularSpeed / 360f) * Time.deltaTime * 2f;

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationFraction);
    }

    bool IsFacingTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // Set a threshold angle to determine if the enemy is facing the target
        float facingThreshold = 30f; // Adjust this value as needed

        return angleToTarget < facingThreshold;
    }

    public void dropLoot()
    {

    }
    private void OnEnable()
    {
        EnemyManager.Instance.RegisterEnemy(gameObject);
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.UnregisterEnemy(gameObject);
    }
}
