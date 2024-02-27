using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Gene Database", menuName = "Gene Database/Gene Database")]
public class GeneDatabase : ScriptableObject
{
    public List<GeneTypeEntry> allGenes = new List<GeneTypeEntry>();

    public event Action OnGeneDatabaseUpdated;

    // 在基因库更新时调用这个方法
    public void UpdateGeneDatabase()
    {
        // ... 基因库更新的逻辑 ...

        // 触发事件，通知所有订阅者
        OnGeneDatabaseUpdated?.Invoke();
    }
}