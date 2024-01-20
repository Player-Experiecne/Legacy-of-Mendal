using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBehaviorsToTarget : MonoBehaviour
{
    [SerializeField] GeneTypeAInfoSO geneTypeAInfo;
    
    public void AddGeneABehaviors(GameObject target, GeneTypeAInfoSO.GeneTypeA geneType, bool defenderOrNot)
    {
        float randomValue = Random.Range(0f, 1f); // Generate a random float between 0 and 1
        float occurrencePossibility = 0; // Default to 0

        if (defenderOrNot) 
        {
            switch (geneType)
            {
                case GeneTypeAInfoSO.GeneTypeA.ADom:
                    target.AddComponent<GeneADomBehaviors>();
                    break;
                case GeneTypeAInfoSO.GeneTypeA.AHet:
                    target.AddComponent<GeneAHetBehaviors>();
                    break;
                case GeneTypeAInfoSO.GeneTypeA.ARec:
                    target.AddComponent<GeneARecBehaviors>();
                    break;
                default:
                    // No gene A behavior attached for 'None'
                    break;
            }
        }
        else 
        {
            switch (geneType)
            {
                case GeneTypeAInfoSO.GeneTypeA.ADom:
                    occurrencePossibility = geneTypeAInfo.ADom.occurrencePossibility;
                    if (randomValue <= occurrencePossibility)
                    {
                        target.AddComponent<GeneADomBehaviors>();
                    }
                    break;
                case GeneTypeAInfoSO.GeneTypeA.AHet:
                    occurrencePossibility = geneTypeAInfo.AHet.occurrencePossibility;
                    if (randomValue <= occurrencePossibility)
                    {
                        target.AddComponent<GeneAHetBehaviors>();
                    }
                    break;
                case GeneTypeAInfoSO.GeneTypeA.ARec:
                    occurrencePossibility = geneTypeAInfo.ARec.occurrencePossibility;
                    if (randomValue <= occurrencePossibility)
                    {
                        target.AddComponent<GeneARecBehaviors>();
                    }
                    break;
                default:
                    // No gene A behavior attached for 'None'
                    break;
            }
        }
    }
}
