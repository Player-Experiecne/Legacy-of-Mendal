using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "SummonerSkill/Heal")]
public class Heal : SummonerSkill
{
    [Header("Stats")]
    public float healingPercentage = 0.3f; // 30%
    public float range = 20f; // Healing range, adjust as needed

    [Header("VFX Prefab")]
    public GameObject VFXPrefab;

    public override void Activate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // Player not found, exit

        // Proceed with the original logic, using player.transform.position
        foreach (var defender in DefenderManager.Instance.defenders)
        {
            if (Vector3.Distance(defender.transform.position, player.transform.position) <= range)
            {
                HealTarget(defender);
                SpawnEffect(defender);
            }
        }
        SoundManager.Instance.PlaySFX(SoundEffect.SummonerHeal);
        Debug.Log("Healing Aura activated!");
    }

    private void HealTarget(GameObject target)
    {
        HP hpComponent = target.GetComponent<HP>();
        if (hpComponent != null && hpComponent.objectType == HP.ObjectType.Defender)
        {
            hpComponent.currentHealth += hpComponent.maxHealth * healingPercentage;
            hpComponent.currentHealth = Mathf.Min(hpComponent.currentHealth, hpComponent.maxHealth);
            hpComponent.UpdateHealthBar();
        }
    }
    private void SpawnEffect(GameObject target)
    {
        Instantiate(VFXPrefab, target.transform.position, Quaternion.identity, target.transform);
    }
}