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
    [Header("GeneTypes")]
    public List<GeneInfo.geneTypes> geneTypes;
    [Header("UI Info")]
    public string defenderName;
    public Sprite defenderImage;
  
    public Defender(Defender other)
    {
        this.defenderPrefab = other.defenderPrefab;
        this.geneTypes = new List<GeneInfo.geneTypes>(other.geneTypes);
        this.hp = other.hp;
        this.attackPower = other.attackPower;
        this.attackRange = other.attackRange;
        this.attackSpeed = other.attackSpeed;
        this.Speed = other.Speed;
        this.defenderName = other.defenderName;
        this.defenderImage = other.defenderImage;
        

    }

}
