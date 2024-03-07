using UnityEngine;

public class SummonerSkillManager : MonoBehaviour
{
    public static SummonerSkillManager Instance;
    public SummonerSkill[] skills = new SummonerSkill[2]; // Array to hold the two chosen skills

    public float[] cooldownTimers;
    public bool[] isCooldownActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        cooldownTimers = new float[skills.Length];
        isCooldownActive = new bool[skills.Length];
        ResetCooldowns(); // Initialize cooldowns
    }

    void Update()
    {
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (isCooldownActive[i] && cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime; // Decrease cooldown timer
                if (cooldownTimers[i] <= 0)
                {
                    isCooldownActive[i] = false; // Reset flag when cooldown ends
                }
            }
        }
        // Input handling for activating skills
        if (InputManager.Instance.GetKeyDown("SummonerSkills1"))
        {
            Debug.Log("Triggering the first summoner skill.");
            ActivateSkill(0); // Trigger the first skill
        }
        if (InputManager.Instance.GetKeyDown("SummonerSkills2"))
        {
            Debug.Log("Triggering the second summoner skill.");
            ActivateSkill(1); // Trigger the second skill
        }
    }

    public void ActivateSkill(int skillIndex)
    {
        if (skillIndex >= 0 && skillIndex < skills.Length)
        {
            if (!isCooldownActive[skillIndex])
            {
                skills[skillIndex].Activate();
                cooldownTimers[skillIndex] = skills[skillIndex].cooldownTime;
                isCooldownActive[skillIndex] = true;
            }
            else
            {
                Debug.Log($"Still cooling down, {cooldownTimers[skillIndex]} seconds left.");
            }

        }
    }

    public void ResetCooldowns()
    {
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            cooldownTimers[i] = 0;
            isCooldownActive[i] = false; // Ensure cooldowns are not active at reset
        }
    }

    public void SetSkills(SummonerSkill firstSkill, SummonerSkill secondSkill)
    {
        skills[0] = firstSkill;
        skills[1] = secondSkill;
        ResetCooldowns(); // Reset cooldowns with new skill selection
    }
}
