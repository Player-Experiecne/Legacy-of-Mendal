using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class GeneLibraryDisplay : MonoBehaviour
{
    public GeneLibrary geneLibrary;
    public List<TextMeshProUGUI> geneListTexts;

    private int itemsPerPage;

    private void Start()
    {
        geneLibrary.OnGeneLibraryUpdated += RefreshDisplay;
        RefreshDisplay();
    
}
    void OnDestroy()
    {
        // 确保当对象被销毁时取消订阅事件
        geneLibrary.OnGeneLibraryUpdated -= RefreshDisplay;
    }
    public void RefreshDisplay()
    {
        itemsPerPage = Mathf.CeilToInt((float)geneLibrary.allGenes.Count / geneListTexts.Count);
        PopulateGeneLists();
    }

    void PopulateGeneLists()
    {
        int geneIndex = 0;

        foreach (var textComponent in geneListTexts)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < itemsPerPage && geneIndex < geneLibrary.allGenes.Count; i++, geneIndex++)
            {
                var gene = geneLibrary.allGenes[geneIndex];
                if (gene.isOwned)
                {
                    stringBuilder.AppendLine(gene.name);
                }
                else
                {
                    stringBuilder.AppendLine("?");
                }
            }
            textComponent.text = stringBuilder.ToString();
        }
    }
}
