using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    [Header("Stats")]
    public float hp;
    public float attackPower;
    public float attackRange;
    public float attackSpeed;
    public float Speed;
    [Header("GeneType")]
    public GeneTypeAInfoSO.GeneTypeA geneTypeA;
    [Header("UI Info")]
    public string enemyName;
    public Sprite enemyImage;
    [Header("Loot")]
    public List<LootGene> lootGenes;
    public LootCultureMedium lootCultureMedium;
}

[System.Serializable]
public class LootGene
{
    public GeneInfo.gene gene;
    [Range(0f, 1f)]
    public float probability;
}

[System.Serializable]
public class LootCultureMedium
{
    public int minLootCultureMedium;
    public int maxLootCultureMedium;
    [Range(0f, 1f)]
    public float lootCultureMediumprobability;
}
