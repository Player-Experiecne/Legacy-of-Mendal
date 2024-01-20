using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{

    public float maxHealth = 100;
    private float currentHealth;
    public Image healthBarFill;
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth == 0)
        {
            Debug.Log("Dead");
            Destroy(gameObject);
        }
    }

    void UpdateHealthBar()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }
}
