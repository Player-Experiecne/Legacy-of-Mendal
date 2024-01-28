using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public List<Level> gameLevels;
    public Transform[] spawnPoints;
    public List<Dictionary<string, float>> monsterData;

    public float timeBetweenWaves = 2f;
    public float timeBetweenLevels = 30f;

    public AddBehaviorsToTarget addBehaviorsToTarget;

    private int currentLevelIndex = 0;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        addBehaviorsToTarget = GetComponent<AddBehaviorsToTarget>();

        LoadNextLevel();
    }


    private void LoadNextLevel()
    {
        if (currentLevelIndex < gameLevels.Count)
        {
            Level currentLevel = gameLevels[currentLevelIndex];
            Debug.Log("Starting Level: " + currentLevel.LevelName);

            StartCoroutine(SpawnWaves(currentLevel.Waves));
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    private IEnumerator SpawnWaves(List<Level.Wave> waves)
    {
        foreach (var wave in waves)
        {
            yield return StartCoroutine(SpawnEnemies(wave.enemies));
        }

        yield return new WaitForSeconds(timeBetweenLevels);
        currentLevelIndex++;
        LoadNextLevel();
    }

    private IEnumerator SpawnEnemies(List<Level.EnemySpawnInfo> enemies)
    {
        foreach (var enemyInfo in enemies)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                Transform spawnPoint = spawnPoints[enemyInfo.spawnLocation];
                GameObject spawnedEnemy = Instantiate(enemyInfo.enemy.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedEnemy.tag = "Enemy";
                AssignProperties(enemyInfo.enemy, spawnedEnemy);
                yield return null;
            }
        }
        yield return new WaitForSeconds(timeBetweenWaves);
    }

    private void AssignProperties(Enemy enemy, GameObject spawnedEnemy)
    {
        EnemyController enemyController = spawnedEnemy.GetComponent<EnemyController>();
        NavMeshAgent navMeshAgent = spawnedEnemy.GetComponent<NavMeshAgent>();
        HP hp = spawnedEnemy.GetComponent<HP>();

        //Assign stats
        if(enemy.hp != 0)
        {
            hp.maxHealth = enemy.hp;
        }
        if (enemy.attackPower != 0)
        {
            enemyController.attackPower = enemy.attackPower;
        }
        /*if (enemy.attackRange != 0)
        {
            enemyController.attackRange = enemy.attackRange;
        }*/
        if (enemy.attackSpeed != 0)
        {
            enemyController.attackSpeed = enemy.attackSpeed;
        }
        if (enemy.speed != 0)
        {
            navMeshAgent.speed = enemy.speed;
        }

        //Add gene behavior script to the spawned enemy
        addBehaviorsToTarget.AddGeneABehaviors(spawnedEnemy, enemy.geneTypeA, false);

        //Assign lootgenes
        foreach (LootGeneType lootGeneType in enemy.lootGeneTypes)
        {
            if (lootGeneType != null)
            {
                float randomValue = Random.Range(0f, 1f);
                if (randomValue <= lootGeneType.probability)
                {
                    enemyController.lootGeneTypes.Add(lootGeneType.geneType);
                }
            }
        }

        //Assign loot culture medium
        float randomValue1 = Random.Range(0f, 1f);
        if (randomValue1 <= enemy.lootCultureMedium.lootCultureMediumprobability)
        {
            enemyController.lootCultureMedium = Random.Range(enemy.lootCultureMedium.minLootCultureMedium, enemy.lootCultureMedium.maxLootCultureMedium + 1);
        }
    }
}
