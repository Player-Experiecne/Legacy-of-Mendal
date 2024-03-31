using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDefenderBackpack : MonoBehaviour
{
    // Start is called before the first frame update
    
    public List<Defender> defendersInTutorial= new List<Defender>();
    public Defender activeDefender = null;
    public DefenderBackpackUI ui;

   

    public void AddDefenderToBackpack(Defender defender)
    {
        defendersInTutorial.Add(defender);
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
        if (defendersInTutorial.Contains(defender))
        {
            if (activeDefender == defender)
            {
                activeDefender = null;
            }
            defendersInTutorial.Remove(defender);
            ui.RefreshUI();
        }
    }

    public void SetActiveDefender(Defender defender)
    {
        if (defendersInTutorial.Contains(defender))
        {
            activeDefender = defender;
            ui.RefreshUI();
        }
    }

    public void ClearBackpack()
    {
        defendersInTutorial.Clear();
        activeDefender = null;
        ui.RefreshUI();
    }
}
