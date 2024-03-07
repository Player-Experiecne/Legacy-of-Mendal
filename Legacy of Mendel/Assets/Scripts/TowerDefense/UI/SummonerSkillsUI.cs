using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonerSkillsUI : MonoBehaviour
{
    public Image[] skillIcons;
    public TextMeshProUGUI[] skillNames;
    public Image[] cooldownOverlays;
    public TextMeshProUGUI[] cooldownTexts;
    private SummonerSkillManager skillManager; // Reference to your skill manager

    void Start()
    {
        skillManager = SummonerSkillManager.Instance;
        UpdateSkillUI(); // Initial UI setup
    }

    void UpdateSkillUI()
    {
        for (int i = 0; i < skillManager.skills.Length; i++)
        {
            var skill = skillManager.skills[i];
            // Assuming each skill has a public sprite and name for UI purposes
            skillIcons[i].sprite = skill.skillIcon;
            //cooldownOverlays[i].sprite = skill.skillIcon;
            skillNames[i].text = skill.skillName;

            // Initial reset for cooldown display
            cooldownOverlays[i].fillAmount = 0;
            cooldownTexts[i].text = "";
        }
    }

    void Update()
    {
        for (int i = 0; i < skillManager.cooldownTimers.Length; i++)
        {
            if (skillManager.isCooldownActive[i])
            {
                // Assuming cooldownTimers are initially set to the skill's cooldown time
                float cooldownLeft = skillManager.cooldownTimers[i];
                float totalCooldown = skillManager.skills[i].cooldownTime;
                cooldownOverlays[i].fillAmount = cooldownLeft / totalCooldown;
                cooldownTexts[i].text = $"Cooldown left: {cooldownLeft:F1}s"; // Show one decimal place
            }
            else
            {
                cooldownOverlays[i].fillAmount = 0;
                cooldownTexts[i].text = "";
            }
        }
    }

}
