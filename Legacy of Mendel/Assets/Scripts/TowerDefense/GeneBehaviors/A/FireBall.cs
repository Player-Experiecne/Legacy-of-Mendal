using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject target;
    public float speed = 20f;

    public float instantDamage;

    public float dotDamage;
    public float burnDuration;
    public float burnTickInterval;

    private void DealInstantDamage(GameObject target)
    {
        Debug.Log("instant damage");
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            targetHP.TakeDamage(instantDamage);
        }
    }

    private void SetTargetOnfire(GameObject target)
    {
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            BurningState burningState = target.GetComponent<BurningState>();
            if (burningState == null) // Not burning yet.
            {
                target.AddComponent<BurningState>().StartBurning(dotDamage, burnDuration, burnTickInterval);
            }
            else if (dotDamage > burningState.CurrentBurnDamage) // New damage is stronger.
            {
                burningState.RefreshBurning(dotDamage, burnDuration);
            }
            else if (dotDamage <= burningState.CurrentBurnDamage)
            {
                //burningState.RefreshBurning(burningState.CurrentBurnDamage, burnDuration);
            }
        }
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        DealInstantDamage(target);
        SetTargetOnfire(target);
        Destroy(gameObject); // Ïú»Ù×Óµ¯
    }
}
