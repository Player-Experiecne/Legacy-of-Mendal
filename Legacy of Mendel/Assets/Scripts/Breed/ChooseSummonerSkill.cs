using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSummonerSkill : MonoBehaviour
{
    public SummonerSkill[] availableSkills;
    public Image[] skillButtons; // 用于显示技能图标的按钮
    private int selectedSkillIndex = -1; // 当前选中的技能索引，-1 表示没有选中

    void Start()
    {
        InitializeSkillButtons();
        UpdateSkillButtonSelection();
    }

    private void InitializeSkillButtons()
    {
        for (int i = 0; i < skillButtons.Length && i < availableSkills.Length; i++)
        {
            int index = i;
            Button button = skillButtons[i].GetComponent<Button>();
            if (button == null)
            {
                button = skillButtons[i].gameObject.AddComponent<Button>();
            }
            button.onClick.AddListener(() => ChooseSkill(index));

            // 设置技能图标
            skillButtons[i].sprite = availableSkills[i].skillIcon;
            // 设置默认状态，没有技能被选中
            skillButtons[i].color = Color.white;  // 默认颜色
        }
    }

    private void UpdateSkillButtonSelection()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            // 如果技能已选中，高亮显示；否则显示为正常状态
            skillButtons[i].color = i == selectedSkillIndex ? Color.yellow : Color.white;
        }
    }

    public void ChooseSkill(int skillIndex)
    {
        // 如果点击的是已经选中的技能，则取消选中
        if (selectedSkillIndex == skillIndex)
        {
            selectedSkillIndex = -1;
        }
        else
        {
            selectedSkillIndex = skillIndex;
        }

        UpdateSkillButtonSelection();
    }

    public void ConfirmSelection()
    {
        // 确认选择，如果有选中的技能
        if (selectedSkillIndex >= 0 && selectedSkillIndex < availableSkills.Length)
        {
            SummonerSkillManager.Instance.SetSkill(availableSkills[selectedSkillIndex]); // 仅设置一个技能
            SummonerSkillManager.Instance.summonerSkillUI.GetComponent<SummonerSkillsUI>().UpdateSkillUI();
            Debug.Log("Confirmed Skill: " + availableSkills[selectedSkillIndex].skillName);

            // 可以在这里添加任何其他确认逻辑，例如关闭选择菜单等
        }
        else
        {
            Debug.Log("No Skill Selected");
        }

        // 可以选择在确认后重置选中状态
        selectedSkillIndex = -1;
        UpdateSkillButtonSelection();
        GameEvents.TriggerBreedingComplete();
    }
}
