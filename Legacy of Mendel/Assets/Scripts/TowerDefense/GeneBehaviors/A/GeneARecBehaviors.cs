using System.Collections;
using UnityEngine;
using static HP;

public class GeneARecBehaviors : MonoBehaviour
{
    //Damage Settings
    private float instantDamage;    // Instant damage applied upon touch.
    private float dotDamage;         // Damage over time applied while burning.
    private float burnDuration;      // Duration of the burn effect.
    private float burnTickInterval;  // Time interval between damage ticks while burning.

    //Fire Ball
    private float fireBallRate;
    private float fireBallRange;
    private float explosionRange;
    private GameObject fireBallPrefab;
    private GameObject fireBallPrefabForEnemies; // Declare a public GameObject for the fire prefab
    private GameObject fireBallPrefabForDefenders; // Declare a public GameObject for the fire prefab
    
    private float nextFireTime = 1.0f;

    private LevelManager levelManager;
    private GeneTypeAInfoSO geneTypeAInfoSO;
    private HP selfHP;
    private GameObject target;

    private void Awake()
    {
        selfHP = GetComponent<HP>();
        levelManager = LevelManager.Instance;
        geneTypeAInfoSO = levelManager.addBehaviorsToTarget.geneTypeAInfo;

        instantDamage = geneTypeAInfoSO.recStats.instantDamage;
        dotDamage = geneTypeAInfoSO.recStats.dotDamage;
        burnDuration = geneTypeAInfoSO.recStats.burnDuration;
        burnTickInterval = geneTypeAInfoSO.recStats.burnTickInterval;
        fireBallRate = geneTypeAInfoSO.recStats.fireBallRate;
        fireBallRange = geneTypeAInfoSO.recStats.fireBallRange;
        explosionRange = geneTypeAInfoSO.recStats.explosionRange;
        fireBallPrefabForEnemies = geneTypeAInfoSO.recStats.fireBallPrefabForEnemies;
        fireBallPrefabForDefenders = geneTypeAInfoSO.recStats.fireBallPrefabForDefenders;
    }

    private void Update()
    {
        if (Time.time > nextFireTime)
        {
            StartCoroutine(LaunchFireBall());
            nextFireTime = Time.time + 1f / fireBallRate;
        }
    }

    IEnumerator LaunchFireBall()
    {
        
        if (selfHP.objectType == ObjectType.Enemy)
        {
            FindClosestDefender();
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            FindClosestEnemy();
        }
        if(target != null) 
        {
            if (Vector3.Distance(transform.position, target.transform.position) < 50)
            {
                Vector3 spawnPosition = transform.position + transform.forward; // Adjust the offset if necessary
                GameObject fireBallPrefab = new GameObject();
                if (selfHP.objectType == ObjectType.Enemy)
                {
                    fireBallPrefab = fireBallPrefabForEnemies;
                }
                else if (selfHP.objectType == ObjectType.Defender)
                {
                    fireBallPrefab = fireBallPrefabForDefenders;
                }
                GameObject fireInstance = Instantiate(fireBallPrefab, spawnPosition, transform.rotation);
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
        yield return null;
    }

    private void FindClosestDefender()
    {
        float closestDistance = fireBallRange;
        GameObject closestDefender = null;
        foreach (GameObject defender in DefenderManager.Instance.defenders)
        {
            float currentDistance = Vector3.Distance(transform.position, defender.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestDefender = defender;
            }
        }
        target = closestDefender;
    }

    private void FindClosestEnemy()
    {
        float closestDistance = fireBallRange;
        GameObject closestEnemy = null;
        foreach (GameObject enemy in EnemyManager.Instance.enemies)
        {
            float currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestEnemy = enemy;
            }
        }
        target = closestEnemy;
    }
}
