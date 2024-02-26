using UnityEngine;
using UnityEngine.AI;
using static HP;

public class FreezeState : MonoBehaviour
{
    public float freezeDuration; // Duration of the freeze state in seconds

    private EnemyController enemyController;
    private DefenderController defenderController;
    private NavMeshAgent navMeshAgent;
    private HP selfHP;

    private bool isActive = true; // Flag to control coroutine execution

    void Awake()
    {
        selfHP = GetComponent<HP>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Depending on the objectType, get the relevant controller
        if (selfHP.objectType == ObjectType.Enemy)
        {
            enemyController = GetComponent<EnemyController>();
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            defenderController = GetComponent<DefenderController>();
        }
    }

    public void StartFreeze()
    {
        if (gameObject != null && gameObject.activeInHierarchy && navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            // Disable movement and attacking
            if (enemyController != null)
            {
                enemyController.isAttacking = false;
                enemyController.isFrozen = true;
                navMeshAgent.isStopped = true;
            }
            else if (defenderController != null)
            {
                defenderController.isAttacking = false;
                defenderController.isFrozen = true;
                navMeshAgent.isStopped = true;
            }
            // Start coroutine to wait for freeze duration before unfreezing
            StartCoroutine(UnfreezeAfterDelay(freezeDuration));
        }
    }

    System.Collections.IEnumerator UnfreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Check the flag before executing further logic
        if (!isActive) yield break;

        Unfreeze();
    }

    void Unfreeze()
    {
        if (!isActive) return;
        // Re-enable movement and attacking
        if (enemyController != null)
        {
            navMeshAgent.isStopped = false;
            enemyController.isFrozen = false;
        }
        else if (defenderController != null)
        {
            navMeshAgent.isStopped = false;
            defenderController.isFrozen = false;
        }

        // Destroy the FreezeState component if it's no longer needed
        Destroy(this);
    }

    void OnDestroy()
    {
        StopAllCoroutines(); // Stops all coroutines on this MonoBehaviour
        isActive = false;
    }
}
