using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance { get; private set; }
    public GameObject lootGenePrefab;
    public GameObject lootCultureMediumPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        switch (lootGeneType.geneName)
        {
            case GeneInfo.geneTypesName.A:
                switch (lootGeneType.geneType)
                {
                    case GeneInfo.geneTypes.Dom:
                        lootController.lootType = LootController.LootType.LootGeneADom;
                        break;
                    case GeneInfo.geneTypes.Het:
                        lootController.lootType = LootController.LootType.LootGeneAHet;
                        break;
                    case GeneInfo.geneTypes.Rec:
                        lootController.lootType = LootController.LootType.LootGeneARec;
                        break;
                }break;
            case GeneInfo.geneTypesName.B:
                switch (lootGeneType.geneType)
                {
                    case GeneInfo.geneTypes.Dom:
                        lootController.lootType = LootController.LootType.LootGeneBDom;
                        break;
                    case GeneInfo.geneTypes.Het:
                        lootController.lootType = LootController.LootType.LootGeneBHet;
                        break;
                    case GeneInfo.geneTypes.Rec:
                        lootController.lootType = LootController.LootType.LootGeneBRec;
                        break;
                }
                break;
            case GeneInfo.geneTypesName.C:
                switch (lootGeneType.geneType)
                {
                    case GeneInfo.geneTypes.Dom:
                        lootController.lootType = LootController.LootType.LootGeneCDom;
                        break;
                    case GeneInfo.geneTypes.Het:
                        lootController.lootType = LootController.LootType.LootGeneCHet;
                        break;
                    case GeneInfo.geneTypes.Rec:
                        lootController.lootType = LootController.LootType.LootGeneCRec;
                        break;
                }
                break;
        }
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
        float radius = 5.0f; // Adjust as needed
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0; // Assuming a 2D game, or you don't want to change the height
        Vector3 dropPosition = transform.position + randomDirection;
        return dropPosition;
    }
}
