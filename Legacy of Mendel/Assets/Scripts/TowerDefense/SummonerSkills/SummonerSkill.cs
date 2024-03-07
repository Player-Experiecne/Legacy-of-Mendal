using UnityEngine;

public abstract class SummonerSkill : ScriptableObject
{
    [Header("Description")]
    public string skillName;
    public Sprite skillIcon;

    [Header("Cooldown")]
    public float cooldownTime;
    public abstract void Activate();

    // Additional properties and methods as needed
}
