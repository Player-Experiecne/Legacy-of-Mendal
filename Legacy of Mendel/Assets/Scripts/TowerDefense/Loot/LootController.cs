using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    public LootType lootType;

    private LootBackpack lootBackpack = LootBackpack.Instance;

    public enum LootType
    {
        LootCultureMedium,
        LootGeneADom,
        LootGeneAHet,
        LootGeneARec,
        LootGeneBDom,
        LootGeneBHet,
        LootGeneBRec,
        LootGeneCDom,
        LootGeneCHet,
        LootGeneCRec,
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            switch (lootType)
            {
                case LootType.LootCultureMedium:
                    lootBackpack.LootCultureMedium(1);
                    break;
                case LootType.LootGeneADom:
                    lootBackpack.LootGeneType(new GeneTypeEntry { geneName = GeneInfo.geneTypesName.A, geneType = GeneInfo.geneTypes.Dom});
                    break;
                case LootType.LootGeneAHet:
                    lootBackpack.LootGeneType(new GeneTypeEntry { geneName = GeneInfo.geneTypesName.A, geneType = GeneInfo.geneTypes.Het});
                    break;
                case LootType.LootGeneARec:
                    lootBackpack.LootGeneType(new GeneTypeEntry { geneName = GeneInfo.geneTypesName.A, geneType = GeneInfo.geneTypes.Rec});
                    break;
                case LootType.LootGeneBDom:
                    lootBackpack.LootGeneType(new GeneTypeEntry { geneName = GeneInfo.geneTypesName.B, geneType = GeneInfo.geneTypes.Dom});
                    break;
                case LootType.LootGeneBHet:
                    lootBackpack.LootGeneType(new GeneTypeEntry { geneName = GeneInfo.geneTypesName.B, geneType = GeneInfo.geneTypes.Het});
                    break;
                case LootType.LootGeneBRec:
                    lootBackpack.LootGeneType(new GeneTypeEntry { geneName = GeneInfo.geneTypesName.B, geneType = GeneInfo.geneTypes.Rec});
                    break;
                case LootType.LootGeneCDom:
                    lootBackpack.LootGeneType(new GeneTypeEntry { geneName = GeneInfo.geneTypesName.C, geneType = GeneInfo.geneTypes.Dom});
                    break;
                case LootType.LootGeneCHet:
                    lootBackpack.LootGeneType(new GeneTypeEntry { geneName = GeneInfo.geneTypesName.C, geneType = GeneInfo.geneTypes.Het});
                    break;
                case LootType.LootGeneCRec:
                    lootBackpack.LootGeneType(new GeneTypeEntry { geneName = GeneInfo.geneTypesName.C, geneType = GeneInfo.geneTypes.Rec});
                    break;
            }
            Destroy(gameObject);
        }
    }
}
