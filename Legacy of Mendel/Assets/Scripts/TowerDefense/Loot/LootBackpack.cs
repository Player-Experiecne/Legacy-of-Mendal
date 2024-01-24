using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootBackpack : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static LootBackpack Instance { get; private set; }

    public List<GeneInfo.gene> lootGenes;
    public int lootCultureMedium = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        RefreshUI();
    }

    public void LootGene(GeneInfo.gene gene)
    {
        lootGenes.Add(gene);
    }

    public void LootCultureMedium(int cultureMedium)
    {
        lootCultureMedium += cultureMedium;
        RefreshUI();
    }
    private void RefreshUI()
    {
        text.text = lootCultureMedium.ToString();
    }

}
