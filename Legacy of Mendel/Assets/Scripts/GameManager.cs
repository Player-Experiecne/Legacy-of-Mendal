using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LevelManager levelManager;
    public LootBackpack lootBackpack;

    public GameObject breedingUI; // 培育界面的UI对象

    private int currentLevelIndex = 0;

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
        // 隐藏培育UI
        if (breedingUI != null)
        {
            breedingUI.SetActive(false);
        }

      
    }

    void Update()
    {
        
    }

    /*private void CheckLevelCompletion()
    {
        if (currentLevelIndex != levelManager.CurrentLevelIndex)
        {
            currentLevelIndex = levelManager.CurrentLevelIndex;
            if (currentLevelIndex >= levelManager.gameLevels.Count)
            {
               
            }
            else
            {
                EnterBreedingPhase();
            }
        }
    }*/

    public void EnterBreedingPhase()
    {
       
        FindObjectOfType<BreedManager>().StartBreedingPhase(); // 启动培育阶段
                                                               
        breedingUI.SetActive(true);
    }

    public void ExitBreedingPhase()
    {
        breedingUI.SetActive(false); // 隐藏培育界面
        levelManager.LoadNextLevel(); // 加载下一关
    }
}
