using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        BulletMaker bulletMarker = other.GetComponent<BulletMaker>();
        if (bulletMarker != null)
        {
            HP enemyHP = GetComponent<HP>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(50f); // 假设伤害值是50
                Destroy(other.gameObject); // 销毁子弹
            }
            
        }
    }

}
