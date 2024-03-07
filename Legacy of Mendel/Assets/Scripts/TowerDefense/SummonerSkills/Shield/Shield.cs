using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "SummonerSkill/Shield")]
public class Shield : SummonerSkill
{
    [Header("Stats")]
    public float shieldStrength = 100f; // The amount of damage the shield can absorb
    public float shieldDuration = 10f; // Duration of the shield in seconds

    [Header("VFX Prefab")]
    public GameObject spawnVFXPrefab;
    public GameObject shieldVFXPrefab;
    private GameObject shieldVFXInstance;
    public override void Activate()
    {
        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            SpawnEffects(baseObject);
            
            ShieldEffect shieldEffect = baseObject.AddComponent<ShieldEffect>();

            // Activate or refresh the shield
            shieldEffect.ActivateShield(shieldStrength, shieldDuration, shieldVFXInstance);
        }
        else
        {
            Debug.LogError("Base object not found.");
        }
    }

    private void SpawnEffects(GameObject target)
    {
        Instantiate(spawnVFXPrefab, target.transform.position, Quaternion.identity, target.transform);
        shieldVFXInstance = Instantiate(shieldVFXPrefab, target.transform.position + new Vector3(0,5,0), Quaternion.identity, target.transform);
    }
}


