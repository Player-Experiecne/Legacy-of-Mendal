using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Defender
{
    public int defenderNumber;
    public GameObject defenderPrefab;
    public GeneTypeAInfoSO.GeneTypeA geneTypeA;
    [Header("UI Info")]
    public Sprite defenderImage;
}
