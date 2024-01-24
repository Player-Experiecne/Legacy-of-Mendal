using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<GameObject> enemies = new List<GameObject>();

    public TextMeshProUGUI text;
    private int currentCount = 0;
    private int totalCount = 0;


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
        RefreshUI();
    }

    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);

        totalCount++;
        currentCount++;
        RefreshUI();
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);

        currentCount--;
        RefreshUI();
    }

    private void RefreshUI()
    {
        text.text = currentCount.ToString() + "/" + totalCount.ToString();
    }
}
