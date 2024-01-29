using UnityEngine;

[CreateAssetMenu(fileName = "GeneTypeAInfo", menuName = "Genes/Gene Type A Info")]
public class GeneTypeAInfoSO : ScriptableObject
{
    public DomStats domStats;
    public HetStats hetStats;
    public RecStats recStats;

    [System.Serializable]
    public class DomStats
    {
        [Range(0f, 1f)] public float occurrencePossibility;

        [Header("Damage Settings")]
        public float instantDamage = 50f;    // Instant damage applied upon touch.
        public float dotDamage = 20f;         // Damage over time applied while burning.
        public float burnDuration = 5f;      // Duration of the burn effect.
        public float burnTickInterval = 1f;  // Time interval between damage ticks while burning.

        [Header("Fire Settings")]
        public GameObject firePrefab;
        public float fireInterval = 1f;
        public float fireDuration = 0.5f;
        public float fireRange = 6f;

    }

    [System.Serializable]
    public class HetStats
    {
        [Range(0f, 1f)] public float occurrencePossibility;

        [Header("Damage Settings")]
        public float instantDamage = 50f;    // Instant damage applied upon touch.
        public float dotDamage = 20f;         // Damage over time applied while burning.
        public float burnDuration = 5f;      // Duration of the burn effect.
        public float burnTickInterval = 1f;  // Time interval between damage ticks while burning.

        [Header("Fire Settings")]
        public GameObject firePrefab;
        public float fireInterval = 1f;
        public float fireDuration = 0.5f;
        public float fireRange = 6f;

    }

    [System.Serializable]
    public class RecStats
    {
        [Range(0f, 1f)] public float occurrencePossibility;

        [Header("Damage Settings")]
        public float instantDamage = 50f;    // Instant damage applied upon touch.
        public float dotDamage = 20f;         // Damage over time applied while burning.
        public float burnDuration = 5f;      // Duration of the burn effect.
        public float burnTickInterval = 1f;  // Time interval between damage ticks while burning.

        [Header("Fire Ball Settings")]
        public float fireBallInterval = 1f;
        public float fireBallRange = 50f;
        public float explosionRange = 5f;
        public GameObject fireBallPrefabForEnemies;
        public GameObject fireBallPrefabForDefenders;
    }
}
