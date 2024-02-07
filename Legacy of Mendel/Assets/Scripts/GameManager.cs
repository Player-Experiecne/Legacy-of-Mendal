using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LevelManager levelManager;
    public LootBackpack lootBackpack;

    public ActionBackpack backpack;

    public GameObject breedingUI; // 培育界面的UI对象

    public PlayerDefenderInventory playerDefenderInventory;

    public BreedManager breedManager;

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

    void DestroyAllWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objectsWithTag)
        {
            Destroy(obj);
        }
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

        DestroyAllWithTag("Loot");

        DestroyAllWithTag("Defender");

        backpack.ClearBackpack();
    }



    public void ExitBreedingPhase()
    {
        breedingUI.SetActive(false); // 隐藏培育界面
        foreach (GameObject uiElement in breedManager.hiddenUIs)
        {
            uiElement.SetActive(true);
        }
        levelManager.LoadNextLevel(); // 加载下一关
        backpack.AddDefendersFromInventory(playerDefenderInventory);
    }
}
