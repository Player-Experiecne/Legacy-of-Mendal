using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialDefenderBackpackUI : MonoBehaviour
{
    public GameObject defenderButtonPrefab;
    public TutorialDefenderBackpack defendersInTutorial;
    public Sprite defenderImage;

    private int selectedDefenderIndex = -1; // Initialize to -1 or another invalid index to denote no selection.
    private List<Button> defenderButtons = new List<Button>();

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        // First, remove old buttons
        foreach (Button button in defenderButtons)
        {
            // 在尝试销毁前先检查对象是否为null
            if (button != null)
                Destroy(button.gameObject);
        }
        defenderButtons.Clear();

        // Add new buttons based on the defenders in the backpack
        int index = 0;
        foreach (Defender defender in defendersInTutorial.defendersInTutorial)
        {
            GameObject btn = Instantiate(defenderButtonPrefab, transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = defender.defenderName;
            Image buttonImage = btn.GetComponentsInChildren<Image>()[1];
            buttonImage.sprite = defenderImage;
            Button buttonComponent = btn.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnDefenderSelected(defender, index));

            defenderButtons.Add(buttonComponent);
            index++;
        }

        UpdateActiveDefenderHighlight();
    }

    private void OnDefenderSelected(Defender defender, int index)
    {
        selectedDefenderIndex = index;
        defendersInTutorial.activeDefender = defender;
        UpdateActiveDefenderHighlight();
    }


    private void UpdateActiveDefenderHighlight()
    {
        Color activeColor = new Color(0.792f, 0.792f, 0.792f); // Corresponds to CACACA
        Color defaultNormalColor = Color.white; // Assuming the default normal color is white

        for (int i = 0; i < defenderButtons.Count; i++)
        {
            Button btn = defenderButtons[i];
            ColorBlock cb = btn.colors;

            if (i == selectedDefenderIndex)
            {
                cb.normalColor = activeColor;
            }
            else
            {
                cb.normalColor = defaultNormalColor; // Resetting to default normal color
            }

            btn.colors = cb;
        }
    }
}
