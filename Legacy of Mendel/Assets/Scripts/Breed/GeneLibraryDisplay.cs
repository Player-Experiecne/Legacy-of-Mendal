using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text; 

public class GeneLibraryDisplay : MonoBehaviour
{
    [System.Serializable]
    public class TextGroup
    {
        public List<TextMeshProUGUI> texts; 
    }

    public GeneDatabase geneDatabase; 
    public List<TextMeshProUGUI> geneNameTexts; 
    public List<TextGroup> geneInfoTextGroups; 

    void Start()
    {
        RefreshDisplay();
        UpdateGeneTypeDisplays();
    }

    public void RefreshDisplay()
    {
        
        string[] geneTypes = { "A", "B", "C" };

       
        for (int i = 0; i < geneTypes.Length; i++)
        {
           
            if (i < geneNameTexts.Count)
            {
               
                bool found = false;
                foreach (var gene in geneDatabase.allGenes)
                {
                    if (gene.geneName.ToString() == geneTypes[i] && gene.isOwned)
                    {
                        found = true;
                        break;
                    }
                }

                // 如果找到，显示基因名称；否则，显示问号
                geneNameTexts[i].text = found ? (geneTypes[i].ToString() + " Type") : "?" + " Type";

            }
        }
    }

    void UpdateGeneTypeDisplays()
    {
        // 你可以在这里定义每个基因型名称的显示方式
        Dictionary<GeneInfo.geneTypesName, string[]> geneDisplays = new Dictionary<GeneInfo.geneTypesName, string[]>()
    {
        { GeneInfo.geneTypesName.A, new string[] { "AA", "Aa", "aa" } },
        { GeneInfo.geneTypesName.B, new string[] { "BB", "Bb", "bb" } },
        { GeneInfo.geneTypesName.C, new string[] { "CC", "Cc", "cc" } }
    };

        // 遍历每个基因型名称
        for (int i = 0; i < geneInfoTextGroups.Count; i++)
        {
            // 假设geneTypesName数组包含了所有基因型的名称
            GeneInfo.geneTypesName[] geneTypesName = { GeneInfo.geneTypesName.A, GeneInfo.geneTypesName.B, GeneInfo.geneTypesName.C };

            var geneName = geneTypesName[i];  // 当前处理的基因名称
            string[] displays = geneDisplays[geneName];  // 当前基因的所有显示形式

            // 在数据库中查找当前基因型名称的所有数据
            for (int j = 0; j < geneDatabase.allGenes.Count; j++)
            {
                var geneData = geneDatabase.allGenes[j];

                if (geneData.geneName == geneName)  // 检查基因名称是否匹配
                {
                    // 根据基因型更新UI显示
                    int textIndex = geneData.geneType == GeneInfo.geneTypes.Dom ? 0 : geneData.geneType == GeneInfo.geneTypes.Het ? 1 : 2; // 根据基因类型确定应该在哪个文本框显示
                    geneInfoTextGroups[i].texts[textIndex].text = geneData.isOwned ? displays[textIndex] : "?";
                }
            }
        }
    }



}
