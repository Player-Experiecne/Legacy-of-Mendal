using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GeneInfo;

public class LootBackpack : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static LootBackpack Instance { get; private set; }

    public List<GeneInfo.geneTypes> lootGeneTypes;
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

    public void LootGeneType(GeneInfo.geneTypes geneType)
    {
        lootGeneTypes.Add(geneType);
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
