using UnityEngine;

[CreateAssetMenu(fileName = "GeneTypeAInfo", menuName = "Genes/Gene Type A Info")]
public class GeneTypeAInfoSO : ScriptableObject
{
    public enum GeneTypeA
    {
        Null, ADom, AHet, ARec
    }

    public GeneData ADom;
    public GeneData AHet;
    public GeneData ARec;

    [System.Serializable]
    public class GeneData
    {
        //public Level.GeneTypeA geneType;
        [Range(0f, 1f)] public float occurrencePossibility;
    }
}
