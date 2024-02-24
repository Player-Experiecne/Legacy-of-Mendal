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
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthBar()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    public void Die()
    {
        switch (objectType)
        {
            case ObjectType.Enemy:
                Destroy(gameObject);
                lootManager.DropLootGeneType(transform, enemyController.lootGeneType);
                lootManager.DropLootCultureMedium(transform, enemyController.lootCultureMedium);
                break;
            case ObjectType.Defender:
                Destroy(gameObject);
                break;
            case ObjectType.Base:
                GameManager.Instance.callOnButton();
                GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject obj in objectsWithTag)
                {
                    Destroy(obj);
                }
                gameObject.SetActive(false);
                Debug.Log("Game Over");
                break;
        }
    }
}
