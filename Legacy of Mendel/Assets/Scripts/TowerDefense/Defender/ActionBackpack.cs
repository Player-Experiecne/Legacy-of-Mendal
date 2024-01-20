using System.Collections.Generic;
using UnityEngine;

public class ActionBackpack : MonoBehaviour
{
    public List<Defender> defendersInBackpack = new List<Defender>();
    [HideInInspector] public Defender activeDefender = null;
    public ActionBackpackUI ui;

    public void AddDefenderToBackpack(Defender defender)
    {
        if (!defendersInBackpack.Contains(defender))
        {
            defendersInBackpack.Add(defender);
            ui.RefreshUI();
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

}
