using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private IAttackBehavior[] attackBehaviors;

    private NavMeshAgent agent;

    public float alertRadius = 30f;

    private GameObject Mendelbase;
    [HideInInspector] public GameObject targetDefender;

    [HideInInspector] public GeneTypeEntry lootGeneType = null;
    [HideInInspector] public int lootCultureMedium = 0;

    /*public float attackPower = 1f;
    public float attackSpeed = 1f;*/
    [HideInInspector] public float attackRange = 100f;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isFrozen = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        attackBehaviors = GetComponents<IAttackBehavior>();

        //Set first target to base
        Mendelbase = GameObject.FindGameObjectWithTag("Base");
        agent.destination = Mendelbase.transform.position;

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
        //Pathfinding logic starts----------------------------------------------------------------

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
                    isAttacking = true;
                }
            }
            // Check if within alert radius but outside attack range
            else if (distanceToDefender <= alertRadius && distanceToDefender > attackRange)
            {
                isAttacking = false;
                MoveTowardsTarget(targetDefender);
            }
            // If the defender is outside the alert radius
            else
            {
                targetDefender = null;  // Lose the target if it's outside the alert range
                MoveTowardsTarget(Mendelbase);
                isAttacking = false;
            }
        }
        // If there's no defender to target, move towards the base
        else
        {
            MoveTowardsTarget(Mendelbase);
            isAttacking = false;
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
        isAttacking = true;
        /*if (!isAttacking)
        {
            StartCoroutine(AttackRoutine(target));
        }*/
    }

    /*private IEnumerator AttackRoutine(GameObject target)
    {
        isAttacking = true;

        while (target != null)
        {
            HP hP = target.GetComponent<HP>();
            if (hP != null)
            {
                hP.TakeDamage(attackPower);
            }

            yield return new WaitForSeconds(1 / attackSpeed); // Delay between attacks
        }

        isAttacking = false;
    }*/

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

    private void OnEnable()
    {
        EnemyManager.Instance.RegisterEnemy(gameObject);
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.UnregisterEnemy(gameObject);
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
}
