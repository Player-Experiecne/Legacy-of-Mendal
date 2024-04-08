using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Cutscene", menuName = "Cutscene/Cutscene")]
public class Cutscene : ScriptableObject
{
    public List<Dialogue> dialogues;
}
