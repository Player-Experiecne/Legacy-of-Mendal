using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletForTutorial : MonoBehaviour
{
    public float damage = 50f; // 可以在Inspector中设置
    void Start()
    {
        Debug.Log("Bullet instantiated, heading towards the target.");
    }

    void Update()
    {
        // Debug.Log输出当前位置，观察子弹是否在移动
        Debug.Log(transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        // 尝试获取碰撞物体的HP组件
        HP hp = other.GetComponent<HP>();
        if (hp != null && hp.objectType == HP.ObjectType.Enemy)
        {
            // 对敌人造成伤害
            hp.TakeDamage(damage);
        }

        // 不管碰撞到什么，子弹都销毁
        Destroy(gameObject);
    }
}

