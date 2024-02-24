using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBehaviorsToTarget : MonoBehaviour
{
    /*  public void AddGeneBehaviors(GameObject target, List<GeneInfo.geneTypes> geneTypes)
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
          }*/
    public void AddGeneBehaviors(GameObject target, List<GeneTypeEntry> geneTypes)
    {
        foreach (var geneType in geneTypes)
        {
            // 根据geneType.geneName和geneType.geneType来决定添加哪个行为
            if (geneType.geneName == GeneInfo.geneTypesName.A)
            {
                switch (geneType.geneType)
                {
                    case GeneInfo.geneTypes.Dom:
                        target.AddComponent<GeneADomBehaviors>();
                        break;
                    case GeneInfo.geneTypes.Het:
                        target.AddComponent<GeneAHetBehaviors>();
                        break;
                    case GeneInfo.geneTypes.Rec:
                        target.AddComponent<GeneARecBehaviors>();
                        break;
                }
            }
            else if (geneType.geneName == GeneInfo.geneTypesName.B)
            {
                switch (geneType.geneType)
                {
                    case GeneInfo.geneTypes.Dom:
                        target.AddComponent<GeneBDomBehaviors>();
                        break;
                    case GeneInfo.geneTypes.Het:
                        //target.AddComponent<GeneBHetBehaviors>();
                        break;
                    case GeneInfo.geneTypes.Rec:
                        //target.AddComponent<GeneBRecBehaviors>();
                        break;
                }
            }
            else if (geneType.geneName == GeneInfo.geneTypesName.C)
            {
                switch (geneType.geneType)
                {
                    case GeneInfo.geneTypes.Dom:
                        //target.AddComponent<GeneCDomBehaviors>();
                        break;
                    case GeneInfo.geneTypes.Het:
                       // target.AddComponent<GeneCHetBehaviors>();
                        break;
                    case GeneInfo.geneTypes.Rec:
                        //target.AddComponent<GeneCRecBehaviors>();
                        break;
                }
            }
            // 可以继续添加其他基因的处理逻辑
        }
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

  
