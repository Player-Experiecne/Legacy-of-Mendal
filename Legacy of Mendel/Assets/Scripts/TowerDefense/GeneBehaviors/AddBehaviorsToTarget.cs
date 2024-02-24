using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBehaviorsToTarget : MonoBehaviour
{
    public void AddGeneBehaviors(GameObject target, List<GeneInfo.geneTypes> geneTypes)
    {
        foreach (GeneInfo.geneTypes geneType in geneTypes)
        {
            switch (geneType)
            {
                case GeneInfo.geneTypes.ADom:
                    target.AddComponent<GeneADomBehaviors>();
                    break;
                case GeneInfo.geneTypes.AHet:
                    target.AddComponent<GeneAHetBehaviors>();
                    break;
                case GeneInfo.geneTypes.ARec:
                    target.AddComponent<GeneARecBehaviors>();
                    break;
                default:
                    // No gene A behavior attached for 'None'
                    break;
            }
        }
        //float randomValue = Random.Range(0f, 1f); // Generate a random float between 0 and 1
        //float occurrencePossibility = 0; // Default to 0

        //Add behaviors according to object types and possibility
        /*foreach(GeneInfo.geneTypes geneType in geneTypes)
        {
            if (defenderOrNot)
            {
                switch (geneType)
                {
                    case GeneInfo.geneTypes.ADom:
                        target.AddComponent<GeneADomBehaviors>();
                        break;
                    case GeneInfo.geneTypes.AHet:
                        target.AddComponent<GeneAHetBehaviors>();
                        break;
                    case GeneInfo.geneTypes.ARec:
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
                    case GeneInfo.geneTypes.ADom:
                        occurrencePossibility = geneTypeAInfo.domStats.occurrencePossibility;
                        if (randomValue <= occurrencePossibility)
                        {
                            target.AddComponent<GeneADomBehaviors>();
                        }
                        break;
                    case GeneInfo.geneTypes.AHet:
                        occurrencePossibility = geneTypeAInfo.hetStats.occurrencePossibility;
                        if (randomValue <= occurrencePossibility)
                        {
                            target.AddComponent<GeneAHetBehaviors>();
                        }
                        break;
                    case GeneInfo.geneTypes.ARec:
                        occurrencePossibility = geneTypeAInfo.recStats.occurrencePossibility;
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
        }*/

    }
}
