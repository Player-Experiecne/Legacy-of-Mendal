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
        LootGeneARec
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
                    lootBackpack.LootGene(GeneInfo.gene.ADom);
                    break;
                case LootType.LootGeneARec:
                    lootBackpack.LootGene(GeneInfo.gene.ARec); 
                    break;
            }
            Destroy(gameObject);
        }
    }
}
