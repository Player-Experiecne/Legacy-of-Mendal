using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HP;

public class GeneADomBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => fireRange;
    
    //Damage Settings
    private float instantDamage;    // Instant damage applied upon touch.
    private float dotDamage;         // Damage over time applied while burning.
    private float burnDuration;      // Duration of the burn effect.
    private float burnTickInterval;  // Time interval between damage ticks while burning.

    //Fire
    public float fireInterval;
    public float fireDuration;
    private float fireRange;
    
    private Collider fireTriggerCollider;
    private GameObject firePrefab; // Declare a public GameObject for the fire prefab
    private GameObject firePoint;

    private string targetTag;
    private float nextFireTime = 1f;
    //private float lastDamageTime = 0f;
    private Dictionary<Collider, float> lastDamageTimes = new Dictionary<Collider, float>();


    private LevelManager levelManager;
    private GeneTypeAInfoSO geneTypeAInfoSO;
    private HP selfHP;
    private EnemyController enemyController;
    private DefenderController defenderController;
    private bool isAttacking;

    private void Awake()
    {
        selfHP = GetComponent<HP>();
        levelManager = LevelManager.Instance;
        geneTypeAInfoSO = levelManager.addBehaviorsToTarget.geneTypeAInfo;

        if (selfHP.objectType == ObjectType.Enemy)
        {
            targetTag = "Defender";
            enemyController = GetComponent<EnemyController>();
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            targetTag = "Enemy";
            defenderController = GetComponent<DefenderController>();
        }

        Collider[] colliders = GetComponents<Collider>();
        fireTriggerCollider = colliders[1];
        firePoint = transform.GetChild(0).gameObject;

        //get stats
        firePrefab = geneTypeAInfoSO.domStats.firePrefab;
        instantDamage = geneTypeAInfoSO.domStats.instantDamage;
        dotDamage = geneTypeAInfoSO.domStats.dotDamage;
        burnDuration = geneTypeAInfoSO.domStats.burnDuration;
        burnTickInterval = geneTypeAInfoSO.domStats.burnTickInterval;

        fireInterval = geneTypeAInfoSO.domStats.fireInterval;
        fireDuration = geneTypeAInfoSO.domStats.fireDuration;
        fireRange = geneTypeAInfoSO.domStats.fireRange;
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
        Debug.Log("3");
        if (other.transform.gameObject.CompareTag(targetTag))
        {
            // If the target is not in the dictionary, initialize its last damage time
            if (!lastDamageTimes.ContainsKey(other))
            {
                Debug.Log("1");
                DealInstantDamage(other.gameObject);
                SetTargetOnfire(other.gameObject); 
                lastDamageTimes[other] = Time.time;
            }

            // Check if enough time has passed to damage this specific target again
            if (Time.time - lastDamageTimes[other] >= fireInterval)
            {
                Debug.Log("2");
                DealInstantDamage(other.gameObject);
                SetTargetOnfire(other.gameObject);
                lastDamageTimes[other] = Time.time; // Update the last damage time for this target
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
        Debug.Log("instant damage");
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
                burningState.RefreshBurning(dotDamage, burnDuration);
            }
            else if (dotDamage <= burningState.CurrentBurnDamage)
            {
                burningState.RefreshBurning(burningState.CurrentBurnDamage, burnDuration);
            }
        }
    }
}