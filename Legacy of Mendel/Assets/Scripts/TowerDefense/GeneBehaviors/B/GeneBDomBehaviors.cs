using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HP;

public class GeneBDomBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => attackRange;

    //Damage Settings
    public float attackRange = 5f;
    public float damagePerHit = 10f;    // Instant damage applied upon touch.
    public float damageInterval = 0.5f;

    //Slow Settings
    public float slowRatio = 0.2f;
    public float slowDuration = 4f;

    [SerializeField] private GameObject waterObject; // Declare the chile GameObject

    private HP selfHP;
    private EnemyController enemyController;
    private DefenderController defenderController;
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
        if (isAttacking)
        {
            SpewWater();
        }
        else if (!isAttacking)
        {
            StopWater();
        }
    }

    private void SpewWater()
    {
        if (waterObject.activeSelf) { return; }

        waterObject.SetActive(true);
        WaterAttack waterAttack = waterObject.GetComponent<WaterAttack>();

        //Assign stats
        waterAttack.damagePerHit = damagePerHit;
        waterAttack.damageInterval = damageInterval;
        waterAttack.slowRatio = slowRatio;
        waterAttack.slowDuration = slowDuration;
        waterAttack.selfHP = selfHP;
    }

    private void StopWater()
    {
        if (!waterObject.activeSelf) { return; }
        waterObject.SetActive(false);
    }
}
