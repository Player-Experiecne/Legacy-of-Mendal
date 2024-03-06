using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static HP;

public class GeneCDomBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => attackRange;

    [Header("DO NOT modify!!!")]
    public GameObject lightOrbPrefab; // Declare a public GameObject for the fire prefab
    public Transform lightOrbPoint;

    [Header("Damage Settings")]
    public float attackRange = 15f;
    public float instantDamage = 30f;
    public float attackInterval = 2f;

    private float nextAttackTime = 1f;

    private HP selfHP;
    private EnemyController enemyController;
    private DefenderController defenderController;
    private GameObject target;
    private bool isAttacking;

    private void Awake()
    {
        selfHP = GetComponent<HP>();
        enemyController = GetComponent<EnemyController>();
        defenderController = GetComponent<DefenderController>();
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
        if (Time.time > nextAttackTime && isAttacking)
        {
            SpewLightOrb();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    private void SpewLightOrb()
    {
        if (selfHP.objectType == ObjectType.Enemy)
        {
            target = enemyController.targetDefender;
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            target = defenderController.targetEnemy;
        }
        // Instantiate the tornado prefab at the tornadopoint
        GameObject lightOrbInstance = Instantiate(lightOrbPrefab, lightOrbPoint.transform.position, lightOrbPoint.transform.rotation);
        LightOrbGroup lightOrb = lightOrbInstance.AddComponent<LightOrbGroup>();
        //Assign stats
        lightOrb.instantDamage = instantDamage;
        lightOrb.moveDistance = attackRange;
        lightOrb.selfHP = selfHP;
        lightOrb.target = target;
    }
}
