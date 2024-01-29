using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LevelManager levelManager;
    public EnemyManager enemyManager;

    public DayNightState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentState = DayNightState.Day; // 初始状态为白天
        StartNewLevel(); // 开始第一关
    }

    public void StartNewLevel()
    {
        if (currentState == DayNightState.Night)
        {
            currentState = DayNightState.Day; // 转换到白天
            levelManager.LoadNextLevel(); // 加载下一个关卡
        }
    }

    public void OnEnemiesCleared()
    {
        // 当所有敌人被消灭时，转换到黑夜，开始培育阶段
        currentState = DayNightState.Night;
        // 这里可以调用开始培育的逻辑，例如显示培育界面
    }

    // 调用此方法以结束培育阶段，并准备开始下一关卡
    public void CompleteBreeding()
    {
        currentState = DayNightState.Night; // 准备转换到白天
        // 可以在这里添加培育结束的逻辑
    }
}


