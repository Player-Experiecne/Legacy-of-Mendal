using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonerSkillsUI : MonoBehaviour
{
    public Image skillIcon;
    public TextMeshProUGUI skillName;
    public Image cooldownOverlay;
    public TextMeshProUGUI cooldownText;
    private SummonerSkillManager skillManager; // Reference to your skill manager

    void Start()
    {
        skillManager = SummonerSkillManager.Instance;
        UpdateSkillUI(); // Initial UI setup
    }

    void UpdateSkillUI()
    {
        if (skillManager.skill != null)
        {
            // Assuming the skill has a public sprite and name for UI purposes
            skillIcon.sprite = skillManager.skill.skillIcon;
            skillName.text = skillManager.skill.skillName;

            // Initial reset for cooldown display
            cooldownOverlay.fillAmount = 0;
            cooldownText.text = "";
        }
    }

    void Update()
    {
        if (skillManager.isCooldownActive)
        {
            // Assuming cooldownTimer is initially set to the skill's cooldown time
            float cooldownLeft = skillManager.cooldownTimer;
            float totalCooldown = skillManager.skill.cooldownTime;
            cooldownOverlay.fillAmount = cooldownLeft / totalCooldown;
            cooldownText.text = $"Cooldown left: {cooldownLeft:F1}s"; // Show one decimal place
        }
        else
        {
            cooldownOverlay.fillAmount = 0;
            cooldownText.text = "";
        }
    }
}
