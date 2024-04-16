using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsHint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.currentLevelIndex != 0)
        {
            Destroy(gameObject);
        }
    }
}

