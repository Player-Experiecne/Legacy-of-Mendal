using UnityEngine;

public abstract class SummonerSkill : ScriptableObject
{
    [Header("Description")]
    public string skillName;
    public Sprite skillIcon;

    [Header("Cooldown")]
    public float cooldownTime;
    public abstract void Activate();

    public Vector3 LocatePlayerGroundPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController controller = player.GetComponent<PlayerController>();
        return controller.groundPosition.position;
    }
}
