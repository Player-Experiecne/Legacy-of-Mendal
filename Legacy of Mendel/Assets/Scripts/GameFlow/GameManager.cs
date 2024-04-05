using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float timeDelayBeforeStart = 3f;
    public GameObject breedingButton; //The button into breeding stage
    public GameObject gameOverScreen; 
    public LoadingScreen loadingScreen;
    public PlayerDefenderInventory playerDefenderInventory;
    public int currentLevelIndex = 0;
    public bool isTitleScreen = true;

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
        GameEvents.OnLevelComplete += CallOnButton;
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
        breedingButton.SetActive(false);
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
        isTitleScreen = false;
        //StartCoroutine(TriggerTutorialAfterDelay(timeDelayBeforeStart));
    }

    public void OnTutorialEnd()
    {
        
    }

    private void CallOnButton()
    {
        //currentLevelIndex++; 
        breedingButton.SetActive(true);
    }

    private void OnTowerDefense()
    {
        loadingScreen.LoadScene("TowerDefense");
        isTitleScreen = false;
        currentLevelIndex = 0;
    }

    private void OnTitleScreen()
    {
        loadingScreen.LoadScene("TitleScreen");
        isTitleScreen = true;
        gameOverScreen.SetActive(false);
        breedingButton.SetActive(false);
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
