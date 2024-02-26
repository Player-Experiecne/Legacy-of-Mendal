using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static HP;

public class GeneBRecBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => attackRange;

    [Header("DO NOT modify!!!")]
    public GameObject tornadoPrefab; // Declare a public GameObject for the fire prefab
    public Transform tornadoPoint;
    public Transform tornadoPoint2; // For tornado option 3
    public Transform tornadoPoint3; // For tornado option 3

    [Header("Tornado Count")]
    public bool spawn3Tornados = false;

    [Header("Damage Settings")]
    public float attackRange = 15f;
    public float instantDamage = 30f;
    public float attackInterval = 2f;

    [Header("Push Back Settings")]
    public float pushDuration = 0.5f;
    public float pushDistance = 5f;

    private float tornadoMoveDistance;
    private float nextAttackTime = 1f;

    private HP selfHP;
    private EnemyController enemyController;
    private DefenderController defenderController;
    private bool isAttacking;

    private void Awake()
    {
        selfHP = GetComponent<HP>();
        enemyController = GetComponent<EnemyController>();
        defenderController = GetComponent<DefenderController>();

        tornadoMoveDistance = attackRange;
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
            SpewTornado();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    private void SpewTornado()
    {
        // Instantiate the tornado prefab at the tornadopoint
        GameObject tornadoInstance = Instantiate(tornadoPrefab, tornadoPoint.transform.position, tornadoPoint.transform.rotation);
        WaterTornado waterTornado = tornadoInstance.AddComponent<WaterTornado>();
        //Assign stats
        waterTornado.instantDamage = instantDamage;
        waterTornado.pushDuration = pushDuration;
        waterTornado.pushDistance = pushDistance;
        waterTornado.tornadoMoveDistance = tornadoMoveDistance;
        waterTornado.selfHP = selfHP;

        if(spawn3Tornados)
        {
            // Instantiate the second tornado prefab at the tornadopoint
            GameObject tornadoInstance2 = Instantiate(tornadoPrefab, tornadoPoint2.transform.position, tornadoPoint2.transform.rotation);
            WaterTornado waterTornado2 = tornadoInstance2.AddComponent<WaterTornado>();
            //Assign stats
            waterTornado2.instantDamage = instantDamage;
            waterTornado2.pushDuration = pushDuration;
            waterTornado2.pushDistance = pushDistance;
            waterTornado2.tornadoMoveDistance = tornadoMoveDistance;
            waterTornado2.selfHP = selfHP;

            // Instantiate the third tornado prefab at the tornadopoint
            GameObject tornadoInstance3 = Instantiate(tornadoPrefab, tornadoPoint3.transform.position, tornadoPoint3.transform.rotation);
            WaterTornado waterTornado3 = tornadoInstance3.AddComponent<WaterTornado>();
            //Assign stats
            waterTornado3.instantDamage = instantDamage;
            waterTornado3.pushDuration = pushDuration;
            waterTornado3.pushDistance = pushDistance;
            waterTornado3.tornadoMoveDistance = tornadoMoveDistance;
            waterTornado3.selfHP = selfHP;
        }
    }
}
