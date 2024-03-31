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
        // 先检查是否已经放置了defender
        if (hasPlacedDefender)
        {
            Debug.Log("A defender has already been placed.");
            return;
        }

        //Get active defender from action backpack
        activeDefender = defenderBackpack.activeDefender;
        // Check if there's an active defender to place
        if (activeDefender == null)
        {
            Debug.Log("No defender selected");
        }
        else if (activeDefender.defenderPrefab == null)
        {
            Debug.Log("Defender's prefab is not set! Defender name: " + activeDefender.defenderName); // Add the defender's name for better debugging.
        }
        else
        {
            // Instantiate using the prefab from the active defender
            GameObject spawnedDefender = Instantiate(activeDefender.defenderPrefab, position, Quaternion.identity);
            BulletShooter shooter = spawnedDefender.AddComponent<BulletShooter>();
            shooter.bulletPrefab = bulletPrefab; // You need to assign this, e.g., via the inspector or find it in the resources if it's a prefab
            shooter.bulletSpawnPoint = spawnedDefender.transform; // Assuming the bullet is spawned from the defender's location
            shooter.target = FindClosestEnemy(spawnedDefender.transform.position); // You need a function that finds the closest enemy
            //shooter.BulletHit(shooter.target.GetComponent<Collision>());
            //add.AddGeneBehaviors(spawnedDefender, activeDefender.geneTypes, true);
            // Remove the defender from the backpack after placing
            defenderBackpack.RemoveDefenderFromBackpack(activeDefender);
            hasPlacedDefender = true; // 更新状态
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
