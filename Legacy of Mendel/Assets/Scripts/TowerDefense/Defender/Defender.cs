using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Defender
{
    public GameObject defenderPrefab;

    [Header("GeneTypes")]
    public List<GeneTypeEntry> geneTypes = new List<GeneTypeEntry>();

    [Header("UI Info")]
    public string defenderName;
    public Sprite defenderImage;
    
    public Defender(Defender other)
    {
        this.defenderPrefab = other.defenderPrefab;
        
        this.geneTypes = new List<GeneTypeEntry>(other.geneTypes);
        this.defenderName = other.defenderName;
        this.defenderImage = other.defenderImage;
    }
}