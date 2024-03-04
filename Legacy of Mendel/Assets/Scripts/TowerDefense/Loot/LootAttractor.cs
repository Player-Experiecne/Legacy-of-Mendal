using UnityEngine;

public class LootAttractor : MonoBehaviour
{
    public float pullRadius = 5f; 
    public float pullSpeed = 5f; 

    void Update()
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);
        foreach (Collider collider in colliders)
        {
            
            if (collider.CompareTag("Loot"))
            {
                
                Vector3 direction = (transform.position - collider.transform.position).normalized;
                collider.transform.position += direction * pullSpeed * Time.deltaTime;
            }
        }
    }
}
