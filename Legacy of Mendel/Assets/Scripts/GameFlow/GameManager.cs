using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float timeDelayBeforeStart = 3f;

    [Header("DO NOT modify!!!")]
    public GameObject victoryUI; //The button into breeding stage
    public GameObject breedingButton;
    public GameObject continueButton;
    public GameObject gameOverScreen; 
    public LoadingScreen loadingScreen;
    public PlayerDefenderInventory playerDefenderInventory;
    public CutsceneManager cutsceneManager;

    [HideInInspector]public int currentLevelIndex = 0;
    [HideInInspector]public bool isTitleScreen = true;

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
        GameEvents.OnEnemySpawn += OnEnemySpawn;
        GameEvents.OnLevelComplete += OnLevelComplete;
        GameEvents.OnLevelFail += () => Debug.Log("Game Over");
        GameEvents.OnBreedingStart += () => loadingScreen.LoadScene("Breeding");
        GameEvents.OnBreedingComplete += OnBreedingComplete;
        GameEvents.OnTitleScreen += OnTitleScreen;
        GameEvents.OnTowerDefense += OnTowerDefense;
        GameEvents.OnLevelFail += OnLevelFail;
        GameEvents.OnTutorialStart += OnTutorialStart;
        GameEvents.OnTutorialEnd += OnTutorialEnd;
    }

    public void OnBreedingButtonClicked()
    {
        GameEvents.TriggerBreedingStart(); 
        victoryUI.SetActive(false);
    }

    private void OnEnemySpawn()
    {
        LevelManager.Instance.StartCurrentLevel();
    }

    public void OnBreedingComplete()
    {
        currentLevelIndex++; // Move to the next level
        loadingScreen.LoadScene("TowerDefense"); // Load the tower defense scene for the next level
    }

    void OnDestroy()
    {
        // 取消注册事件
        GameEvents.OnBreedingComplete -= OnBreedingComplete;
        GameEvents.OnTutorialStart -= OnTutorialStart;
        GameEvents.OnTutorialEnd -= OnTutorialEnd;
    }

    public IEnumerator TriggerEnemySpawnAfterDelay()
    {
        yield return new WaitForSeconds(timeDelayBeforeStart);
        //Trigger the event OnEnemySpawn
        GameEvents.TriggerEnemySpawn();
    }

    public IEnumerator TriggerTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
       
        GameEvents.TriggerTutorialStart();
    }

    public void OnTutorialStart()
    {
        loadingScreen.LoadScene("Tutorial");
        //StartCoroutine(TriggerTutorialAfterDelay(timeDelayBeforeStart));
    }

    public void OnTutorialEnd()
    {
        
    }

    private void OnLevelComplete()
    {
        victoryUI.SetActive(true);
        if (currentLevelIndex == 4) 
        {
            breedingButton.SetActive(false);
            continueButton.SetActive(true);
        }
    }

    private void OnTowerDefense()
    {
        loadingScreen.LoadScene("TowerDefense");
        currentLevelIndex = 0;
    }

    private void OnTitleScreen()
    {
        StopAllCoroutines();
        loadingScreen.LoadScene("TitleScreen");
        gameOverScreen.SetActive(false);
        victoryUI.SetActive(false);
        continueButton.SetActive(false);
        breedingButton.SetActive(true);
        if (LevelManager.Instance != null) { Destroy(LevelManager.Instance.gameObject); }
        if (DefenderBackpack.Instance != null) { Destroy(DefenderBackpack.Instance.gameObject); }
        if (SummonerSkillManager.Instance != null) { Destroy(SummonerSkillManager.Instance.gameObject); }
        if (LootBackpack.Instance != null) { Destroy(LootBackpack.Instance.gameObject); }
    }

    private void OnLevelFail()
    {
        gameOverScreen.SetActive(true);
    }

    public static void TriggerTowerDefense()
    {
        GameEvents.TriggerTowerDefense();
    }

    public static void TriggerTitleScreen()
    {
        GameEvents.TriggerTitleScreen();
    }

    public static void TriggerTutorial()
    {
        GameEvents.TriggerTutorialStart();
    }


}
