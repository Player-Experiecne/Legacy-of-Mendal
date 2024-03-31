using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletShooter : MonoBehaviour
{
    public Transform target; // 目标
    public GameObject bulletPrefab; // 子弹预制体
    public Transform bulletSpawnPoint; // 子弹发射点
    public float shootingRate = 1f; // 发射率
    public float bulletSpeed = 10f; // 子弹速度

    private float shootingTimer;

    void Start()
    {
        shootingTimer = 0f;
    }

    void Update()
    {
        // 更新射击计时器
        shootingTimer += Time.deltaTime;

        // 如果达到发射率时间，发射子弹
        if (shootingTimer >= 1f / shootingRate)
        {
            Shoot();
            shootingTimer = 0f; // 重置计时器
        }
    }
    public void BulletHit(Collider collider)
    {
        HP enemyHP = collider.GetComponent<HP>();
        if (enemyHP != null && enemyHP.objectType == HP.ObjectType.Enemy)
        {
            // 对敌人造成伤害
            enemyHP.TakeDamage(50f); // 假设伤害值是50
        }

        // 子弹碰撞后销毁
        Destroy(gameObject);
    }
    void Shoot()
    {
        // 检查确保有子弹预制体和目标
        if (bulletPrefab != null && target != null && bulletSpawnPoint != null)
        {
            // 实例化子弹预制体
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

            // 计算子弹朝向目标的方向
            Vector3 direction = (target.position - bulletSpawnPoint.position).normalized;

            // 设置子弹的初始速度
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = direction * bulletSpeed;
            }

            // 因为子弹预制体上已经有了处理逻辑的脚本，这里不需要再进行任何额外的设置
        }
    }




}

