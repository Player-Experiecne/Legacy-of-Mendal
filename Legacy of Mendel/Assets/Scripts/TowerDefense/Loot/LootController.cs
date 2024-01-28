using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    [SerializeField]
    public LootType lootType;

    private LootBackpack lootBackpack = LootBackpack.Instance;

    public enum LootType
    {
        LootCultureMedium,
        LootGeneADom,
        LootGeneAHet,
        LootGeneARec
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
        if(other.tag == "Player")
        {
            Debug.Log("2");
            switch (lootType)
            {
                case LootType.LootCultureMedium:
                    lootBackpack.LootCultureMedium(1);
                    break;
                case LootType.LootGeneADom:
                    lootBackpack.LootGeneType(GeneInfo.geneTypes.ADom);
                    break;
                case LootType.LootGeneAHet:
                    lootBackpack.LootGeneType(GeneInfo.geneTypes.AHet);
                    break;
                case LootType.LootGeneARec:
                    lootBackpack.LootGeneType(GeneInfo.geneTypes.ARec); 
                    break;
            }
            Destroy(gameObject);
        }
    }
}
