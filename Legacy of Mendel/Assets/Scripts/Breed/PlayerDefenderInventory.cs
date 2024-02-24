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
    public void AddDefender(Defender defender)
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
    }
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
    private bool AreGeneTypesEqual(List<GeneTypeEntry> geneTypesA, List<GeneTypeEntry> geneTypesB)
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
    }


}