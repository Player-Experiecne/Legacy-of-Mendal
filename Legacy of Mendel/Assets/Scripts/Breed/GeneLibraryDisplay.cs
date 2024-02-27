using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class GeneLibraryDisplay : MonoBehaviour
{
    public GeneDatabase geneDatabase; // 将类型从GeneLibrary更改为GeneDatabase
    public List<TextMeshProUGUI> geneListTexts;

    private int itemsPerPage;

   /* private void Start()
    {
        geneDatabase.OnGeneLibraryUpdated += RefreshDisplay; // 确保GeneDatabase有一个类似的事件
        RefreshDisplay();
    }

    void OnDestroy()
    {
        // 确保当对象被销毁时取消订阅事件
        geneDatabase.OnGeneLibraryUpdated -= RefreshDisplay;
    }*/

    public void RefreshDisplay()
    {
        itemsPerPage = Mathf.CeilToInt((float)geneDatabase.allGenes.Count / geneListTexts.Count); // 确保GeneDatabase有一个allGenes列表
        PopulateGeneLists();
    }

    void PopulateGeneLists()
    {
        int geneIndex = 0;

        foreach (var textComponent in geneListTexts)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < itemsPerPage && geneIndex < geneDatabase.allGenes.Count; i++, geneIndex++)
            {
                var gene = geneDatabase.allGenes[geneIndex]; // 使用GeneDatabase的数据
                if (gene.isOwned)
                {
                    // 显示基因名和基因类型
                    stringBuilder.AppendLine($"{gene.geneName} - {gene.geneType}");
                }
                else
                {
                    // 如果基因未拥有，显示问号
                    stringBuilder.AppendLine("?");
                }
            }
            textComponent.text = stringBuilder.ToString();
        }
    }
}
