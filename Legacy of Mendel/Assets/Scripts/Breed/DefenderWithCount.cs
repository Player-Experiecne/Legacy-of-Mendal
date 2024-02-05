using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class DefenderWithCount
{
    public Defender defender;
    public int count;

    public DefenderWithCount(Defender defender, int count)
    {
        this.defender = defender;
        this.count = count;
    }
}


