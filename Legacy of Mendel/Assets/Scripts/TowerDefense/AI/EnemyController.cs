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
            float distanceToDefender = Vector3.Distance(transform.position, targetDefender.transform.position);

            // Check if within attack range
            if (distanceToDefender <= attackRange)
            {
                StopMovement();
                Attack(targetDefender);
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

    private void OnEnable()
    {
        EnemyManager.Instance.RegisterEnemy(gameObject);
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.UnregisterEnemy(gameObject);
    }
}
