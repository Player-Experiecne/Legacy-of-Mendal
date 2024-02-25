using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneBHetBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => attackRange;

    //Damage Settings
    private float attackRange = 5f;
    public float damagePerHit = 10f;    // Instant damage applied upon touch.
    public float damageInterval = 0.5f;

    //Slow Settings
    public float slowRatio = 0.2f;
    public float slowDuration = 4f;

    private Collider triggerCollider;
    public GameObject waterPrefab; // Declare a public GameObject for the fire prefab
    private GameObject waterPoint;

    private float nextFireTime = 1f;
    //private float lastDamageTime = 0f;
    private Dictionary<Collider, float> lastDamageTimes = new Dictionary<Collider, float>();

    private HP selfHP;
    private EnemyController enemyController;
    private DefenderController defenderController;
    private bool isAttacking;
}
