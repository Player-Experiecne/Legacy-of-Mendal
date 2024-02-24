using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static HP;

public class IceAttack : MonoBehaviour
{
    //Damage Settings
    public float instantDamage;

    //Slow Settings
    public float slowRatio;
    public float slowDuration;

    public HP selfHP;
    private Collider triggerCollider;
    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();

    void Awake()
    {
        triggerCollider = GetComponent<MeshCollider>();
        StartCoroutine(DetectEnemies());
    }
    
    private IEnumerator DetectEnemies()
    {
        yield return new WaitForSeconds(0.4f);
        triggerCollider.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        HitCorrectTargetType(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if(damagedEnemies.Contains(other.gameObject))
        {
            damagedEnemies.Remove(other.gameObject);
        }
    }

    private void HitCorrectTargetType(GameObject target)
    {
        if (selfHP.objectType == ObjectType.Enemy)
        {
            if (target.CompareTag("Base") || target.CompareTag("Defender"))
            {
                HitUndamagedTarget(target);
            }
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            if (target.CompareTag("Enemy"))
            {
                HitUndamagedTarget(target);
            }
        }
    }
    private void DealInstantDamage(GameObject target)
    {
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            targetHP.TakeDamage(instantDamage);
        }
    }

    private void HitUndamagedTarget(GameObject target)
    {
        if (!damagedEnemies.Contains(target))
        {
            damagedEnemies.Add(target);
            DealInstantDamage(target);
            SlowTarget(target);
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
