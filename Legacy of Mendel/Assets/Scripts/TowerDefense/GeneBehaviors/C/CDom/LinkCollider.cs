using UnityEngine;

public class LinkCollider : MonoBehaviour
{
    public Transform orbA; // Assign the first Light Orb transform
    public Transform orbB; // Assign the second Light Orb transform

    private CapsuleCollider capsuleCollider;
    private float instantDamage;
    private HP selfHP;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        instantDamage = transform.GetComponentInParent<LightOrbGroup>().instantDamage;
        selfHP = transform.GetComponentInParent<LightOrbGroup>().selfHP;
    }

    void Update()
    {
        if (orbA == null || orbB == null) return;

        // Calculate the midpoint between the two orbs
        Vector3 midPoint = (orbA.position + orbB.position) / 2;
        transform.position = midPoint;

        // Calculate the direction vector from Orb B to Orb A
        Vector3 directionBA = (orbA.position - orbB.position).normalized;

        // Set the collider's rotation to match this direction
        // Create a Quaternion rotation from the direction vector, assuming the forward axis is Z
        transform.rotation = Quaternion.LookRotation(directionBA);

        // Update the capsule collider's height to match the distance between the orbs
        // Assuming the capsule's local Y-axis is aligned with its length
        float distance = Vector3.Distance(orbA.position, orbB.position);
        capsuleCollider.height = distance + 0.52f;

        // Optionally, adjust the collider's radius or other properties based on your needs
    }
    private void OnTriggerEnter(Collider other)
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
                DealInstantDamage(target);
            }
        }
        else if (selfHP.objectType == HP.ObjectType.Defender)
        {
            if (target.CompareTag("Enemy"))
            {
                DealInstantDamage(target);
            }
        }
    }
}
