using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DefenderController : MonoBehaviour
{
    private IAttackBehavior[] attackBehaviors;

    private NavMeshAgent agent;

    public float alertRadius = 10f;

    private GameObject defendPoint; // The location the defender should defend when not engaging enemies.
    [HideInInspector] public GameObject targetEnemy;

    /*public float attackPower = 1f;
    public float attackSpeed = 1f;*/
    [HideInInspector] public float attackRange = 100f;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isControlled = false;
    [HideInInspector] public bool isFrozen = false;
    [HideInInspector] public Transform playerTransform;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        attackBehaviors = GetComponents<IAttackBehavior>();
        // Remember the current location to be the defendPoint
        defendPoint = new GameObject("_DefendPoint");
        defendPoint.transform.position = gameObject.transform.position;
        MoveTowardsTarget(defendPoint);

        //Select attack range
        IAttackBehavior selectedAttack = SelectAttackRange();
        if (selectedAttack != null)
        {
            attackRange = selectedAttack.AttackRange;
            // Use attackRange for selected attack
        }
    }
    void Update()
    {
        if (isFrozen) { return; }
        if (isControlled) 
        { 
            //GetControlled(playerTransform);
            return;
        }
        else { AutoDefend(); }
    }

    private void AutoDefend()
    {
        // If the defender doesn't have a target or if its target was destroyed
        if (targetEnemy == null || !EnemyManager.Instance.Enemies.Contains(targetEnemy))
        {
            FindClosestEnemy();
        }

        if (targetEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy.transform.position);

            // Check if within attack range
            if (distanceToEnemy <= attackRange)
            {
                StopMovement();
                RotateTowards(targetEnemy.transform.position);
                if (IsFacingTarget(targetEnemy.transform.position))
                {
                    isAttacking = true;
                }
            }
            // Check if within alert radius but outside attack range
            else if (distanceToEnemy <= alertRadius && distanceToEnemy > attackRange)
            {
                MoveTowardsTarget(targetEnemy);
                isAttacking = false;
            }
            // If the enemy is outside the alert radius
            else
            {
                targetEnemy = null;  // Lose the target if it's outside the alert range
                MoveTowardsTarget(defendPoint);
                isAttacking = false;
            }
        }
        // If there's no enemy to target, move towards the defend point
        else
        {
            MoveTowardsTarget(defendPoint);
            isAttacking = false;
        }
    }

    private void FindClosestEnemy()
    {
        float closestDistance = alertRadius;
        GameObject closestEnemy = null;
        foreach (GameObject enemy in EnemyManager.Instance.Enemies)
        {
            float currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestEnemy = enemy;
            }
        }
        targetEnemy = closestEnemy;
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

    IAttackBehavior SelectAttackRange()
    {
        if (attackBehaviors.Length == 0)
        {
            return null; // No attack behaviors available
        }

        IAttackBehavior minAttackRange = attackBehaviors[0];
        foreach (var attackBehavior in attackBehaviors)
        {
            if (attackBehavior.AttackRange < minAttackRange.AttackRange)
            {
                minAttackRange = attackBehavior;
            }
        }
        return minAttackRange;
    }

    public void EnterControlledMode(Transform playerTransform)
    {
        StopMovement();
        isAttacking = false;
        agent.isStopped = false; // Allow the agent to move again
        this.playerTransform = playerTransform;
        isControlled = true;
        agent.enabled = false;
    }
    /*private void GetControlled(Transform playerTransform)
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
        }
    }*/

    public void ExitControlledMode()
    {
        isControlled = false;
        defendPoint.transform.position = transform.position; // Update defend point to current location
        agent.enabled = true;
        MoveTowardsTarget(defendPoint); // Resume autonomous behavior
    }

    private void OnEnable()
    {
        DefenderManager.Instance.RegisterDefender(gameObject);
    }

    private void OnDestroy()
    {
        DefenderManager.Instance.UnregisterDefender(gameObject);
    }
}
