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

        GameEvents.OnLevelStart += OnLevelStart;
        GameEvents.OnBreedingComplete += OnBreedingComplete;

        //Register functions into OnLevelFail event
        //Game over functions
        GameEvents.OnLevelFail += () => Debug.Log("Game Over");

        //Register functions into OnBreedingStart event
        GameEvents.OnBreedingStart += () => SceneLoader.LoadScene("Breeding");

        //Register functions into OnBreedingComplete event
        //GameEvents.OnBreedingComplete += () => SceneLoader.LoadScene("TowerDefense");

        //GameEvents.OnLevelComplete += OnLevelComplete;
        //GameEvents.OnBreedingComplete += OnBreedingComplete;
    }

    public void OnBreedingButtonClicked()
    {
        GameEvents.TriggerBreedingStart(); 
        SceneLoader.LoadScene("Breeding");
        breedingButton.SetActive(false);

    }
   /* private void OnLevelComplete()
    {
        // 关卡完成，显示培育按钮或直接跳转到培育场景
        breedingButton.SetActive(true);
    }*/
    private void OnLevelStart()
    {
        // 关卡完成，显示培育按钮或直接跳转到培育场景
        LevelManager.Instance.StartCurrentLevel();
    }

    public void OnBreedingComplete()
    {
        currentLevelIndex++; // Move to the next level
        StartCoroutine(TriggerLevelStartAfterDelay(timeDelayBeforeStart));
        SceneLoader.LoadScene("TowerDefense"); // Load the tower defense scene for the next level
         // Ensure this method is used to start levels
    }


    void OnDestroy()
    {
        // 取消注册事件
        //GameEvents.OnLevelComplete -= OnLevelComplete;
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
