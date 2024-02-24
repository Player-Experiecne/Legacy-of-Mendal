using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level", order = 51)]
public class Level : ScriptableObject
{
    [SerializeField] private string levelName;
    [SerializeField] private List<Wave> waves;

    public string LevelName => levelName;
    public List<Wave> Waves => waves;
    public void AddWave(Wave wave)
    {
        if (waves == null)
        {
            waves = new List<Wave>();
        }
        waves.Add(wave);
    }

    [System.Serializable]
    public class Wave
    {
        [SerializeField] public List<EnemySpawnInfo> enemies;
        public void AddEnemy(EnemySpawnInfo enemy)
        {
            if (enemies == null)
            {
                enemies = new List<EnemySpawnInfo>();
            }
            enemies.Add(enemy);
        }
    }

    [System.Serializable]
    public class EnemySpawnInfo
    {
        public Enemy enemy;
        public int spawnLocation;
        public int count;
    }

    
}
