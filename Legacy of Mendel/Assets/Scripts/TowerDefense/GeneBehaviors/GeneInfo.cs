using System;
using System.Collections.Generic;

public class GeneInfo
{
 
    public enum geneTypesName
    {
        Null, A, B, C
    }
    public enum geneTypes
    {
        Null, Dom, Het, Rec
    }
    public List<GeneTypeEntry> genes = new List<GeneTypeEntry>();

    public GeneInfo()
    {
        // 初始化三个基因型
        genes.Add(new GeneTypeEntry { geneName = geneTypesName.A, geneType = geneTypes.Null });
        genes.Add(new GeneTypeEntry { geneName = geneTypesName.B, geneType = geneTypes.Null });
        genes.Add(new GeneTypeEntry { geneName = geneTypesName.C, geneType = geneTypes.Null });
    }
}
