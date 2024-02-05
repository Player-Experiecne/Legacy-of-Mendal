using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDefenderInventory", menuName = "Game Data/Player Defender Inventory")]
public class PlayerDefenderInventory : ScriptableObject
{
    public List<DefenderWithCount> ownedDefenders;
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
    }

    // 添加一个辅助方法来比较两个基因型列表是否相等
    private bool AreGeneTypesEqual(List<GeneInfo.geneTypes> geneTypesA, List<GeneInfo.geneTypes> geneTypesB)
    {
        // 比较列表长度是否相等
        if (geneTypesA.Count != geneTypesB.Count) return false;

        // 检查每个元素是否相等
        for (int i = 0; i < geneTypesA.Count; i++)
        {
            if (geneTypesA[i] != geneTypesB[i]) return false;
        }

        return true;
    }

}
