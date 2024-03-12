using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public DefenderBackpack defenderBackpack;
    private Defender activeDefender = null;
    private AddBehaviorsToTarget add;

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
            //add.AddGeneBehaviors(spawnedDefender, activeDefender.geneTypes, true);
            // Remove the defender from the backpack after placing
            defenderBackpack.RemoveDefenderFromBackpack(activeDefender);
        }
    }

}
