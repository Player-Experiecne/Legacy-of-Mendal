using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightOrbGroup : MonoBehaviour
{
    public float instantDamage;

    public float moveDistance = 15f;

    public HP selfHP;
    public GameObject target;

    private float destroyAfterSeconds = 1.25f;
    void Start()
    {
        StartCoroutine(SelfDestruction());
        foreach (var component in GetComponentsInChildren<LightOrb>())
        {
            component.moveDistance = moveDistance;
            component.PrepareAttack(target.transform.position);
        }
    }

    private IEnumerator SelfDestruction()
    {
        GameObject lightOrb1 = gameObject.transform.GetChild(0).gameObject;
        GameObject lightOrb2 = gameObject.transform.GetChild(1).gameObject;
        yield return new WaitForSeconds(destroyAfterSeconds);
        if(lightOrb1 != null) { Destroy(lightOrb1); }
        if(lightOrb2 != null) { Destroy(lightOrb2); }
        if(gameObject != null) { Destroy(gameObject); }
    }
}
