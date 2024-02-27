using UnityEngine;
using System.Collections.Generic;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance { get; private set; }
    public GameObject lootGenePrefab;
    public GameObject lootCultureMediumPrefab;

    public enum LootType
    {
        Null,
        LootCultureMedium,
        LootGeneADom,
        LootGeneAHet,
        LootGeneARec,
        LootGeneBDom,
        LootGeneBHet,
        LootGeneBRec,
        LootGeneCDom,
        LootGeneCHet,
        LootGeneCRec,
    }

    void Awake()
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

    public void DropLootGeneType(Transform transform, GeneTypeEntry lootGeneType)
    {
        Vector3 dropPosition = GetRandomDropPosition(transform);
        GameObject lootGameObject = Instantiate(lootGenePrefab, dropPosition, Quaternion.identity);
        LootController lootController = lootGameObject.AddComponent<LootController>();
        lootController.lootType = TranscribeLootInfo(lootGeneType);
    }

    public void DropLootCultureMedium(Transform transform, int lootCultureMedium)
    {
        for (int i = 0; i < lootCultureMedium; i++)
        {
            Vector3 dropPosition1 = GetRandomDropPosition(transform);
            Instantiate(lootCultureMediumPrefab, dropPosition1, Quaternion.identity);
        }
    }

    private Vector3 GetRandomDropPosition(Transform transform)
    {
        float radius = 2.0f; // Adjust as needed
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0; // Assuming a 2D game, or you don't want to change the height
        Vector3 dropPosition = transform.position + randomDirection;
        return dropPosition;
    }

    private LootType TranscribeLootInfo(GeneTypeEntry lootGeneType)
    {
        switch (lootGeneType.geneName)
        {
            case GeneInfo.geneTypesName.A:
                switch (lootGeneType.geneType)
                {
                    case GeneInfo.geneTypes.Dom:
                        return LootType.LootGeneADom;
                    case GeneInfo.geneTypes.Het:
                        return LootType.LootGeneAHet;
                    case GeneInfo.geneTypes.Rec:
                        return LootType.LootGeneARec;
                }
                break;
            case GeneInfo.geneTypesName.B:
                switch (lootGeneType.geneType)
                {
                    case GeneInfo.geneTypes.Dom:
                        return LootType.LootGeneBDom;
                    case GeneInfo.geneTypes.Het:
                        return LootType.LootGeneBHet;
                    case GeneInfo.geneTypes.Rec:
                        return LootType.LootGeneBRec;
                }
                break;
            case GeneInfo.geneTypesName.C:
                switch (lootGeneType.geneType)
                {
                    case GeneInfo.geneTypes.Dom:
                        return LootType.LootGeneCDom;
                    case GeneInfo.geneTypes.Het:
                        return LootType.LootGeneCHet;
                    case GeneInfo.geneTypes.Rec:
                        return LootType.LootGeneCRec;
                }
                break;
        }
        return LootType.Null;
    }

}
