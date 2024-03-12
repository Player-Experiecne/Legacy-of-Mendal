using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float timeDelayBeforeStart = 3f;
    public GameObject breedingButton; //The button into breeding stage
    public PlayerDefenderInventory playerDefenderInventory;
    public int currentLevelIndex = 0;

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
        //Start the game after a delay
        StartCoroutine(TriggerLevelStartAfterDelay(timeDelayBeforeStart));

        GameEvents.OnLevelStart += OnLevelStart;
        GameEvents.OnLevelComplete += CallOnButton;
        GameEvents.OnLevelFail += () => Debug.Log("Game Over");
        GameEvents.OnBreedingStart += () => SceneLoader.LoadScene("Breeding");
        GameEvents.OnBreedingComplete += OnBreedingComplete;
        GameEvents.OnTitleScreen += () => SceneLoader.LoadScene("TitleScreen");
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
    }
    public IEnumerator TriggerLevelStartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Trigger the event OnLevelStart
        GameEvents.TriggerLevelStart();
    }

    private void CallOnButton()
    {
        //currentLevelIndex++; 
        breedingButton.SetActive(true);
    }
}
