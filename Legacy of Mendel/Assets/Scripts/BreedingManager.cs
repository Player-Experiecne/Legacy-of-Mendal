using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingManager : MonoBehaviour
{
    void Start()
    {
        // 培育事件
        FindObjectOfType<GameManager>().OnBreedingEvent += HandleBreedingEvent;
    }

    private void HandleBreedingEvent()
    {
        // 培育逻辑
    }
}

