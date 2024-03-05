using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HP;

public class GeneCRecBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => attackRange;

    [Header("DO NOT modify!!!")]
    public GameObject lightningPrefab;

    [Header("Damage Settings")]
    public float instantDamage = 30f;    // Instant damage applied upon touch.

    [Header("Lightning Settings")]
    private float attackInterval = 1.5f;
    public float attackRange = 20f;
    private float lightningDuration = 1f;

    [Header("Chain Settings")]
    public float chainTimes = 3f;
    public float chainRange = 5f;
    private float chainInterval = 0.5f;

    private float nextFireTime = 1.0f;

    private HP selfHP;
    [HideInInspector] public GameObject target;
    private EnemyController enemyController;
    private DefenderController defenderController;
    private bool isAttacking;

    private void Awake()
    {
        selfHP = GetComponent<HP>();

        if (selfHP.objectType == ObjectType.Enemy)
        {
            enemyController = GetComponent<EnemyController>();
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            defenderController = GetComponent<DefenderController>();
        }
    }

    private void Update()
    {
        //Get isattcking state
        if (selfHP.objectType == ObjectType.Enemy)
        {
            isAttacking = enemyController.isAttacking;
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            isAttacking = defenderController.isAttacking;
        }

        //Start attacking
        if (Time.time > nextFireTime && isAttacking)
        {
            if (selfHP.objectType == ObjectType.Enemy)
            {
                target = enemyController.targetDefender;
            }
            else if (selfHP.objectType == ObjectType.Defender)
            {
                target = defenderController.targetEnemy;
            }
            StartCoroutine(ChainLightning()); 
            nextFireTime = Time.time + attackInterval;
            if (!isAttacking)
            {
                StopAllCoroutines();
            }
        }
    }
    private IEnumerator ChainLightning()
    {
        HashSet<GameObject> struckTargets = new HashSet<GameObject>();
        GameObject currentTarget = target; // Initial target
        GameObject previousTarget = gameObject; // Start from the host
        struckTargets.Add(gameObject); // Assuming the host should not be a target

        for (int i = 0; i < chainTimes; i++)
        {
            if (currentTarget != null && !struckTargets.Contains(currentTarget))
            {
                StartCoroutine(LaunchLightning(previousTarget, currentTarget));
                struckTargets.Add(currentTarget); // Mark current target as struck

                // Wait for the lightning duration before continuing the chain
                yield return chainInterval;

                // Find the next closest target, excluding any that have been struck
                previousTarget = currentTarget;
                currentTarget = selfHP.objectType == ObjectType.Enemy ?
                    FindClosestDefender(currentTarget, struckTargets) :
                    FindClosestEnemy(currentTarget, struckTargets);
            }
            else
            {
                // No valid target found, exit the loop
                break;
            }
        }
    }

    private IEnumerator LaunchLightning(GameObject host, GameObject lightningTarget)
    {
        // Instantiate the lightning prefab at the host's position
        GameObject lightningInstance = Instantiate(lightningPrefab, host.transform.position, Quaternion.identity);

        // Assuming the first child is the start point and the second child is the end point
        Transform startPoint = lightningInstance.transform.GetChild(0);
        Transform endPoint = lightningInstance.transform.GetChild(1);

        // Set the start point as a child of the host, and the end point as a child of the target
        startPoint.SetParent(host.transform);
        endPoint.SetParent(lightningTarget.transform);

        // Optionally, you might want to offset the child positions to match exactly where you want them on the host and target
        startPoint.localPosition = Vector3.zero; // Adjust this as needed
        endPoint.localPosition = Vector3.zero; // Adjust this as needed

        // Wait for the lightning effect to complete its duration before cleaning up
        yield return new WaitForSeconds(lightningDuration);

        // After the effect duration, destroy the instantiated lightning prefab
        Destroy(startPoint.gameObject);
        Destroy(endPoint.gameObject);
        Destroy(lightningInstance);
    }


    private GameObject FindClosestDefender(GameObject currentTarget, HashSet<GameObject> struckTargets)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestDefender = null;

        foreach (GameObject defender in DefenderManager.Instance.defenders)
        {
            if (defender == currentTarget || struckTargets.Contains(defender)) continue;

            float distance = Vector3.Distance(currentTarget.transform.position, defender.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDefender = defender;
            }
        }

        return closestDefender;
    }

    private GameObject FindClosestEnemy(GameObject currentTarget, HashSet<GameObject> struckTargets)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in EnemyManager.Instance.Enemies)
        {
            if (enemy == currentTarget || struckTargets.Contains(enemy)) continue;

            float distance = Vector3.Distance(currentTarget.transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
