using UnityEngine;

[CreateAssetMenu(fileName = "Smite", menuName = "SummonerSkill/Smite")]
public class Smite : SummonerSkill
{
    [Header("Stats")]
    public float damagePerHit = 5f;
    public int damageHits = 10;
    private float range = 13f;
    private float duration = 2.2f;

    [Header("VFX Prefab")]
    public GameObject VFXPrefab;

    public override void Activate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // Player not found, exit

        // Assuming you spawn some VFX at the player's location for the thunderstorm
        SpawnEffect(player);

        // Apply burning effect to each enemy within range
        foreach (var enemy in EnemyManager.Instance.Enemies)
        {
            if (Vector3.Distance(enemy.transform.position, player.transform.position) <= range)
            {
                BurningState burningState = enemy.AddComponent<BurningState>();

                // Start or restart the burning effect on the enemy
                burningState.StartBurning(damagePerHit, duration, duration / damageHits);
            }
        }
        Debug.Log("Smite activated!");
    }

    private void SpawnEffect(GameObject target)
    {
        Instantiate(VFXPrefab, target.transform.position, Quaternion.identity, target.transform);
    }
}