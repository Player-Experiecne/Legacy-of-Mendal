using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HP;

public class GeneBHetBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => attackRange;

    [Header("DO NOT modify!!!")]
    public GameObject icePrefab; // Declare a public GameObject for the fire prefab
    public Transform icePoint;

    [Header("Damage Settings")]
    public float instantDamage = 30f;
    public float attackInterval = 2f;
    private float attackDuration = 1.5f;
    private float attackRange = 9f;

    [Header("Freeze Settings")]
    public float freezeDuration = 1.5f;

    private IceAttack iceAttack;

    private float nextAttackTime = 1f;
    private GameObject iceInstance;

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
        if (Time.time > nextAttackTime && isAttacking)
        {
            SpewIce();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    private void SpewIce()
    {
        // Instantiate the ice prefab at the icepoint
        iceInstance = Instantiate(icePrefab, icePoint.transform.position, icePoint.transform.rotation);
        iceAttack = iceInstance.transform.GetChild(0).gameObject.AddComponent<IceAttack>();
        //Assign stats
        iceAttack.instantDamage = instantDamage;
        iceAttack.attackDuration = attackDuration;
        iceAttack.freezeDuration = freezeDuration;
        iceAttack.selfHP = selfHP;
    }

    //Changed this old way of detection to trigger collider on ice attack prefab
    /*private void DetectEnemiesInIce()
    {
        if (!isDetectorOn) { return; }
        else
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
                float angleBetween = Vector3.Angle(transform.forward, directionToTarget);

                if (angleBetween < attackAngle)
                {
                    if (HitTargetOrNot(hitCollider))
                    {
                        DealInstantDamage(hitCollider.gameObject);
                    }
                }
            }
        }
    }*/

    
}
