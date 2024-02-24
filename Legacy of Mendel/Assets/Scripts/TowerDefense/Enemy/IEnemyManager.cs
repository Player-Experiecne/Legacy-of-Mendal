using System.Collections.Generic;
using UnityEngine;

public interface IEnemyManager
{
    void RegisterEnemy(GameObject enemy);
    void UnregisterEnemy(GameObject enemy);
    void ResetEnemyCount();
    List<GameObject> Enemies { get;}
}
