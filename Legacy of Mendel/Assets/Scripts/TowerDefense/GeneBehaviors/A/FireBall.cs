using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public HP.ObjectType objectType;
    
    public GameObject fireBallTarget;
    public float speed = 20f;

    public float instantDamage;
    public float dotDamage;
    public float burnDuration;
    public float burnTickInterval;
    public float explosionRange;

    void FixedUpdate()
    {
        if (fireBallTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        //old moving logic
        /*Vector3 direction = fireBallTarget.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);*/

        //fireball moving logic
        transform.LookAt(fireBallTarget.transform);
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    public void HitTarget()
    {
        DealInstantDamage(fireBallTarget, 1);
        SetTargetOnfire(fireBallTarget, 1);
    }

    private void DealInstantDamage(GameObject target, float damageModifier)
    {
        float damageToDeal = Mathf.Max(instantDamage * damageModifier, instantDamage * 0.5f); // Ensuring minimum 50% damage
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            targetHP.TakeDamage(damageToDeal);
        }
    }

    private void SetTargetOnfire(GameObject target, float damageModifier)
    {
        float damageToDeal = Mathf.Max(dotDamage * damageModifier, dotDamage * 0.5f); // Ensuring minimum 50% damage
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            BurningState burningState = target.GetComponent<BurningState>();
            if (burningState == null) // Not burning yet.
            {
                target.AddComponent<BurningState>().StartBurning(damageToDeal, burnDuration, burnTickInterval);
            }
            else if (damageToDeal > burningState.CurrentBurnDamage) // New damage is stronger.
            {
                burningState.RefreshBurning(damageToDeal, burnDuration);
            }
            else if (damageToDeal <= burningState.CurrentBurnDamage)
            {
                burningState.RefreshBurning(burningState.CurrentBurnDamage, burnDuration);
            }
        }
    }

    public void AOETargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRange);

        foreach (Collider hitCollider in hitColliders)
        {
            GameObject enemy = hitCollider.gameObject;
            // Check if the GameObject has the tag "Enemy"
            if (enemy.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                float damageModifier = 1 - (distance / explosionRange);
                
                DealInstantDamage(enemy, damageModifier);
                SetTargetOnfire(enemy, damageModifier);
            }
        }
    }
}
