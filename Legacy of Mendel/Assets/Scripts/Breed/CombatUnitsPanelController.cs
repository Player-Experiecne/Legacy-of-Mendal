using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUnitsPanelController : MonoBehaviour
{
    public PlayerDefenderInventory playerDefenderInventory; 
    public List<Image> combatUnitImages; 

    private void Start()
    {
        PopulateCombatUnitImages();
    }

   
    private void PopulateCombatUnitImages()
    {
        
        int count = Mathf.Min(playerDefenderInventory.ownedDefenders.Count, combatUnitImages.Count);

      
        for (int i = 0; i < count; i++)
        {
           
            combatUnitImages[i].sprite = playerDefenderInventory.ownedDefenders[i].defender.defenderImage;
            combatUnitImages[i].gameObject.SetActive(true); 
        }

        
        for (int i = count; i < combatUnitImages.Count; i++)
        {
            combatUnitImages[i].gameObject.SetActive(false);
        }
    }
}
