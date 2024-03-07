using UnityEngine;

[CreateAssetMenu(fileName = "Smite", menuName = "SummonerSkill/Smite")]
public class Smite : SummonerSkill
{
    [Header("Stats")]
    public float healingPercentage = 0.3f; // 30%
    public float range = 20f; // Healing range, adjust as needed

    public override void Activate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // Player not found, exit

        // Proceed with the original logic, using player.transform.position
        foreach (var defender in DefenderManager.Instance.defenders)
        {
            if (Vector3.Distance(defender.transform.position, player.transform.position) <= range)
            {
                HP hpComponent = defender.GetComponent<HP>();
                if (hpComponent != null && hpComponent.objectType == HP.ObjectType.Defender)
                {
                    hpComponent.currentHealth += hpComponent.maxHealth * healingPercentage;
                    hpComponent.currentHealth = Mathf.Min(hpComponent.currentHealth, hpComponent.maxHealth);
                    hpComponent.UpdateHealthBar();
                }
            }
        }
        Debug.Log("Healing Aura activated!");
    }
}