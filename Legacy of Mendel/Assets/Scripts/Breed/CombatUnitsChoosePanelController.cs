using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUnitsChoosePanelController : MonoBehaviour
{
    public static BreedManager Instance;
    
    public PlayerDefenderInventory playerDefenderInventory;
    public List<Image> combatUnitImages;
    public Image defenderDisplayImage;
    public Button confirmButton; 
    public GameObject thisPanel;
    //public GameObject previousPanel; 
    


    private int selectedIndex = -1;

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
            int index = i; 
            combatUnitImages[i].GetComponent<Button>().onClick.RemoveAllListeners(); 
            combatUnitImages[i].GetComponent<Button>().onClick.AddListener(() => {
                ToggleSelection(index); 
                SelectDefender(index); 
            });
        }

        for (int i = count; i < combatUnitImages.Count; i++)
        {
            combatUnitImages[i].gameObject.SetActive(false);
        }
    }


    private void ToggleSelection(int index)
    {
        if (selectedIndex == index) 
        {
            combatUnitImages[index].color = Color.white; 
            defenderDisplayImage.sprite = null; 
            selectedIndex = -1;
        }
        else
        {
            if (selectedIndex != -1) 
            {
                combatUnitImages[selectedIndex].color = Color.white;
            }
            combatUnitImages[index].color = Color.green; 
            selectedIndex = index;
           
            defenderDisplayImage.sprite = playerDefenderInventory.ownedDefenders[index].defender.defenderImage;
        }
    }

    private void SelectDefender(int index)
    {
        
        defenderDisplayImage.sprite = playerDefenderInventory.ownedDefenders[index].defender.defenderImage;
        BreedManager.Instance.SetSelectedDefender(playerDefenderInventory.ownedDefenders[index].defender);
    }
    public void ConfirmSelection()
    {
        // 隐藏当前面板
        thisPanel.SetActive(false);
      
        
    }
}

