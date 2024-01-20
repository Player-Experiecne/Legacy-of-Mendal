using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetInfo : MonoBehaviour
{
    [SerializeField]
    private int ID;  // To determine which dictionary is to be used.
    public AIType aiType;

    public enum AIType
    {
        Enemy,
        Defender
    }
    private List<Dictionary<string, float>> monsterData;

    private float attackRange;
    private float movementSpeed;
    private float attackPower;
    private float hp;
    private float attackSpeed;

    private EnemyController enemyController;
    private DefenderController defenderController;
    private NavMeshAgent navMeshAgent;
    private HP enemyHP;

    private void Awake()
    {
        //LoadEnemyInfo();

        if (aiType == AIType.Enemy)
        {
            enemyController = GetComponent<EnemyController>();
            enemyController.attackRange = attackRange;
            enemyController.attackPower = attackPower;
            enemyController.attackSpeed = attackSpeed;
        }
        else if (aiType == AIType.Defender)
        {
            defenderController = GetComponent<DefenderController>();
            defenderController.attackRange = attackRange;
            defenderController.attackPower = attackPower;
            defenderController.attackSpeed = attackSpeed;
        }
        enemyHP = GetComponent<HP>();
        enemyHP.maxHealth = hp;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = movementSpeed;
    }

    /*void LoadEnemyInfo()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        monsterData = gameManager.monsterData;
        int index = ID - 201;  // Get the dictionary index based on the ID

        if (index >= 0 && index < monsterData.Count)
        {
            Dictionary<string, float> currentData = monsterData[index];

            // Retrieve the data based on the keys
            currentData.TryGetValue("Attack Range", out attackRange);
            currentData.TryGetValue("Movement Speed", out movementSpeed);
            currentData.TryGetValue("Attack Power", out attackPower);
            currentData.TryGetValue("HP", out hp);
            currentData.TryGetValue("Attack Speed", out attackSpeed);
        }
        else
        {
            Debug.LogError("Invalid ID or Monster Data doesn't exist for this ID!");
        }
    }*/

    // You can further add methods to use these values or set them for the enemy attributes.
}
