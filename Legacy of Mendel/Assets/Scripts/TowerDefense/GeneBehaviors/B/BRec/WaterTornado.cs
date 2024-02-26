using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaterTornado : MonoBehaviour
{
    //Damage Settings
    public float instantDamage;

    //Push Settings
    public float pushDuration;
    public float pushDistance;

    //Tornado Move Settings
    public float tornadoMoveDistance;
    private float moveSpeed;

    public HP selfHP;
    private Collider triggerCollider;
    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();

    private float turnOffTriggerAfterSeconds = 1.6f;
    private float destroyAfterSeconds = 1.7f;
    void Start()
    {
        moveSpeed = tornadoMoveDistance / destroyAfterSeconds;
        triggerCollider = GetComponent<Collider>();
        StartCoroutine(SelfDestruction());
    }

    private void Update()
    {
        // Move the tornado forward at the calculated speed
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private IEnumerator SelfDestruction()
    {
        //yield return new WaitForEndOfFrame();
        //triggerCollider.enabled = true;
        yield return new WaitForSeconds(turnOffTriggerAfterSeconds);
        triggerCollider.enabled = false;
        yield return new WaitForSeconds(destroyAfterSeconds);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        HitCorrectTargetType(other.gameObject);
    }


    private void DealInstantDamage(GameObject target)
    {
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            targetHP.TakeDamage(instantDamage);
        }
    }

    private void HitCorrectTargetType(GameObject target)
    {
        if (selfHP.objectType == HP.ObjectType.Enemy)
        {
            if (target.CompareTag("Base") || target.CompareTag("Defender"))
            {
                HitUndamagedTarget(target);
            }
        }
        else if (selfHP.objectType == HP.ObjectType.Defender)
        {
            if (target.CompareTag("Enemy"))
            {
                HitUndamagedTarget(target);
            }
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
        targetFreezeNew.freezeDuration = pushDuration;
        if (target != null && target.activeInHierarchy)
        {
            targetFreezeNew.StartFreeze();
        }
    }

    private void HitUndamagedTarget(GameObject target)
    {
        if (!damagedEnemies.Contains(target))
        {
            damagedEnemies.Add(target);
            DealInstantDamage(target);
            FreezeTarget(target);
            StartCoroutine(PushTarget(target));
        }
    }

    private IEnumerator PushTarget(GameObject target)
    {
        NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
        if (gameObject != null && gameObject.activeInHierarchy && agent != null && agent.enabled && agent.isOnNavMesh)
        {
            Vector3 pushDirection = (agent.transform.position - transform.position).normalized;
            Vector3 pushTarget = agent.transform.position + pushDirection * pushDistance;

            float startTime = Time.time;

            while (Time.time < startTime + pushDuration)
            {
                if (agent != null) // Check if the agent hasn't been destroyed
                {
                    // Manually set the agent's position towards the push target
                    agent.transform.position = Vector3.MoveTowards(agent.transform.position, pushTarget, Time.deltaTime * (pushDistance / pushDuration));
                }
                yield return null;
            }
        }
    }
}
