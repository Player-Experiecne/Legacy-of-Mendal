using System.Collections;
using UnityEngine;
using static HP;

public class GeneARecBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => fireBallRange;

    [Header("DO NOT modify!!!")]
    public GameObject fireBallPrefabForEnemies; // Declare a public GameObject for the fire prefab
    public GameObject fireBallPrefabForDefenders; // Declare a public GameObject for the fire prefab

    [Header("Damage Settings")]
    public float instantDamage = 30f;    // Instant damage applied upon touch.
    public float dotDamage = 5f;         // Damage over time applied while burning.
    public float burnDuration = 3f;      // Duration of the burn effect.
    public float burnTickInterval = 1f;  // Time interval between damage ticks while burning.

    [Header("Attack Settings")]
    public float fireBallInterval = 1f;
    public float fireBallRange = 20f;
    public float explosionRange = 5f;
    private GameObject firePoint;
    private GameObject fireBallPrefab;
    
    private float nextFireTime = 1.0f;

    private HP selfHP;
    [HideInInspector]public GameObject target;
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
        firePoint = transform.GetChild(0).gameObject;
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
            LaunchFireBall();
            nextFireTime = Time.time + fireBallInterval;
        }
    }

    private void LaunchFireBall()
    {
        if (selfHP.objectType == ObjectType.Enemy)
        {
            target = enemyController.targetDefender;
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            target = defenderController.targetEnemy;
        }
        if(target != null) 
        {
            //Vector3 spawnPosition = transform.position + transform.forward; // Adjust the offset if necessary
            if (selfHP.objectType == ObjectType.Enemy)
            {
                fireBallPrefab = fireBallPrefabForEnemies;
            }
            else if (selfHP.objectType == ObjectType.Defender)
            {
                fireBallPrefab = fireBallPrefabForDefenders;
            }
            GameObject fireInstance = Instantiate(fireBallPrefab, firePoint.transform.position, transform.rotation);
            FireBall fireBall = fireInstance.AddComponent<FireBall>();
            fireBall.objectType = selfHP.objectType;
            fireBall.fireBallTarget = target;
            fireBall.burnDuration = burnDuration;
            fireBall.burnTickInterval = burnTickInterval;
            fireBall.instantDamage = instantDamage;
            fireBall.dotDamage = dotDamage;
            fireBall.explosionRange = explosionRange;
        }
    }
}
