using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gene Library", menuName = "Gene Library/New Gene Library")]
public class GeneLibrary : ScriptableObject
{
    public List<GeneTypeData> allGenes = new List<GeneTypeData>();
}
