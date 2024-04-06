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
    public LayerMask baseLayer;

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
                // Check if the hit object is on the Base layer
                if ((baseLayer.value & (1 << hit.collider.gameObject.layer)) == 0)
                {
                    // The hit object is not a base, proceed to place defender
                    PlaceDefender(hit.point);
                }
                else
                {
                    // Hit object is a base, log message or handle accordingly
                    Debug.Log("Cannot place defender on base!");
                }
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
