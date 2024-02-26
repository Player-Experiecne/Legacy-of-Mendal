using System.Collections;
using UnityEngine;
using static HP;

public class RangeAutoAttack : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => attackRange;

    [Header("DO NOT modify!!!")]
    public GameObject bulletPrefab;

    [Header("Damage Settings")]
    public float instantDamage = 10f;    // Instant damage applied upon touch.

    [Header("Attack Settings")]
    public float attackInterval = 1f;
    public float attackRange = 8;
    private GameObject firePoint;

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

        //get stats
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
            StartCoroutine(Attack());
            nextFireTime = Time.time + attackInterval;
        }
    }

    IEnumerator Attack()
    {

        if (selfHP.objectType == ObjectType.Enemy)
        {
            target = enemyController.targetDefender;
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            target = defenderController.targetEnemy;
        }
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
            {
                GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.transform.position, transform.rotation);
                Bullet bullet = bulletInstance.AddComponent<Bullet>();
                bullet.attackTarget = target;
                bullet.instantDamage = instantDamage;
            }
        }
        yield return null;
    }
}
