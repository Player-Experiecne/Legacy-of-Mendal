using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Gene Library", menuName = "Gene Library/New Gene Library")]
public class GeneLibrary : ScriptableObject
{
    public List<GeneTypeData> allGenes = new List<GeneTypeData>();

    public event Action OnGeneLibraryUpdated;

    // 在基因库更新时调用这个方法
    public void UpdateGeneLibrary()
    {
        // ... 基因库更新的逻辑 ...

        // 触发事件，通知所有订阅者
        OnGeneLibraryUpdated?.Invoke();
    }
}
