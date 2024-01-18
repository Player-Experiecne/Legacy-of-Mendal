using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DayNightState
{
    Day,
    Night
}

// 黑夜白昼切换事件
public delegate void DayNightChangeDelegate(DayNightState newState); 
// 培育事件
public delegate void BreedingEventDelegate(); 
// 塔防事件
public delegate void TowerDefenseEventDelegate(); 
