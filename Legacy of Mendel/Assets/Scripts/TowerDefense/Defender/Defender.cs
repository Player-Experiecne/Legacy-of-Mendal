using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Defender
{
    public GameObject defenderPrefab;
    [Header("Stats")]
    public float hp;
    public float attackPower;
    public float attackRange;
    public float attackSpeed;
    public float Speed;
    [Header("GeneType")]
    public GeneTypeAInfoSO.GeneTypeA geneTypeA;
    [Header("UI Info")]
    public string defenderName;
    public Sprite defenderImage;
}
