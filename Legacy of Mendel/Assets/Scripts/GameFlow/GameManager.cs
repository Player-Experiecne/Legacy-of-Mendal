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
        GameEvents.OnLevelStart += OnLevelStart;
        GameEvents.OnLevelComplete += CallOnButton;
        GameEvents.OnLevelFail += () => Debug.Log("Game Over");
        GameEvents.OnBreedingStart += () => SceneLoader.LoadScene("Breeding");
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
    private void OnLevelStart()
    {
        LevelManager.Instance.StartCurrentLevel();
    }

    public void OnBreedingComplete()
    {
        currentLevelIndex++; // Move to the next level
        SceneLoader.LoadScene("TowerDefense"); // Load the tower defense scene for the next level
        StartCoroutine(TriggerLevelStartAfterDelay(timeDelayBeforeStart));
         // Ensure this method is used to start levels
    }

    void OnDestroy()
    {
        // 取消注册事件
        GameEvents.OnBreedingComplete -= OnBreedingComplete;
        GameEvents.OnTutorialStart -= OnTutorialStart;
        GameEvents.OnTutorialEnd -= OnTutorialEnd;
    }
    public IEnumerator TriggerLevelStartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Trigger the event OnLevelStart
        GameEvents.TriggerLevelStart();
    }


    public IEnumerator TriggerTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
       
        GameEvents.TriggerTutorialStart();
    }
    public void OnTutorialStart()
    {
        SceneLoader.LoadScene("Tutorial");
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
        SceneLoader.LoadScene("Tutorial");
        isTitleScreen = false;
        currentLevelIndex = 0;
        StartCoroutine(TriggerLevelStartAfterDelay(timeDelayBeforeStart));
    }

    private void OnTitleScreen()
    {
        SceneLoader.LoadScene("TitleScreen");
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


}
