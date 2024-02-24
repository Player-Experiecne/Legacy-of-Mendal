using System.Collections;
using UnityEngine;

public class BurningState : MonoBehaviour
{
    private float burnDamage;
    private float burnDuration;
    private float burnTickInterval;

    private float elapsed;

    public float CurrentBurnDamage => burnDamage;
    public float CurrentBurnTickInterval => burnTickInterval;

    public void StartBurning(float damage, float duration, float tickInterval)
    {
        burnDamage = damage;
        burnDuration = duration;
        burnTickInterval = tickInterval;
        elapsed = 0;

        StopAllCoroutines();
        StartCoroutine(Burn());
    }

    private IEnumerator Burn()
    {
        HP targetHP = GetComponent<HP>();
        if (!targetHP)
        {
            Debug.LogError("HP component missing on the burning object.", this);
            yield break;
        }

        while (elapsed < burnDuration)
        {
            yield return new WaitForSeconds(burnTickInterval);
            targetHP.TakeDamage(burnDamage);
            elapsed += burnTickInterval;
        }

        Destroy(this); // Destroy this script after burning effect is over.
    }
}
