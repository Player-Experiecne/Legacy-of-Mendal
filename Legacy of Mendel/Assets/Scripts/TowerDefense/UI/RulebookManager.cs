using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulebookManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rulebook;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowRulebook()
    {
        rulebook.SetActive(true);
    }

    public void HideRulebook()
    {
        rulebook.SetActive(false);
    }
}
