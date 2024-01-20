using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LevelManager : MonoBehaviour
{
    public List<Level> gameLevels;
    public Transform[] spawnPoints;
    public List<Dictionary<string, float>> monsterData;

    public float timeBetweenEnemies = 1f;
    public float timeBetweenWaves = 2f;
    public float timeBetweenLevels = 30f;

    private AddBehaviorsToTarget addBehaviorsToTarget;

    private int currentLevelIndex = 0;

    string path = Path.Combine(Application.streamingAssetsPath, "Monster Type.csv");

    void Start()
    {
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
                GameObject spawnedEnemy = Instantiate(enemyInfo.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedEnemy.tag = "Enemy";
                //Add gene behavior script to the spawned enemy
                //addBehaviorsToTarget.AddGeneABehaviors(spawnedEnemy, enemyInfo.geneTypeA, false);
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
        }
        yield return new WaitForSeconds(timeBetweenWaves);
    }
}
