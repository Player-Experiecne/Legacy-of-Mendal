using System;
using System.Collections;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    public float waitTime = 1f;
    private void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
