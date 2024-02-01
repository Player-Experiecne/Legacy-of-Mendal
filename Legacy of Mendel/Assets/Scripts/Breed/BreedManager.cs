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

    
    public void EndBreedingPhase()
    {
        
    }
}
