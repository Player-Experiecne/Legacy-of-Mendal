using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "PlayerDefenderInventory", menuName = "Game Data/Player Defender Inventory")]
public class PlayerDefenderInventory : ScriptableObject
{
    public List<DefenderWithCount> ownedDefenders;
    public event Action OnInventoryUpdated;
    /*public void AddDefender(Defender defender)
    {
        // 查找是否已经拥有具有相同基因型的 Defender
        var existingDefender = ownedDefenders.Find(d => AreGeneTypesEqual(d.defender.geneTypes, defender.geneTypes));
        if (existingDefender != null)
        {
            // 如果已经拥有，则数量加一
            existingDefender.count += 1;
        }
        else
        {
            // 如果还没有，则添加新的 DefenderWithCount 到列表中
            ownedDefenders.Add(new DefenderWithCount(defender, 1));
        }
        OnInventoryUpdated?.Invoke();
    }*/
    public void DecreaseDefenderCount(Defender defender)
    {
        foreach (var defenderWithCount in ownedDefenders)
        {
            if (defenderWithCount.defender == defender)
            {
                defenderWithCount.count--;
                if (defenderWithCount.count <= 0)
                {
                    // 如果数量降至0或以下，从库存中移除Defender
                    ownedDefenders.Remove(defenderWithCount);
                    OnInventoryUpdated?.Invoke();
                }
                break;
            }
        }
    }
    // 添加一个辅助方法来比较两个基因型列表是否相等
    /*   private bool AreGeneTypesEqual(List<GeneTypeEntry> geneTypesA, List<GeneTypeEntry> geneTypesB)
       {
           if (geneTypesA.Count != geneTypesB.Count) return false;

           // 对于A中的每个基因型条目，检查B中是否有匹配的基因型和基因名
           foreach (var geneA in geneTypesA)
           {
               var geneB = geneTypesB.FirstOrDefault(g => g.geneName == geneA.geneName && g.geneType == geneA.geneType);
               if (geneB == null)
               {
                   // 如果在B中找不到匹配项，则两个基因型列表不相等
                   return false;
               }
           }

           // 如果所有检查都通过，说明两个基因型列表相等
           return true;
       }*/
    private bool AreGeneTypesEqual(DefenderWithCount defenderWithCount, Defender defenderB)
    {
        // 获取两个防御者的基因型列表
        List<GeneTypeEntry> geneTypesA = defenderWithCount.defender.geneTypes;
        List<GeneTypeEntry> geneTypesB = defenderB.geneTypes;

        // 比较列表长度是否相等
        if (geneTypesA.Count != geneTypesB.Count) return false;

        // 检查每个基因型是否相等
        for (int i = 0; i < geneTypesA.Count; i++)
        {
            if (geneTypesA[i].geneName != geneTypesB[i].geneName || geneTypesA[i].geneType != geneTypesB[i].geneType)
            {
                return false;
            }
        }
        return true;
    }
    // 在PlayerDefenderInventory类中
    public void AddDefenderToInventory(Defender newDefender)
    {
        // 查找库存中是否已有相同基因型的防御者
        DefenderWithCount existingDefender = ownedDefenders.Find(def => AreGeneTypesEqual(def, newDefender));

        if (existingDefender != null)
        {
            // 如果找到，只增加现有防御者的数量
            existingDefender.count++;
        }
        else
        {
            // 如果未找到，添加新的防御者到库存
            ownedDefenders.Add(new DefenderWithCount(newDefender, 1));
        }
    }

    public void RemoveDefenderFromInventory(Defender defenderToRemove)
    {
        foreach (var defenderWithCount in ownedDefenders)
        {
            if (defenderWithCount.defender == defenderToRemove)
            {
                defenderWithCount.count--;
                if (defenderWithCount.count <= 0)
                {
                    // 如果数量降至0或以下，从库存中移除Defender
                    ownedDefenders.Remove(defenderWithCount);
                    
                }
                break;
            }
        }
    }


}