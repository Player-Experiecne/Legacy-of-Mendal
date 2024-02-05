using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefenderLibrary", menuName = "Game Data/Defender Library")]
public class DefenderLibrary : ScriptableObject
{
    public List<Defender> allDefenders;
}