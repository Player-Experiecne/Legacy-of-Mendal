using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialBuildManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TutorialDefenderBackpack defenderBackpack;
    public GameObject bulletPrefab;
    private Defender activeDefender = null;
    private AddBehaviorsToTarget add;
    private bool hasPlacedDefender = false; // 新增变量

    private void Start()
    {
        add = GetComponent<AddBehaviorsToTarget>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                PlaceDefender(hit.point);
            }
        }
    }

    void PlaceDefender(Vector3 position)
    {
        if (hasPlacedDefender)
        {
            Debug.Log("A defender has already been placed.");
            return;
        }

        activeDefender = defenderBackpack.activeDefender;
        if (activeDefender == null)
        {
            Debug.Log("No defender selected");
        }
        else if (activeDefender.defenderPrefab == null)
        {
            Debug.Log("Defender's prefab is not set! Defender name: " + activeDefender.defenderName);
        }
        else
        {
            Transform closestEnemy = FindClosestEnemy(position);
            if (closestEnemy != null)
            {
                // 计算朝向最近敌人的旋转
                Vector3 direction = closestEnemy.position - position;
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                // 实例化并设置初始旋转使之面向最近的敌人
                GameObject spawnedDefender = Instantiate(activeDefender.defenderPrefab, position, lookRotation);

                // 添加和配置BulletShooter组件
                BulletShooter shooter = spawnedDefender.AddComponent<BulletShooter>();
                shooter.bulletPrefab = bulletPrefab;
                shooter.bulletSpawnPoint = spawnedDefender.transform;
                shooter.target = closestEnemy;

                defenderBackpack.RemoveDefenderFromBackpack(activeDefender);
                hasPlacedDefender = true;
            }
        }
    }

    public Transform FindClosestEnemy(Vector3 position)
    {
        // 找到所有的敌人
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // 遍历所有敌人并找到距离最近的
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }
    public bool HasPlacedDefender()
    {
        return hasPlacedDefender;
    }

    // 在需要的时候可以调用这个方法来重置放置状态
    public void ResetPlacedDefenderStatus()
    {
        hasPlacedDefender = false;
    }
}
