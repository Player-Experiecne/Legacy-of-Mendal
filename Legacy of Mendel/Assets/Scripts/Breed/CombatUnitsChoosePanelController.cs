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

    private int lastInventoryCount = -1;


    private int selectedIndex = -1;

    private void Start()
    {
        RefreshDisplay();
        
    }

    void Update()
    {
        // 检查库存数量是否有变化
        if (playerDefenderInventory.ownedDefenders.Count != lastInventoryCount)
        {
            RefreshDisplay(); // 如果有变化，更新显示
            lastInventoryCount = playerDefenderInventory.ownedDefenders.Count; // 更新最后的库存数量，以便下次检查
        }
    }

    public void RefreshDisplay()
    {
        foreach (var image in combatUnitImages)
        {
            image.gameObject.SetActive(false); // 先隐藏所有图像
        }

        PopulateCombatUnitImages(); // 再填充图像
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

        if (selectedIndex != -1)
        {
            combatUnitImages[selectedIndex].color = Color.white;
            //defenderDisplayImage.sprite = null;
            selectedIndex = -1; 
        }
        thisPanel.SetActive(false);
      
        
    }
}

