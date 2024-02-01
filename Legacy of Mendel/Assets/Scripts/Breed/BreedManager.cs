using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedManager : MonoBehaviour
{
    
    public GameObject breedingUI; 

    
    public void StartBreedingPhase()
    {
        breedingUI.SetActive(true);
        PerformAnalysis(); 
    }

 
    private void PerformAnalysis()
    {
        // 获取 LootBackpack 实例中的数据
        var lootGeneTypes = LootBackpack.Instance.lootGeneTypes;
       
        //右上角显示数量
        int tissueCount = LootBackpack.Instance.lootGeneTypes.Count;
        int cultureMediumCount = LootBackpack.Instance.lootCultureMedium;

    }

    public Defender PerformFusion(Defender defender, GeneInfo.geneTypes newGene)
    {
        
        Defender newDefender = Instantiate(defender.defenderPrefab).GetComponent<Defender>();

      
        if (newGene == GeneInfo.geneTypes.ADom || defender.geneTypes.Contains(GeneInfo.geneTypes.ADom))
        {
            newDefender.geneTypes.Add(GeneInfo.geneTypes.ADom);
        }
        
        else if (newGene == GeneInfo.geneTypes.AHet && !defender.geneTypes.Contains(GeneInfo.geneTypes.ADom))
        {
            newDefender.geneTypes.Add(GeneInfo.geneTypes.AHet);
        }
        
        else
        {
            newDefender.geneTypes.Add(GeneInfo.geneTypes.ARec);
        }

        

        return newDefender;
    }
    public void EndBreedingPhase()
    {
        
    }
}
