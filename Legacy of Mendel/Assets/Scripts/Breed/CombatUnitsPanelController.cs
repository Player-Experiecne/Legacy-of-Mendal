using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro; 

public class CombatUnitsPanelController : MonoBehaviour
{
    public PlayerDefenderInventory playerDefenderInventory;
    public List<Image> combatUnitImages;
   

    private GameObject currentTooltip;
    private int lastInventoryCount = -1;
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
        if (HasInventoryCountChanged())
        {
            UpdateDefenderDisplays();
        }
    }
    private bool HasInventoryCountChanged()
    {
        
        for (int i = 0; i < playerDefenderInventory.ownedDefenders.Count; i++)
        {
            if (playerDefenderInventory.ownedDefenders[i].count != lastInventoryCount)
            {
                return true;
            }
        }
        return false;
    }
    public void RefreshDisplay()
    {
        foreach (var image in combatUnitImages)
        {
            image.gameObject.SetActive(false); // 先隐藏所有图像
        }

        PopulateCombatUnitImages(); // 再填充图像
    }

    private void UpdateDefenderDisplays()
    {
        for (int i = 0; i < playerDefenderInventory.ownedDefenders.Count; i++)
        {
            Transform countTextTransform = combatUnitImages[i].transform.Find("CountText");
            if (countTextTransform != null)
            {
                TextMeshProUGUI countTextComponent = countTextTransform.GetComponent<TextMeshProUGUI>();
                if (countTextComponent != null)
                {
                    // 更新数量显示
                    countTextComponent.text = "Count: " + playerDefenderInventory.ownedDefenders[i].count.ToString();
                }
            }
        }
        // 更新最后的库存计数
        lastInventoryCount = playerDefenderInventory.ownedDefenders.Count;
    }
    private void PopulateCombatUnitImages()
    {
        int count = Mathf.Min(playerDefenderInventory.ownedDefenders.Count, combatUnitImages.Count);

        for (int i = 0; i < count; i++)
        {
            combatUnitImages[i].sprite = playerDefenderInventory.ownedDefenders[i].defender.defenderImage;

            Transform nameTextTransform = combatUnitImages[i].transform.Find("NameText"); 
            if (nameTextTransform != null)
            {
                TextMeshProUGUI nameTextComponent = nameTextTransform.GetComponent<TextMeshProUGUI>();
                if (nameTextComponent != null)
                {
                    nameTextComponent.text = "Name: "+ playerDefenderInventory.ownedDefenders[i].defender.defenderName;
                }
            }

            Transform countTextTransform = combatUnitImages[i].transform.Find("CountText"); 
            if (countTextTransform != null)
            {
                TextMeshProUGUI countTextComponent = countTextTransform.GetComponent<TextMeshProUGUI>();
                if (countTextComponent != null)
                {
                    countTextComponent.text = "Count: "+playerDefenderInventory.ownedDefenders[i].count.ToString();
                }
            }
            combatUnitImages[i].gameObject.SetActive(true);

            // 为每个Image添加EventTrigger监听
            //AddEventTrigger(combatUnitImages[i].gameObject, i);
        }

        for (int i = count; i < combatUnitImages.Count; i++)
        {
            combatUnitImages[i].gameObject.SetActive(false);
        }
    }

    

   
}
