using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject analyzePanel;
    public GameObject breedPanel;
    public GameObject monsterHandbookPanel;
    public GameObject dnaHandbookPanel;
    public GameObject combatUnitsPanel;

    public Button analyzeButton;
    public Button breedButton;
    public Button monsterHandbookButton;
    public Button dnaHandbookButton;
    public Button combatUnitsButton;

    private void Start()
    {
        
        analyzeButton.onClick.AddListener(() => ActivatePanel(analyzePanel));
        breedButton.onClick.AddListener(() => ActivatePanel(breedPanel));
        monsterHandbookButton.onClick.AddListener(() => ActivatePanel(monsterHandbookPanel));
        dnaHandbookButton.onClick.AddListener(() => ActivatePanel(dnaHandbookPanel));
        combatUnitsButton.onClick.AddListener(() => ActivatePanel(combatUnitsPanel));

        
        ActivatePanel(analyzePanel);
    }

    void ActivatePanel(GameObject panel)
    {
       
        analyzePanel.SetActive(false);
        breedPanel.SetActive(false);
        monsterHandbookPanel.SetActive(false);
        dnaHandbookPanel.SetActive(false);
        combatUnitsPanel.SetActive(false);

       
        panel.SetActive(true);
    }
}
