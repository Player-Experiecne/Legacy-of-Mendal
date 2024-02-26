using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HP;

public class GeneADomBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => fireRange;

    [Header("DO NOT modify!!!")]
    public float fireRange = 5f;
    public GameObject firePrefab; // Declare a public GameObject for the fire prefab

    [Header("Damage Settings")]
    public float instantDamage = 30f;    // Instant damage applied upon touch.
    public float dotDamage = 5f;         // Damage over time applied while burning.
    public float burnDuration = 3f;      // Duration of the burn effect.
    public float burnTickInterval = 1f;  // Time interval between damage ticks while burning.

    [Header("Fire Settings")]
    public float fireInterval = 2f;
    public float fireDuration = 1f;
    
    private Collider fireTriggerCollider;
    private GameObject firePoint;

    private float nextFireTime = 1f;
    //private float lastDamageTime = 0f;
    private Dictionary<Collider, float> lastDamageTimes = new Dictionary<Collider, float>();

    private HP selfHP;
    private EnemyController enemyController;
    private DefenderController defenderController;
    private bool isAttacking;

    private void Awake()
    {
        selfHP = GetComponent<HP>();
        enemyController = GetComponent<EnemyController>();
        defenderController = GetComponent<DefenderController>();

        Collider[] colliders = GetComponents<Collider>();
        fireTriggerCollider = colliders[1];
        firePoint = transform.GetChild(0).gameObject;

        //get stats
        /*instantDamage = geneTypeAInfoSO.domStats.instantDamage;
        dotDamage = geneTypeAInfoSO.domStats.dotDamage;
        burnDuration = geneTypeAInfoSO.domStats.burnDuration;
        burnTickInterval = geneTypeAInfoSO.domStats.burnTickInterval;

        fireInterval = geneTypeAInfoSO.domStats.fireInterval;
        fireDuration = geneTypeAInfoSO.domStats.fireDuration;
        fireRange = geneTypeAInfoSO.domStats.fireRange;*/
    }

    private void Update()
    {
        if (selfHP.objectType == ObjectType.Enemy)
        {
            isAttacking = enemyController.isAttacking;
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            isAttacking = defenderController.isAttacking;
        }
        if (Time.time > nextFireTime && isAttacking)
        {
            StartCoroutine(SpewFire());
            nextFireTime = Time.time + fireInterval;
        }
    }

    IEnumerator SpewFire()
    {
        fireTriggerCollider.enabled = true;

        // Instantiate the fire prefab at the firepoint
        //Vector3 spawnPosition = transform.position + transform.forward; // Adjust the offset if necessary
        GameObject fireInstance = Instantiate(firePrefab, firePoint.transform.position, transform.rotation, transform);

        yield return new WaitForSeconds(fireDuration);

        Destroy(fireInstance); // Destroy the fire instance
        fireTriggerCollider.enabled = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (selfHP.objectType == ObjectType.Enemy)
        {
            if (other.CompareTag("Base") || other.CompareTag("Defender"))
            {
                // If the target is not in the dictionary, initialize its last damage time
                if (!lastDamageTimes.ContainsKey(other))
                {
                    DealInstantDamage(other.gameObject);
                    SetTargetOnfire(other.gameObject);
                    lastDamageTimes[other] = Time.time;
                }

                // Check if enough time has passed to damage this specific target again
                if (Time.time - lastDamageTimes[other] >= fireInterval)
                {
                    DealInstantDamage(other.gameObject);
                    SetTargetOnfire(other.gameObject);
                    lastDamageTimes[other] = Time.time; // Update the last damage time for this target
                }
            }
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            if (other.CompareTag("Enemy"))
            {
                // If the target is not in the dictionary, initialize its last damage time
                if (!lastDamageTimes.ContainsKey(other))
                {
                    DealInstantDamage(other.gameObject);
                    SetTargetOnfire(other.gameObject);
                    lastDamageTimes[other] = Time.time;
                }

                // Check if enough time has passed to damage this specific target again
                if (Time.time - lastDamageTimes[other] >= fireInterval)
                {
                    DealInstantDamage(other.gameObject);
                    SetTargetOnfire(other.gameObject);
                    lastDamageTimes[other] = Time.time; // Update the last damage time for this target
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (lastDamageTimes.ContainsKey(other))
        {
            lastDamageTimes.Remove(other);
        }
    }


    private void DealInstantDamage(GameObject target)
    {
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            targetHP.TakeDamage(instantDamage);
        }
        //lastDamageTime = Time.time;
    }

    private void SetTargetOnfire(GameObject target)
    {
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            BurningState burningState = target.GetComponent<BurningState>();
            if (burningState == null) // Not burning yet.
            {
                target.AddComponent<BurningState>().StartBurning(dotDamage, burnDuration, burnTickInterval);
            }
            else if (dotDamage > burningState.CurrentBurnDamage) // New damage is stronger.
            {
                burningState.StartBurning(dotDamage, burnDuration, burnTickInterval);
            }
            else if (dotDamage <= burningState.CurrentBurnDamage) // Existing damage is stronger
            {
                burningState.StartBurning(burningState.CurrentBurnDamage, burnDuration, burningState.CurrentBurnTickInterval);
            }
        }
    }
}