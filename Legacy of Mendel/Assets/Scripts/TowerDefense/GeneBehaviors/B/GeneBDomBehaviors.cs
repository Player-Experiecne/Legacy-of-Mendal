using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HP;

public class GeneBDomBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => attackRange;

    //Damage Settings
    private float attackRange = 9f;
    public float instantDamage = 30f;
    public float attackInterval = 2f;
    private float attackDuration = 1.5f;

    //Slow Settings
    public float slowRatio = 0.2f;
    public float slowDuration = 4f;

    //Instantiation Settings
    public GameObject icePrefab; // Declare a public GameObject for the fire prefab
    public Transform icePoint;
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
            StartCoroutine(SpewIce());
            nextAttackTime = Time.time + attackInterval;
        }
    }

    IEnumerator SpewIce()
    {
        // Instantiate the ice prefab at the icepoint
        iceInstance = Instantiate(icePrefab, icePoint.transform.position, icePoint.transform.rotation);
        iceAttack = iceInstance.transform.GetChild(0).gameObject.AddComponent<IceAttack>();
        //Assign stats
        iceAttack.instantDamage = instantDamage;
        iceAttack.slowRatio = slowRatio;
        iceAttack.slowDuration = slowDuration;
        iceAttack.selfHP = selfHP;

        //Wait for attack duration and then stop the ice attack
        yield return new WaitForSeconds(attackDuration);
        Destroy(iceInstance); // Destroy the ice instance
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
