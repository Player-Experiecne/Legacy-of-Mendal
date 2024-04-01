using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HP : MonoBehaviour
{
    [SerializeField]
    public ObjectType objectType;

    public enum ObjectType
    {
        Enemy,
        Defender,
        Base
    }

    public float maxHealth = 100;
    [HideInInspector]
    public float currentHealth;
    public Image healthBarFill;
    public Image shieldBarFill;
    private bool isDead = false;

    LootManager lootManager;
    private EnemyController enemyController;
    private DefenderController defenderController;
    void Start()
    {
        lootManager = LootManager.Instance;
        switch (objectType)
        {
            case ObjectType.Enemy:
                enemyController = GetComponent<EnemyController>();
                break;
            case ObjectType.Defender:
                defenderController = GetComponent<DefenderController>();
                break;
        }

        currentHealth = maxHealth;
        healthBarFill.fillOrigin = 1;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) // Check if Die has already been called
            return; // If so, do nothing more in this method

        ShieldEffect shieldEffect = GetComponent<ShieldEffect>();
        if (shieldEffect != null)
        {
            shieldEffect.TakeDamage(damage);
        }
        else
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthBar();

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void UpdateHealthBar()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;

        if (shieldBarFill != null)
        {
            ShieldEffect shieldEffectComponent = GetComponent<ShieldEffect>();
            float shieldStrength = shieldEffectComponent != null ? shieldEffectComponent.shieldStrength : 0;

            // Calculate the fill amount relative to the max health for visual consistency
            shieldBarFill.fillAmount = shieldStrength / maxHealth;

            RectTransform shieldBarRT = shieldBarFill.GetComponent<RectTransform>();
            RectTransform healthBarRT = healthBarFill.GetComponent<RectTransform>();

            if (shieldBarRT != null && healthBarRT != null)
            {
                // Adjust for the rotation: we're effectively seeing the UI mirrored.
                // Calculate the new position based on the health bar's current fill amount.
                // This positions the shield bar to "extend" to the right from the viewer's perspective,
                // but due to the rotation, it's technically extending left in the local frame.

                // Calculate offset for the shield bar based on health fill amount
                // Since the UI is rotated, "extending to the right" means moving left in local space.
                float offsetX = (1 - healthBarFill.fillAmount) * healthBarRT.rect.width;

                // Set the shield bar's anchored position to start where the health bar fill ends,
                // taking into account the rotation flip.
                float newPositionX = offsetX;
                shieldBarRT.anchoredPosition = new Vector2(newPositionX, 0);
            }
        }
    }
    public void Die()
    {
        if (isDead) // Double-check to prevent multiple executions
            return;
        isDead = true;
        switch (objectType)
        {
            case ObjectType.Enemy:
                Destroy(gameObject);
                if(enemyController.lootGeneType != null && enemyController.lootGeneType.geneType != GeneInfo.geneTypes.Null)
                {
                    lootManager.DropLootGeneType(transform, enemyController.lootGeneType);
                }
                lootManager.DropLootCultureMedium(transform, enemyController.lootCultureMedium);
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
                foreach (GameObject bullet in bullets)
                {
                    Destroy(bullet); // 销毁子弹
                }
                break;
            case ObjectType.Defender:
                Destroy(gameObject);
                break;
            case ObjectType.Base:
                GameEvents.TriggerLevelFail();
                gameObject.SetActive(false);
                break;
        }
    }
}
