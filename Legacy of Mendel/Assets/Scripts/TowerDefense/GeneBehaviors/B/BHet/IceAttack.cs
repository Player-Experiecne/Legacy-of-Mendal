using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static HP;

public class IceAttack : MonoBehaviour
{
    //Damage Settings
    public float instantDamage;
    public float attackDuration;

    //Freeze Settings
    public float freezeDuration;

    public HP selfHP;
    private Collider triggerCollider;
    private GameObject iceAttackObject;
    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();

    void Awake()
    {
        triggerCollider = GetComponent<MeshCollider>();
        iceAttackObject = transform.parent.gameObject;
        StartCoroutine(DetectEnemies());
    }
    
    private IEnumerator DetectEnemies()
    {
        yield return new WaitForSeconds(0.4f);
        triggerCollider.enabled = true;
        //Wait for attack duration and then stop the ice attack
        yield return new WaitForSeconds(attackDuration);
        Destroy(iceAttackObject);// Destroy the ice instance
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
            FreezeTarget(target);
        }
    }

    private void FreezeTarget(GameObject target)
    {
        FreezeState targetFreezeOld = target.GetComponent<FreezeState>();
        if (targetFreezeOld != null)
        {
            Destroy(targetFreezeOld);
        }
        FreezeState targetFreezeNew = target.AddComponent<FreezeState>();
        targetFreezeNew.freezeDuration = freezeDuration;
        if (target != null && target.activeInHierarchy)
        {
            targetFreezeNew.StartFreeze();
        }
    }
}
