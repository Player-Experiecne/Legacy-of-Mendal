using System.Collections;
using UnityEngine;
using static GetInfo;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(GetInfo))]
public class GeneARecBehaviors : MonoBehaviour
{
    [Header("Damage Settings")]
    public float instantDamage = 50f;    // Instant damage applied upon touch.
    public float dotDamage = 20f;         // Damage over time applied while burning.
    public float burnDuration = 5f;      // Duration of the burn effect.
    public float burnTickInterval = 1f;  // Time interval between damage ticks while burning.

    [Header("Fire Ball")]
    [SerializeField] private float fireBallRate = 0.5f;
    [SerializeField] private float fireBallRange = 50f;

    public GameObject fireBallPrefab; // Declare a public GameObject for the fire prefab
    private float nextFireTime = 0.0f;

    private GetInfo getInfo;
    private DefenderController defenderController;
    private EnemyController enemyController;
    private GameObject target;

    private void Awake()
    {
        getInfo = GetComponent<GetInfo>();
        if (getInfo.aiType == AIType.Enemy)
        {
            enemyController = GetComponent<EnemyController>();
        }
        else if (getInfo.aiType == AIType.Defender)
        {
            defenderController = GetComponent<DefenderController>();
        }
        
        fireBallPrefab = Resources.Load<GameObject>("FireBall");
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
        
        if (getInfo.aiType == AIType.Enemy)
        {
            FindClosestDefender();
        }
        else if (getInfo.aiType == AIType.Defender)
        {
            FindClosestEnemy();
        }
        if(target != null) 
        {
            if (Vector3.Distance(transform.position, target.transform.position) < 50)
            {
                Debug.Log(target.name);
                Vector3 spawnPosition = transform.position + transform.forward; // Adjust the offset if necessary
                GameObject fireInstance = Instantiate(fireBallPrefab, spawnPosition, transform.rotation, transform);
                FireBall fireBall = fireInstance.AddComponent<FireBall>();
                fireBall.target = target;
                fireBall.burnDuration = burnDuration;
                fireBall.burnTickInterval = burnTickInterval;
                fireBall.instantDamage = instantDamage;
                fireBall.dotDamage = dotDamage;
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
