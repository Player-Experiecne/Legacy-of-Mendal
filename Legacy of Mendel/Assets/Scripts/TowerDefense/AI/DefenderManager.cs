using System.Collections.Generic;
using UnityEngine;

public class DefenderManager : MonoBehaviour
{
    public static DefenderManager Instance;

    public List<GameObject> defenders = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void RegisterDefender(GameObject defender)
    {
        defenders.Add(defender);
    }

    public void UnregisterDefender(GameObject defender)
    {
        defenders.Remove(defender);
    }
}
