using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
