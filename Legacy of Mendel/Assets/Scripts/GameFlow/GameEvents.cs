using UnityEngine;

public class GameEvents
{
    public delegate void GameStateChange();
    public static event GameStateChange OnTutorialStart;
    public static event GameStateChange OnTutorialEnd;
    public static event GameStateChange OnEnemySpawn;
    public static event GameStateChange OnLevelComplete;
    public static event GameStateChange OnLevelFail;
    public static event GameStateChange OnBreedingStart;
    public static event GameStateChange OnBreedingComplete;
    public static event GameStateChange OnTitleScreen;
    public static event GameStateChange OnTowerDefense;



    public static void TriggerTutorialStart()
    {
        OnTutorialStart?.Invoke();
    }
    public static void TriggerTutorialEnd()
    {
        OnTutorialEnd?.Invoke();
    }
    public static void TriggerEnemySpawn()
    {
        OnEnemySpawn?.Invoke();
    }
    public static void TriggerLevelComplete()
    {
        OnLevelComplete?.Invoke();
    }
    public static void TriggerLevelFail() 
    {
        OnLevelFail?.Invoke();
    }
    public static void TriggerBreedingStart()
    {
        OnBreedingStart?.Invoke();
    }
    public static void TriggerBreedingComplete()
    {
        OnBreedingComplete?.Invoke();
    }
    public static void TriggerTitleScreen()
    {
        OnTitleScreen?.Invoke();
    }
    public static void TriggerTowerDefense()
    {
        OnTowerDefense?.Invoke();
    }
}
