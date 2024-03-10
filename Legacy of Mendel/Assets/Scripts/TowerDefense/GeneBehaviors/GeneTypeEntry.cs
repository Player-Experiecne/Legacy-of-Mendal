using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneTypeEntry
{
    public GeneInfo.geneTypesName geneName;
    public GeneInfo.geneTypes geneType;
    public bool isOwned;

    // 无参数的构造函数
    public GeneTypeEntry() { }

    // 带有一个参数的构造函数
    public GeneTypeEntry(GeneTypeEntry other)
    {
        this.geneName = other.geneName;
        this.geneType = other.geneType;
        this.isOwned = other.isOwned;
    }
}
