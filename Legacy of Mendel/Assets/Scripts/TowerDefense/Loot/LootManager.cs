using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

    public void DropLootGeneType(Transform sourceTransform, GeneTypeEntry lootGeneType)
    {
        Vector3 lastPosition = sourceTransform.position;
        GameObject lootGameObject = Instantiate(lootGenePrefab, lastPosition, Quaternion.identity);
        LootController lootController = lootGameObject.AddComponent<LootController>();
        lootController.lootType = TranscribeLootInfo(lootGeneType);
        StartCoroutine(MoveLootToRandomPosition(lootGameObject, lastPosition));
    }

    public void DropLootCultureMedium(Transform sourceTransform, int lootCultureMedium)
    {
        Vector3 lastPosition = sourceTransform.position;
        for (int i = 0; i < lootCultureMedium; i++)
        {
            GameObject loot = Instantiate(lootCultureMediumPrefab, lastPosition, Quaternion.identity);
            StartCoroutine(MoveLootToRandomPosition(loot, lastPosition));
        }
    }

    private IEnumerator MoveLootToRandomPosition(GameObject loot, Vector3 startPosition)
    {
        Vector3 randomHorizontalDirection = Random.insideUnitCircle * 4f; // Explosion radius in horizontal plane
        float initialHeight = 1.0f; // Initial height for the parabolic motion
        Vector3 endPosition = startPosition + new Vector3(randomHorizontalDirection.x, 0, randomHorizontalDirection.y);

        float timeToMove = 0.75f; // Duration of the motion
        float elapsed = 0;
        float peakHeight = 3.0f; // Maximum height the loot will reach

        while (elapsed < timeToMove)
        {
            float verticalPosition = -4 * peakHeight / (timeToMove * timeToMove) * (elapsed - timeToMove / 2) * (elapsed - timeToMove / 2) + peakHeight;
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, elapsed / timeToMove);
            currentPosition.y += verticalPosition + initialHeight; // Apply the vertical component

            loot.transform.position = currentPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        loot.transform.position = new Vector3(endPosition.x, startPosition.y, endPosition.z); // Ensure it lands back at ground level
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
