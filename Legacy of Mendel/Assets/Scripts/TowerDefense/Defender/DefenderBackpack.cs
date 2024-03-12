using System.Collections.Generic;
using UnityEngine;

public class DefenderBackpack : MonoBehaviour
{
    public static DefenderBackpack Instance;

    public List<Defender> defendersInBackpack = new List<Defender>();
    public Defender activeDefender = null;
    public DefenderBackpackUI ui;

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

    public void AddDefenderToBackpack(Defender defender)
    {
        defendersInBackpack.Add(defender);
        ui.RefreshUI();
    }


    public void AddDefendersFromInventory(PlayerDefenderInventory inventory, int countToAdd)
    {
        foreach (var defenderWithCount in inventory.ownedDefenders)
        {
           
            int actualCountToAdd = Mathf.Min(countToAdd, defenderWithCount.count);

           
            for (int i = 0; i < actualCountToAdd; i++)
            {
                AddDefenderToBackpack(defenderWithCount.defender);
            }
        }
    }


    public void RemoveDefenderFromBackpack(Defender defender)
    {
        if (defendersInBackpack.Contains(defender))
        {
            if(activeDefender == defender)
            {
                activeDefender = null;
            }
            defendersInBackpack.Remove(defender);
            ui.RefreshUI();
        }
    }

    public void SetActiveDefender(Defender defender)
    {
        if (defendersInBackpack.Contains(defender))
        {
            activeDefender = defender;
            ui.RefreshUI();
        }
    }

    public void ClearBackpack()
    {
        defendersInBackpack.Clear();
        activeDefender = null;
        ui.RefreshUI();
    }
}
