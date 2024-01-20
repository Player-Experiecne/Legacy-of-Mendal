using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DefenderController : MonoBehaviour
{
    private NavMeshAgent agent;

    public float alertRadius = 10f;

    private GameObject defendPoint; // The location the defender should defend when not engaging enemies.
    public GameObject targetEnemy;

    public float attackPower = 1f;
    public float attackRange = 5f;
    public float attackSpeed = 1f;
    private bool isAttacking = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // Remember the current location to be the defendPoint
        defendPoint = new GameObject("_DefendPoint");
        defendPoint.transform.position = gameObject.transform.position;
        MoveTowardsTarget(defendPoint);
    }

    void Update()
    {
        // If the defender doesn't have a target or if its target was destroyed
        if (targetEnemy == null || !EnemyManager.Instance.enemies.Contains(targetEnemy))
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
                Attack(targetEnemy);
            }
            // Check if within alert radius but outside attack range
            else if (distanceToEnemy <= alertRadius && distanceToEnemy > attackRange)
            {
                MoveTowardsTarget(targetEnemy);
            }
            // If the enemy is outside the alert radius
            else
            {
                targetEnemy = null;  // Lose the target if it's outside the alert range
                MoveTowardsTarget(defendPoint);
            }
        }
        // If there's no enemy to target, move towards the defend point
        else
        {
            MoveTowardsTarget(defendPoint);
        }
    }

    private void FindClosestEnemy()
    {
        float closestDistance = alertRadius;
        GameObject closestEnemy = null;
        foreach (GameObject enemy in EnemyManager.Instance.enemies)
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

    private void OnEnable()
    {
        DefenderManager.Instance.RegisterDefender(gameObject);
    }

    private void OnDestroy()
    {
        DefenderManager.Instance.UnregisterDefender(gameObject);
    }
}
