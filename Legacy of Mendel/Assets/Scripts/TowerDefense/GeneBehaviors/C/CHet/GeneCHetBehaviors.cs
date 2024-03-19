using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HP;

public class GeneCHetBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => attackRange;

    [Header("DO NOT modify!!!")]
    public GameObject upPrefab;
    public GameObject downPrefab;
    public Transform thunderPoint;

    [Header("Damage Settings")]
    public float instantDamage = 30f;    // Instant damage applied upon touch.
    private float explosionRange = 5f;

    [Header("Lightning Settings")]
    private float attackInterval = 1.5f;
    public float attackRange = 20f;
    private float lightningDuration = 1f;

    private float nextFireTime = 1.0f;

    private HP selfHP;
    private string targetTag;
    private string targetTag1;
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
            targetTag = "Defender";
            targetTag1 = "Base";
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            defenderController = GetComponent<DefenderController>();
            targetTag = "Enemy";
            targetTag1 = "Enemy";
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
            StartCoroutine(ThunderStrike());
            nextFireTime = Time.time + attackInterval;
            if (!isAttacking)
            {
                StopAllCoroutines();
            }
        }
    }

    private IEnumerator ThunderStrike()
    {
        GameObject upWard = Instantiate(upPrefab, thunderPoint.position, thunderPoint.rotation, transform);
        upWard.GetComponent<SelfDestruction>().waitTime = lightningDuration;
        yield return new WaitForSeconds(lightningDuration);
        // Assuming the upWard's scale is meant to be uniform and independent of the parent's scale
        Vector3 upWardScale = upWard.transform.parent.localScale;

        if (target == null) { yield break; }

        // Calculate the position for downWard. If the target is "Base", adjust to instantiate at its top
        Vector3 downWardPosition = target.transform.position;
        if (target.CompareTag("Base"))
        {
            // Assuming 'model' is the name of the child object containing the mesh renderers
            GameObject model = target.transform.GetChild(0).gameObject;
            Bounds modelBounds = GetCombinedBounds(model);

            // Find a random position at the top of the model
            float randomX = UnityEngine.Random.Range(modelBounds.min.x, modelBounds.max.x);
            float randomZ = UnityEngine.Random.Range(modelBounds.min.z, modelBounds.max.z);
            downWardPosition = new Vector3(randomX, modelBounds.max.y, randomZ);

            // Adjust Y position to ensure the effect visually hits the top surface
            // Creating a raycast that starts above the model and moves downward
            RaycastHit hit;
            if (Physics.Raycast(downWardPosition + Vector3.up * 10, Vector3.down, out hit, Mathf.Infinity))
            {
                // Place downWard effect at the hit point
                downWardPosition.y = hit.point.y;
            }
        }
        GameObject downWard = Instantiate(downPrefab, downWardPosition, target.transform.rotation);
        downWard.GetComponent<SelfDestruction>().waitTime = 1.2f;
        AOETargets(target.transform);
        downWard.transform.localScale = upWardScale;
    }
    public void AOETargets(Transform targetLocation)
    {
        Collider[] hitColliders = Physics.OverlapSphere(targetLocation.position, explosionRange);

        foreach (Collider hitCollider in hitColliders)
        {
            GameObject enemy = hitCollider.gameObject;
            // Check if the GameObject has the correct tag
            if (enemy.CompareTag(targetTag) || enemy.CompareTag(targetTag1))
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                float damageModifier = 1 - (distance / explosionRange);

                DealInstantDamage(enemy, damageModifier);
            }
        }
    }

    private void DealInstantDamage(GameObject target, float damageModifier)
    {
        float damageToDeal = Mathf.Max(instantDamage * damageModifier, instantDamage * 0.5f); // Ensuring minimum 50% damage
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            targetHP.TakeDamage(damageToDeal);
        }
    }

    Bounds GetCombinedBounds(GameObject parentObject)
    {
        MeshRenderer[] meshRenderers = parentObject.GetComponentsInChildren<MeshRenderer>();
        if (meshRenderers.Length == 0) return new Bounds();

        Bounds combinedBounds = meshRenderers[0].bounds;
        for (int i = 1; i < meshRenderers.Length; i++)
        {
            combinedBounds.Encapsulate(meshRenderers[i].bounds);
        }
        return combinedBounds;
    }

}
