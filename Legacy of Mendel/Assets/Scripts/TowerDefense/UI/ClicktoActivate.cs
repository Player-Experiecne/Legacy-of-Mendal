using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicktoActivate : MonoBehaviour
{
    public GameObject target;

    public void ActivateTarget()
    {
        target.SetActive(true);
    }
    public void DeactivateTarget()
    {
        target.SetActive(false);
    }
}
