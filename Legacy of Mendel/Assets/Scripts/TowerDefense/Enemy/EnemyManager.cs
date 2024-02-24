using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EnemyManager : MonoBehaviour, IEnemyManager
{
    public static IEnemyManager Instance;

    public GameManager gameManager;

    private List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> Enemies => enemies;

    public GameObject breedingButton;

    public TextMeshProUGUI text;
    private int currentCount = 0;
    private int totalCount = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if ((Object)Instance != (Object)this)
        {
            Destroy(gameObject);
        }
        RefreshUI();
    }
    private void Start()
    {
        breedingButton.SetActive(false);
        
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

        // 如果当前没有敌人通知 GameManager 更改状态
        if (currentCount == 0 && gameManager != null)
        {
            breedingButton.SetActive(true);
        }
    }
    public void ResetEnemyCount()
    {
        currentCount = 0;
        totalCount = 0;
        enemies.Clear(); 
        RefreshUI();
    }

    private void RefreshUI()
    {
        text.text = currentCount.ToString() + "/" + totalCount.ToString();
    }
}
