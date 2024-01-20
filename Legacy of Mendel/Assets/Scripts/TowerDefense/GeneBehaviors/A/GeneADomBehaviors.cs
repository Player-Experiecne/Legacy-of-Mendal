using System.Collections;
using UnityEngine;
using static GetInfo;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(GetInfo))]
public class GeneADomBehaviors : MonoBehaviour
{
    [Header("Damage Settings")]
    public float instantDamage = 10f;    // Instant damage applied upon touch.
    public float dotDamage = 20f;         // Damage over time applied while burning.
    public float burnDuration = 5f;      // Duration of the burn effect.
    public float burnTickInterval = 1f;  // Time interval between damage ticks while burning.

    [Header("Fire")]
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float fireDuration = 1f;
    [SerializeField] private Collider fireTriggerCollider; // Assign this in the Editor.

    public GameObject firePrefab; // Declare a public GameObject for the fire prefab
    private string targetTag;
    private float nextFireTime = 0.0f;
    private float lastDamageTime = 0f;

    private void Awake()
    {
        GetInfo getInfo = GetComponent<GetInfo>();
        if (getInfo.aiType == AIType.Enemy)
        {
            targetTag = "Defender";
        }
        else if (getInfo.aiType == AIType.Defender)
        {
            targetTag = "Enemy";
        }

        fireTriggerCollider = GetComponent<Collider>();
        firePrefab = Resources.Load<GameObject>("Fire");
    }

    private void Update()
    {
        if (Time.time > nextFireTime)
        {
            StartCoroutine(SpewFire());
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    IEnumerator SpewFire()
    {
        fireTriggerCollider.enabled = true;

        // Instantiate the fire prefab at the front of the game object
        Vector3 spawnPosition = transform.position + transform.forward; // Adjust the offset if necessary
        GameObject fireInstance = Instantiate(firePrefab, spawnPosition, transform.rotation, transform);

        yield return new WaitForSeconds(fireDuration);

        Destroy(fireInstance); // Destroy the fire instance
        fireTriggerCollider.enabled = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag) && Time.time - lastDamageTime >= fireRate)
        {
            DealInstantDamage(other.gameObject);
            SetTargetOnfire(other.gameObject);
        }
    }

    private void DealInstantDamage(GameObject target)
    {
        Debug.Log("instant damage");
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            targetHP.TakeDamage(instantDamage);
        }
        lastDamageTime = Time.time;
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
}
