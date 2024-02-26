using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static HP;

public class WaterAttack : MonoBehaviour
{
    //Damage Settings
    public float damagePerHit;
    public float damageInterval;

    //Slow Settings
    public float slowRatio;
    public float slowDuration;

    public HP selfHP;

    private Dictionary<Collider, float> lastDamageTimes = new Dictionary<Collider, float>();

    private void OnTriggerStay(Collider other)
    {
        if (selfHP.objectType == ObjectType.Enemy)
        {
            if (other.CompareTag("Base") || other.CompareTag("Defender"))
            {
                //Slow target anyway
                SlowTarget(other.gameObject);
                // If the target is not in the dictionary, initialize its last damage time
                if (!lastDamageTimes.ContainsKey(other))
                {
                    DealDamage(other.gameObject);
                    lastDamageTimes[other] = Time.time;
                }

                // Check if enough time has passed to damage this specific target again
                if (Time.time - lastDamageTimes[other] >= damageInterval)
                {
                    DealDamage(other.gameObject);
                    lastDamageTimes[other] = Time.time; // Update the last damage time for this target
                }
            }
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            if (other.CompareTag("Enemy"))
            {
                //Slow target anyway
                SlowTarget(other.gameObject);
                // If the target is not in the dictionary, initialize its last damage time
                if (!lastDamageTimes.ContainsKey(other))
                {
                    DealDamage(other.gameObject);
                    lastDamageTimes[other] = Time.time;
                }

                // Check if enough time has passed to damage this specific target again
                if (Time.time - lastDamageTimes[other] >= damageInterval)
                {
                    DealDamage(other.gameObject);
                    lastDamageTimes[other] = Time.time; // Update the last damage time for this target
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (lastDamageTimes.ContainsKey(other))
        {
            lastDamageTimes.Remove(other);
        }
    }

    private void DealDamage(GameObject target)
    {
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            targetHP.TakeDamage(damagePerHit);
        }
    }
    private void SlowTarget(GameObject target)
    {
        NavMeshAgent targetNav = target.GetComponent<NavMeshAgent>();
        if (targetNav != null)
        {
            SlowState slowState = target.GetComponent<SlowState>();
            if (slowState == null) // Not slowed yet.
            {
                target.AddComponent<SlowState>().StartSlowed(slowRatio, slowDuration);
            }
            else if (slowRatio > slowState.existingSlowRatio) // New slow is stronger.
            {
                slowState.StartSlowed(slowRatio, slowDuration);
            }
            else if (slowRatio <= slowState.existingSlowRatio) // Existing slow is stronger
            {
                slowState.StartSlowed(slowState.existingSlowRatio, slowDuration);
            }
        }
    }
}
