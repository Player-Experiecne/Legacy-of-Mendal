using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    /*[Header("Stats")]
    public float hp;
    public float attackPower;
    public float attackRange;
    public float attackSpeed;
    public float speed;
    [Header("GeneTypes")]
    public List<GeneInfo.geneTypes> geneTypes;*/
    [HideInInspector]
    public string enemyName;
    [HideInInspector]
    public Sprite enemyImage;
    [Header("Loot")]
    public LootGeneType lootGeneType;
    public LootCultureMedium lootCultureMedium;
}

[System.Serializable]
public class LootGeneType
{
    public GeneTypeEntry geneType;
    [Range(0f, 1f)]
    public float lootPossibility;
}

[System.Serializable]
public class LootCultureMedium
{
    public int minLootCultureMedium;
    public int maxLootCultureMedium;
}
