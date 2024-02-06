using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DefenderLibrary", menuName = "Game Data/Defender Library")]
public class DefenderLibrary : ScriptableObject
{
    public List<Defender> defenders;
}