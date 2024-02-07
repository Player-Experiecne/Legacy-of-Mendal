using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro; 

public class CombatUnitsPanelController : MonoBehaviour
{
    public PlayerDefenderInventory playerDefenderInventory;
    public List<Image> combatUnitImages;
    public GameObject tooltipPrefab; // 提示框的Prefab

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

            // 为每个Image添加EventTrigger监听
            AddEventTrigger(combatUnitImages[i].gameObject, i);
        }

        for (int i = count; i < combatUnitImages.Count; i++)
        {
            combatUnitImages[i].gameObject.SetActive(false);
        }
    }

    private void AddEventTrigger(GameObject imageGameObject, int index)
    {
        EventTrigger trigger = imageGameObject.GetComponent<EventTrigger>() ?? imageGameObject.AddComponent<EventTrigger>();

        // 悬停进入
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { ShowTooltip(index); });
        trigger.triggers.Add(entryEnter);

        // 悬停退出
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { HideTooltip(); });
        trigger.triggers.Add(entryExit);
    }

    private void ShowTooltip(int index)
    {
        if (currentTooltip != null) Destroy(currentTooltip);

        currentTooltip = Instantiate(tooltipPrefab, transform);
       
        TextMeshProUGUI tooltipText = currentTooltip.GetComponentInChildren<TextMeshProUGUI>();
        tooltipText.text = $"Count: {playerDefenderInventory.ownedDefenders[index].count}";

        // 调整提示框位置，稍微偏离鼠标位置防止遮挡
        Vector3 mousePosition = Input.mousePosition;
        //mousePosition += new Vector3(10, -10, 0); // 例如向右下角偏移
        currentTooltip.transform.position = mousePosition;
    }

    private void HideTooltip()
    {
        Destroy(currentTooltip);
    }
}
