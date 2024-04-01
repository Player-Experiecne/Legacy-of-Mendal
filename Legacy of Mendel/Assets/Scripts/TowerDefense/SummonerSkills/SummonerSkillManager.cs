using UnityEngine;

public class SummonerSkillManager : MonoBehaviour
{
    public static SummonerSkillManager Instance;
    public SummonerSkill skill; // Variable to hold the chosen skill
    public GameObject summonerSkillUI;

    public float cooldownTimer;
    public bool isCooldownActive;

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
        DontDestroyOnLoad(gameObject); // Keep this instance alive across scenes
    }

    void Start()
    {
        ResetCooldown(); // Initialize cooldown
    }

    void Update()
    {
        if (isCooldownActive && cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime; // Decrease cooldown timer
            if (cooldownTimer <= 0)
            {
                isCooldownActive = false; // Reset flag when cooldown ends
            }
        }
        // Input handling for activating skill
        if (InputManager.Instance.GetKeyDown("SummonerSkill") && !GameManager.Instance.isTitleScreen)
        {
            Debug.Log("Triggering the summoner skill.");
            ActivateSkill(); // Trigger the skill
        }
    }

    public void ActivateSkill()
    {
        if (!isCooldownActive && skill != null)
        {
            skill.Activate();
            cooldownTimer = skill.cooldownTime;
            isCooldownActive = true;
        }
        else if (isCooldownActive)
        {
            Debug.Log($"Still cooling down, {cooldownTimer} seconds left.");
        }
    }

    public void ResetCooldown()
    {
        cooldownTimer = 0;
        isCooldownActive = false; // Ensure cooldown is not active at reset
    }

    public void SetSkill(SummonerSkill newSkill)
    {
        skill = newSkill;
        ResetCooldown(); // Reset cooldown with new skill selection
    }
}
