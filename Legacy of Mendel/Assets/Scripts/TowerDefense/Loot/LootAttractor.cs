using UnityEngine;

public class LootAttractor : MonoBehaviour
{
    public float pullRadius = 5f;
    public float pullSpeed = 5f;

    public static LootAttractor Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 检测pullRadius范围内所有的Collider
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);
        foreach (Collider collider in colliders)
        {
            // 检查Collider是否为Loot
            if (collider.CompareTag("Loot"))
            {
                // 计算从Loot指向飞碟的方向
                Vector3 direction = (transform.position - collider.transform.position).normalized;
                // 移动Loot沿这个方向
                collider.transform.position += direction * pullSpeed * Time.deltaTime;
            }
        }
    }
}
