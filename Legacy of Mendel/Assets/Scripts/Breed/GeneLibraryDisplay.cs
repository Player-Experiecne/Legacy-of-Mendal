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
    void Update()
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
        
        Dictionary<GeneInfo.geneTypesName, string[]> geneDisplays = new Dictionary<GeneInfo.geneTypesName, string[]>()
    {
        { GeneInfo.geneTypesName.A, new string[] { "AA", "Aa", "aa" } },
        { GeneInfo.geneTypesName.B, new string[] { "BB", "Bb", "bb" } },
        { GeneInfo.geneTypesName.C, new string[] { "CC", "Cc", "cc" } }
    };

        
        for (int i = 0; i < geneInfoTextGroups.Count; i++)
        {
            
            GeneInfo.geneTypesName[] geneTypesName = { GeneInfo.geneTypesName.A, GeneInfo.geneTypesName.B, GeneInfo.geneTypesName.C };

            var geneName = geneTypesName[i];  
            string[] displays = geneDisplays[geneName];  

           
            for (int j = 0; j < geneDatabase.allGenes.Count; j++)
            {
                var geneData = geneDatabase.allGenes[j];

                if (geneData.geneName == geneName)  
                {
                    
                    int textIndex = geneData.geneType == GeneInfo.geneTypes.Dom ? 0 : geneData.geneType == GeneInfo.geneTypes.Het ? 1 : 2; 
                    geneInfoTextGroups[i].texts[textIndex].text = geneData.isOwned ? displays[textIndex] : "?";
                }
            }
        }
    }



}
