using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public ActionBackpack actionBackpack;
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
        activeDefender = actionBackpack.activeDefender;
        // Check if there's an active defender to place
        if (activeDefender == null)
        {
            Debug.Log("No defender selected");
        }
        else if (activeDefender.defenderPrefab == null)
        {
            Debug.Log("Defender's prefab is not set!");
        }
        else
        {
            // Instantiate using the prefab from the active defender
            GameObject spawnedDefender = Instantiate(activeDefender.defenderPrefab, position, Quaternion.identity);
            add.AddGeneABehaviors(spawnedDefender, activeDefender.geneTypeA, true);

            // Remove the defender from the backpack after placing
            actionBackpack.RemoveDefenderFromBackpack(activeDefender);
        }
    }

}
