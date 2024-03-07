using UnityEngine;
using System.Collections;

public class ShieldEffect : MonoBehaviour
{
    private HP hpComponent;
    public float shieldStrength { get; private set; } // Expose the remaining shield strength
    private float shieldDuration;
    private GameObject shieldVFXInstance;

    void Awake()
    {
        hpComponent = GetComponent<HP>();
        if (hpComponent == null)
        {
            Debug.LogError("ShieldEffect requires an HP component on the same GameObject.", this);
        }
    }

    public void ActivateShield(float strength, float duration, GameObject VFXInstance)
    {
        this.shieldStrength = strength;
        this.shieldDuration = duration;
        this.shieldVFXInstance = VFXInstance;

        if (shieldDuration > 0)
        {
            // Start the coroutine to remove the shield after its duration
            StartCoroutine(ShieldDurationCountdown());
        }
    }

    private IEnumerator ShieldDurationCountdown()
    {
        hpComponent.UpdateHealthBar();
        yield return new WaitForSeconds(shieldDuration);
        // Once the duration ends, you can optionally make adjustments or notify that the shield is down
        // For example, reset shieldStrength to 0 or notify via an event
        shieldStrength = 0;
        hpComponent.UpdateHealthBar();
        Destroy(this);
        Destroy(shieldVFXInstance);
    }

    // Call this method to handle incoming damage against the shield
    public void TakeDamage(float damage)
    {
        if (shieldStrength > 0)
        {
            shieldStrength -= damage;

            if (shieldStrength <= 0)
            {
                float overflowDamage = -shieldStrength;
                shieldStrength = 0;
                hpComponent.UpdateHealthBar();
                Destroy(this);
                Destroy(shieldVFXInstance);
                // If there's any damage that exceeds the shield, apply it to the health
                if (overflowDamage > 0)
                {
                    hpComponent.TakeDamage(overflowDamage);
                }
            }
            hpComponent.UpdateHealthBar();
        }
    }
}
