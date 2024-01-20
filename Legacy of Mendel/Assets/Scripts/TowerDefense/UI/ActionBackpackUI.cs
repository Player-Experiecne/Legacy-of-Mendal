using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionBackpackUI : MonoBehaviour
{
    public GameObject defenderButtonPrefab;
    public ActionBackpack actionBackpack;

    private int selectedDefenderIndex = -1; // Initialize to -1 or another invalid index to denote no selection.
    private List<Button> defenderButtons = new List<Button>();

    private Color activeColor = new Color(0.792f, 0.792f, 0.792f);

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        // First, remove old buttons
        foreach (Button button in defenderButtons)
        {
            Destroy(button.gameObject);
        }
        defenderButtons.Clear();

        // Add new buttons based on the defenders in the backpack
        int index = 0;
        foreach (Defender defender in actionBackpack.defendersInBackpack)
        {
            GameObject btn = Instantiate(defenderButtonPrefab, transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = "Defender " + defender.defenderNumber.ToString();
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
        actionBackpack.activeDefender = defender;
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
