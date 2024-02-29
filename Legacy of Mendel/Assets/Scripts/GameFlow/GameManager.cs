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

        //Register functions into OnLevelComplete event
        GameEvents.OnLevelComplete += CallOnButton;
        GameEvents.OnLevelComplete += IncreaseLevelIndex;

        //Register functions into OnLevelFail event
        //Game over functions
        GameEvents.OnLevelFail += () => Debug.Log("Game Over");

        //Register functions into OnBreedingStart event
        GameEvents.OnBreedingStart += () => SceneLoader.LoadScene("Breeding");

        //Register functions into OnBreedingComplete event
        GameEvents.OnBreedingComplete += () => SceneLoader.LoadScene("TowerDefense");
    }

    public IEnumerator TriggerLevelStartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Trigger the event OnLevelStart
        GameEvents.TriggerLevelStart();
    }
    public void TriggerBreedingStart()
    {
        GameEvents.TriggerBreedingStart();
    }

    private void CallOnButton()
    {
        breedingButton.SetActive(true);
    }

    private void IncreaseLevelIndex()
    {
        currentLevelIndex++;
    } 
}
