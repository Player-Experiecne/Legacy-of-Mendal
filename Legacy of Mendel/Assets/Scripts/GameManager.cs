using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DayNightState CurrentState { get; private set; }

    public GameObject breedingUI;


    public event DayNightChangeDelegate OnDayNightChange;
    public event BreedingEventDelegate OnBreedingEvent;
    public event TowerDefenseEventDelegate OnTowerDefenseEvent;

    void Start()
    {
        // 初始化状态
        ChangeState(DayNightState.Day);

        if (breedingUI != null)
        {
            breedingUI.SetActive(false);
        }
    }


    public void ChangeState(DayNightState newState)
    {
        if (CurrentState != newState)
        {
            CurrentState = newState;
            OnDayNightChange?.Invoke(newState);
            HandleStateChange(newState);
        }
    }

    private void HandleStateChange(DayNightState newState)
    {
        switch (newState)
        {
            case DayNightState.Day:
                // 进入白天
                OnTowerDefenseEvent?.Invoke(); // 触发塔防事件
                break;
            case DayNightState.Night:
                // 进入黑天
                OnBreedingEvent?.Invoke(); // 触发培育事件
                break;
        }
    }

    
}

