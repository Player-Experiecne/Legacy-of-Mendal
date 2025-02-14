﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CombatUnitsPanelController : MonoBehaviour
{
    public PlayerDefenderInventory playerDefenderInventory;
    public List<Image> combatUnitImages;


    public int itemsPerPage = 10;  // 每页显示的单位数
    private int currentPage = 0;
    private int totalPageCount = 0;
    public Button nextPageButton;  // 翻到下一页的按钮
    public Button previousPageButton;  // 翻到上一页的按钮
    private Dictionary<string, int> inventorySnapshot;
    private int lastInventoryCount = -1;
    void Start()
    {
        inventorySnapshot = new Dictionary<string, int>();
        SnapshotInventory();
        UpdatePageCount();
        RefreshDisplay();
        UpdatePageButtons();
    }

    private void SnapshotInventory()
    {
        inventorySnapshot.Clear();
        foreach (var defender in playerDefenderInventory.ownedDefenders)
        {
            if (inventorySnapshot.ContainsKey(defender.defender.defenderName))
                inventorySnapshot[defender.defender.defenderName] += defender.count;
            else
                inventorySnapshot.Add(defender.defender.defenderName, defender.count);
        }
    }

    public void NextPage()
    {
        if (currentPage < totalPageCount - 1)
        {
            currentPage++;
            RefreshDisplay();
            UpdatePageButtons();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            RefreshDisplay();
            UpdatePageButtons();
        }
    }
    private void UpdatePageCount()
    {
        totalPageCount = Mathf.CeilToInt((float)playerDefenderInventory.ownedDefenders.Count / itemsPerPage);
    }


    private void UpdatePageButtons()
    {
        if (nextPageButton == null || previousPageButton == null)
        {
            Debug.LogWarning("One or both of the page buttons are not set in the CombatUnitsPanelController.");
            return;  // Prevent further execution of this method
        }

        // 更新翻页按钮的互动性
        nextPageButton.interactable = currentPage < totalPageCount - 1;
        previousPageButton.interactable = currentPage > 0;

        // 根据总页数决定是否显示翻页按钮
        bool shouldShowButtons = totalPageCount > 1;  // 只有当总页数大于1时才显示按钮
        nextPageButton.gameObject.SetActive(shouldShowButtons);
        previousPageButton.gameObject.SetActive(shouldShowButtons);
    }



    void Update()
    {
        
        if (HasInventoryChanged())
        {
            RefreshDisplay(); // Refresh display if any inventory changes detected
            SnapshotInventory(); // Update the snapshot after any change
        }
    }
    private bool HasInventoryChanged()
    {
        var currentSnapshot = new Dictionary<string, int>();
        foreach (var defender in playerDefenderInventory.ownedDefenders)
        {
            if (currentSnapshot.ContainsKey(defender.defender.defenderName))
                currentSnapshot[defender.defender.defenderName] += defender.count;
            else
                currentSnapshot.Add(defender.defender.defenderName, defender.count);
        }

        if (currentSnapshot.Count != inventorySnapshot.Count)
            return true; // Different number of types

        foreach (var item in currentSnapshot)
        {
            if (!inventorySnapshot.ContainsKey(item.Key) || inventorySnapshot[item.Key] != item.Value)
                return true; // Type not present or count mismatch
        }

        return false;
    }
    public void RefreshDisplay()
    {
        for (int i = 0; i < combatUnitImages.Count; i++)
        {
            if (i < itemsPerPage && currentPage * itemsPerPage + i < playerDefenderInventory.ownedDefenders.Count)
            {
                int defenderIndex = currentPage * itemsPerPage + i;
                combatUnitImages[i].sprite = playerDefenderInventory.ownedDefenders[defenderIndex].defender.defenderImage;
                combatUnitImages[i].gameObject.SetActive(true);
                UpdateText(combatUnitImages[i], "NameText", playerDefenderInventory.ownedDefenders[defenderIndex].defender.defenderName);
                UpdateText(combatUnitImages[i], "CountText", "Count: " + playerDefenderInventory.ownedDefenders[defenderIndex].count);
            }
            else
            {
                combatUnitImages[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateText(Image parentImage, string childName, string text)
    {
        Transform childTransform = parentImage.transform.Find(childName);
        if (childTransform != null)
        {
            TextMeshProUGUI textComponent = childTransform.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = text;
            }
        }
    }

    private void UpdateDefenderDisplays()
    {
        int itemCount = Mathf.Min(playerDefenderInventory.ownedDefenders.Count, combatUnitImages.Count);
        for (int i = 0; i < itemCount; i++)
        {
            Transform countTextTransform = combatUnitImages[i].transform.Find("CountText");
            if (countTextTransform != null)
            {
                TextMeshProUGUI countTextComponent = countTextTransform.GetComponent<TextMeshProUGUI>();
                if (countTextComponent != null)
                {
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
                    nameTextComponent.text = "Name: " + playerDefenderInventory.ownedDefenders[i].defender.defenderName;
                }
            }

            Transform countTextTransform = combatUnitImages[i].transform.Find("CountText");
            if (countTextTransform != null)
            {
                TextMeshProUGUI countTextComponent = countTextTransform.GetComponent<TextMeshProUGUI>();
                if (countTextComponent != null)
                {
                    countTextComponent.text = "Count: " + playerDefenderInventory.ownedDefenders[i].count.ToString();
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