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

    public float timeBetweenWaves = 2f;
    public float timeBetweenEnemies = 0.5f;

    private bool levelCompleted = true;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public bool LevelCompleted
    {
        get { return levelCompleted; }
    }

    public void StartCurrentLevel()
    {
        LoadLevelWithIndex(GameManager.Instance.currentLevelIndex);
    }
    

    private void LoadLevelWithIndex(int index)
    {
        if (index < gameLevels.Count)
        {
            EnemyManager.Instance.ResetEnemyCount();
            Level currentLevel = gameLevels[index];
            Debug.Log("Starting Level: " + currentLevel.LevelName);
            levelCompleted = false;
            StartCoroutine(SpawnWaves(currentLevel.Waves));
        }
    }

    private IEnumerator SpawnWaves(List<Level.Wave> waves)
    {
        List<Coroutine> waveCoroutines = new List<Coroutine>();

        for (int w = 0; w < waves.Count; w++)
        {
            Coroutine waveCoroutine = StartCoroutine(SpawnEnemies(waves[w].enemies));
            waveCoroutines.Add(waveCoroutine);

            // Wait if it's not the last wave
            if (w < waves.Count - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        // Wait for all waves to complete
        foreach (var coroutine in waveCoroutines)
        {
            yield return coroutine;
        }

        levelCompleted = true;
    }


    private IEnumerator SpawnEnemies(List<Level.EnemySpawnInfo> enemies)
    {
        for (int e = 0; e < enemies.Count; e++)
        {
            var enemyInfo = enemies[e];
            for (int i = 0; i < enemyInfo.count; i++)
            {
                Transform spawnPoint = spawnPoints[enemyInfo.spawnLocation];
                GameObject spawnedEnemy = Instantiate(enemyInfo.enemy.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedEnemy.tag = "Enemy";
                AssignProperties(enemyInfo.enemy, spawnedEnemy);

                // Wait if it's not the last enemy of the last spawn info
                if (e < enemies.Count - 1 || i < enemyInfo.count - 1)
                {
                    yield return new WaitForSeconds(timeBetweenEnemies);
                }
            }
        }
    }

    private void AssignProperties(Enemy enemy, GameObject spawnedEnemy)
    {
        EnemyController enemyController = spawnedEnemy.GetComponent<EnemyController>();
        //NavMeshAgent navMeshAgent = spawnedEnemy.GetComponent<NavMeshAgent>();
        //HP hp = spawnedEnemy.GetComponent<HP>();

        //Assign stats
        /*if(enemy.hp != 0)
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
        }
        if (enemy.attackSpeed != 0)
        {
            enemyController.attackSpeed = enemy.attackSpeed;
        }
        if (enemy.speed != 0)
        {
            navMeshAgent.speed = enemy.speed;
        }

        //Add gene behavior script to the spawned enemy
        addBehaviorsToTarget.AddGeneBehaviors(spawnedEnemy, enemy.geneTypes, false);*/

        //Assign lootgenes
        if (enemy.lootGeneType != null && (enemy.lootGeneType.geneType.geneType != GeneInfo.geneTypes.Null))
        {
            if(Random.value < enemy.lootGeneType.lootPossibility)
            {
                enemyController.lootGeneType = enemy.lootGeneType.geneType;
            }
        }
        //Assign loot culture medium
        enemyController.lootCultureMedium = Random.Range(enemy.lootCultureMedium.minLootCultureMedium, enemy.lootCultureMedium.maxLootCultureMedium + 1);
    }

    // 在 LevelManager 中，当关卡结束时调用
    public void OnLevelEnd(bool isSuccess)
    {
        if (isSuccess)
        {
            // Level completed successfully
            GameEvents.TriggerLevelComplete();
        }
        else
        {
            // Level failed
            GameEvents.TriggerLevelFail();
        }
    }


}
