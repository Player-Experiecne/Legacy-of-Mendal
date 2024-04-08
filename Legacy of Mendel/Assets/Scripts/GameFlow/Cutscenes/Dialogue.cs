using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Cutscene/Dialogue")]
public class Dialogue : ScriptableObject
{
    [TextArea(3, 10)]
    public string content;
}