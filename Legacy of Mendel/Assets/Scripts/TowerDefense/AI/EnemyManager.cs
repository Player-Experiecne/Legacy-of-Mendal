using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public GameManager gameManager;

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

        // 如果当前没有敌人通知 GameManager更改状态
        if (currentCount == 0 && gameManager != null)
        {
            // 切换状态
            DayNightState newState = gameManager.CurrentState == DayNightState.Day
                ? DayNightState.Night : DayNightState.Day;
            gameManager.ChangeState(newState);
            Debug.Log("State change success");
        }
    }

    private void RefreshUI()
    {
        text.text = currentCount.ToString() + "/" + totalCount.ToString();
    }
}
