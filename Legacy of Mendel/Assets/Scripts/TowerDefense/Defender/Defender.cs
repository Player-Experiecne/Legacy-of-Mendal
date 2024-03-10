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

    // 复制构造函数
    public Defender(Defender other)
    {
        this.defenderPrefab = other.defenderPrefab;

        // 进行深复制
        this.geneTypes = new List<GeneTypeEntry>();
        foreach (var geneTypeEntry in other.geneTypes)
        {
            // 假设GeneTypeEntry有正确的复制构造函数或克隆方法
            this.geneTypes.Add(new GeneTypeEntry(geneTypeEntry));
        }

        this.defenderName = other.defenderName;
        this.defenderImage = other.defenderImage;
    }
}
