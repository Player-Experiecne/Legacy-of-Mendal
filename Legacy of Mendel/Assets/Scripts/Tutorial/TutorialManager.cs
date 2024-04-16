using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeDelayBeforeStart = 5;

    public Button analyzebutton;
    public Button breedbutton;
    public Button handbookbutton;
    public Button defendersbutton;
    public Button nextdaybutton;
    public Button selecttissuebutton;
    public Button confirmbreedbutton;
    public Button choosedefenderbutton;
    public Button confirmclonebutton;
    public Button decrease;
    public Button increase;
    public Button confirmclonenum;
    public Button nextphasebutton;
    public Button button_1;


    public TMP_Dropdown geneDropdown; 
    public GameObject Image;
    public GameObject Image1;
    public GameObject Image2;
    
    public GameObject Image3;
    public GameObject Image4;
    public GameObject Image5;

    public GameObject Image6;

    public GameObject Image7;

    public GameObject Image8;
    public GameObject Image9;
    public GameObject Image10;
    public GameObject Image11;
    public GameObject Image12;
    public GameObject Image13;
    public GameObject Image14;
    public GameObject Image15;
    public GameObject Image16;
    public GameObject Image17;
    public GameObject Image18;
    public GameObject Image19;
    public GameObject Image20;
    public GameObject Image21;
    public GameObject Image22;
    public GameObject Image23;
    public GameObject Image24;
    public GameObject Image25;
    public GameObject Image26;
    public GameObject Image27;
    public GameObject Image28;
    public GameObject Image29;
    public GameObject Image30;
    public GameObject Image31;
    public GameObject Image32;
    public GameObject Image33;
    public GameObject ReminderToAnalyze;
    public GameObject ReminderToChooseTissue;
    public GameObject PreparationPanel;

    public GameObject CloneDefenderChooseReminder;
    public GameObject CloneDefenderChooseConfirmReminder;
    public GameObject numChoose1;
    public GameObject numChoose2;

    public GameObject CloneNumChoosePanel;
    public Image imageToHighlight;
    public Image imageToHighlight_2;
    public Image imageToHighlight1;
    public Image def;
    public Image newImage;

    public Image defImg_1;
    public Image defImg_2;
    public Image SmiteImage;

    public GameObject resultImage;
    public GameObject ClonePanel;


    public TextMeshProUGUI tissueText;
    public TextMeshProUGUI cultureMedium;
    public TextMeshProUGUI tissueCount;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI clonenum;
    public TextMeshProUGUI rumbleNum;
    public TextMeshProUGUI culNum;
    public NavMeshAgent agent;
    private float originalSpeed;
    public MonoBehaviour movementScript;
    public TutorialDefenderBackpack defenderBackpack;
    public GameObject[] tutorialSteps; // UI Elements for each step
    public TutorialBuildManager tbm;
    public int currentStep = 0;

    public int time=0;
    private Color originalColor;

    public Button myButton; // Assign this in the inspector
    public bool isButtonGreen = false;
    public void ChooseForNextBattle()
    {

    }
    void Start()
    {
        LockMovement(movementScript);
        StartCoroutine(PrepareTutorial());
        //StartCoroutine(BeginTutorial());
        geneDropdown.onValueChanged.AddListener(delegate {
            OnGeneTypeSelected(geneDropdown.value);
        });
        originalColor = imageToHighlight1.color;
        myButton.onClick.AddListener(() => {
            ChangeButtonColor();
            // Optionally perform the next step directly
            // if it should happen immediately after the color change.
             PerformNextStep();
        });
        DisableButton(breedbutton);
        DisableButton(handbookbutton);
        DisableButton(defendersbutton);
        DisableButton(nextdaybutton);
        DisableButton(confirmclonebutton);
        DisableButton(nextphasebutton);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (clonenum.text.Equals("1"))
        {
            DisableButton(increase);
            EnableButton(confirmclonenum);
            numChoose1.SetActive(false);
            numChoose2.SetActive(true);
            DisableButton(confirmclonebutton);
        }
    }
    public void addOne()
    {
        
    }
    public void OpenClonePanel()
    {
        ClonePanel.SetActive(true);
        

    }
    public Button button_2;
    public void OpenCloneNumChoose()
    {
        CloneDefenderChooseConfirmReminder.SetActive(false);
        button_2.interactable = false;
        CloneNumChoosePanel.SetActive(true);
        imageToHighlight_2.color = Color.white;
        CloneDefenderChooseReminder.SetActive(false);
        DisableButton(decrease);
        DisableButton(confirmclonenum);
        

    }

    public GameObject gam1;
    public GameObject gam2;
    public void ConfirmCloneRumble()
    {
        CloneNumChoosePanel.SetActive(false);
        rumbleNum.text = "Count: 2";
        culNum.text = "1";
        gam1.SetActive(true);
        StartCoroutine(BeginNextDay());

    }

    IEnumerator BeginNextDay()
    {
        yield return new WaitForSeconds(2);
        gam1.SetActive(false);
        gam2.SetActive(true);
        EnableButton(nextphasebutton);
    }

    public void EnterPreparation()
    {
        PreparationPanel.SetActive(true);

    }
    private void ChangeButtonColor()
    {
        myButton.GetComponent<Image>().color = Color.green;

        // Now that the button is green, set the flag to true
        isButtonGreen = true;

        // Optionally enable the next step directly here
        // if you have another UI element that needs to be activated.
        // EnableNextStepUIElement();
    }

    private void PerformNextStep()
    {

        ReminderToAnalyze.SetActive(true);
        ReminderToChooseTissue.SetActive(false);
    }
    public
    IEnumerator PrepareTutorial()
    {
        // 等待3秒开始教程
        //yield return new WaitForSeconds(3);
        PauseNavMeshAgent(agent);

        // 显示提示选择Defender的图片
        Image1.SetActive(true);

        yield return new WaitUntil(() => defenderBackpack.HasActiveDefender());

        Debug.Log(defenderBackpack.HasActiveDefender());

        // 选中Defender后隐藏提示图片
        Image1.SetActive(false);
        Image2.SetActive(true); // 假设Image2是提示玩家放置Defender的图片

        // 等待直到玩家放置了Defender
        yield return new WaitUntil(() => tbm.HasPlacedDefender());

        Debug.Log("Defender has been placed, continuing tutorial...");

        // 放置Defender后隐藏提示图片
        Image2.SetActive(false);
        ResumeNavMeshAgent(agent);
        yield return new WaitUntil(() => GameObject.FindWithTag("Enemy") == null);
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var bullet in bullets)
        {

            Destroy(bullet);
        }
        Image3.SetActive(true);

    }

    public void SetHighlightColor()
    {
        // 确保我们有一个有效的Image组件引用
        if (imageToHighlight != null)
        {
            // 设置Image的颜色为绿色
            imageToHighlight.color = Color.green;
            Image15.SetActive(false);
            Image16.SetActive(true);
            button_1.interactable = false;
        }
        else
        {
            Debug.LogError("No image component found to highlight.");
        }
    }

    public void SetHighlightColor_2()
    {
        // 确保我们有一个有效的Image组件引用
        if (imageToHighlight_2 != null)
        {
            // 设置Image的颜色为绿色
            imageToHighlight_2.color = Color.green;
            EnableButton(confirmclonebutton);
            CloneDefenderChooseReminder.SetActive(false);
            CloneDefenderChooseConfirmReminder.SetActive(true);
}
        else
        {
            Debug.LogError("No image component found to highlight.");
        }
    }
    public void SetHighlightColor_new()
    {
        // 确保我们有一个有效的Image组件引用
        if (imageToHighlight1 != null)
        {
            // 设置Image的颜色为绿色
            imageToHighlight1.color = Color.green;
           
            time++;
        }
        else
        {
            Debug.LogError("No image component found to highlight.");
        }
    }

    public void ConfirmDefender()
    {
        if (time == 1) {
            defImg_1.sprite = imageToHighlight1.sprite;
            imageToHighlight1.color = originalColor;
            countText.text = "Count: 1";
        }
        if (time == 2)
        {
            defImg_2.sprite = imageToHighlight1.sprite;
            imageToHighlight1.gameObject.SetActive(false);
            Image28.SetActive(false);
            Image29.SetActive(true);
        }
    }
    public void ConfirmSelection()
    {
        Image14.SetActive(false);
        Image13.SetActive(false);
        def.sprite = newImage.sprite;
        choosedefenderbutton.interactable = false;
        Image17.SetActive(true);
        EnableButton(confirmbreedbutton);
    }

    public void SummnorSkillEnter()
    {
        Image30.SetActive(true);
        

    }
    public void EnterTutorialPhase2()
    {
        Image.SetActive(false);
        Destroy(Image1);
        Destroy(Image2);
        Destroy(Image3);
        Image4.SetActive(true);

        Debug.Log("Enter Phase 2");
    }

    public void CloseHint()
    {

        Image5.SetActive(false);

       
    }
    public void CloseHint2()
    {

        Image27.SetActive(false);
        Image28.SetActive(true);


    }

    public void CloseHint3()
    {

        Image31.SetActive(false);
        Image32.SetActive(true);

       /* if (SmiteImage.color == Color.yellow)
        {
            Image32.SetActive(false);
            Image33.SetActive(true);
        }*/
    }

    public void CheckSmite()
    {
        Image32.SetActive(false);
        Image33.SetActive(true);
    }
    public void OpenTissueSelection()
    {

        Image6.SetActive(true);
        Image7.SetActive(false);
        DisableButton(analyzebutton);


    }
    public void DisableButton(Button myButton)
    {
        myButton.interactable = false;
    }

    // Call this function to enable the button
    public void EnableButton(Button myButton)
    {
        myButton.interactable = true;
    }
    public void Analyze()
    {
        Debug.Log("----------");
        Image17.SetActive(false);
        Image6.SetActive(false);
        Image7.SetActive(false);
        tissueText.text = "You have received gene type AA";
        cultureMedium.text = "23";
        tissueCount.text = "3";
        StartCoroutine(BeginPhaseThree());


    }
    IEnumerator BeginPhaseThree()
    {
        Debug.Log("test"); 
        DisableButton(selecttissuebutton);
        yield return new WaitForSeconds(1);
        Image8.SetActive(true);
        yield return new WaitForSeconds(3);
        Image8.SetActive(false);
        Image9.SetActive(true);
        EnableButton(breedbutton);
    }

    public void EnterBreed()
    {
        Image9.SetActive(false);
        Image10.SetActive(true);
        Image23.SetActive(false);
        Image11.SetActive(true);
        DisableButton(choosedefenderbutton);
        DisableButton(confirmbreedbutton);
        DisableButton(breedbutton);

    }

    public void EnterBreedPhase()
    {
        
        Image11.SetActive(false);
        
        StartCoroutine(BeginPhaseAfterChooseGene());


    }

    public void ChooseCombatUnits()
    {

        Image13.SetActive(false);
        Image14.SetActive(true);

        Image15.SetActive(true);

    }
    IEnumerator ChooseExactDefenders()
    {
        yield return new WaitForSeconds(2);

       

    }

    IEnumerator WaitForPlayerToConfirm()
    {
        yield return new WaitForSeconds(2);

        Image17.SetActive(true);

    }
    IEnumerator BeginPhaseAfterChooseGene()
    {
        geneDropdown.interactable = false;

        //yield return new WaitForSeconds(2);
        Image12.SetActive(true);
        yield return new WaitForSeconds(2);
        Image12.SetActive(false);
        EnableButton(choosedefenderbutton);
        Image13.SetActive(true);

    }
  

    public void OnGeneTypeSelected(int index)
    {
        // TMP_Dropdown的options是一个列表，其中包含了所有选项
        string selectedOption = geneDropdown.options[index].text;

        // 检查选择的是不是"AA"
        if (selectedOption == "AA")
        {
            // 如果是"AA"，等待两秒后执行下一步
            EnterBreedPhase();
        }
        else
        {
            // 如果不是"AA"，给予用户反馈
            Debug.Log("Please select the 'AA' gene type to continue.");
        }
    }
    void HighlightUIElement(GameObject element)
    {
        // Implement the logic to highlight the UI element
        // This could be a simple animation, a change of color, a frame around the element, etc.
    }

    bool PlayerMadeCorrectInput(GameObject step)
    {
        // Implement the logic to check if player made the correct interaction
        // This might involve subscribing to button events or other input detection methods.
        return true; // Replace this with actual check
    }

    // Call this from the UI button's onClick event
    public void PlayerClickedButton()
    {
        if (currentStep < tutorialSteps.Length)
        {
            currentStep++;
        }
    }

    public void ConfirmBreed()
    {
        def.sprite = null;
        geneDropdown.value = 0;
        geneDropdown.RefreshShownValue();
        resultImage.SetActive(true);
        Debug.Log("Confirm breed");
        resultText.text = "You have got defender : Rumble";
        Image17.SetActive(false);
        cultureMedium.text = "11";
        StartCoroutine(OpeartionsAfterConfiorm());

    }

    IEnumerator OpeartionsAfterConfiorm()
    {
        DisableButton(confirmbreedbutton);
        DisableButton(breedbutton);
        DisableButton(choosedefenderbutton);
        
        yield return new WaitForSeconds(1);
        Image19.SetActive(true);
        yield return new WaitForSeconds(2);
        Image19.SetActive(false);
        Image18.SetActive(true);
        EnableButton(handbookbutton);

    }
    public void PauseNavMeshAgent(NavMeshAgent agent)
    {
        originalSpeed = agent.speed;
        agent.speed = 0;
        Debug.Log("NavMeshAgent speed set to 0");
    }
    public void OpenDNAHandbook()
    {
        Image20.SetActive(true);
        Image18.SetActive(false);
        StartCoroutine(OpeartionsAfterHandbook());
    }

    IEnumerator OpeartionsAfterHandbook()
    {

        yield return new WaitForSeconds(3);
       
        Image21.SetActive(true);
        yield return new WaitForSeconds(3);
        Image21.SetActive(false);
        Image22.SetActive(true);
        DisableButton(handbookbutton);
        EnableButton(defendersbutton);


    }

    public void OpenCombatUnits()
    {
        Image23.SetActive(false);
        Image10.SetActive(false);
        Image20.SetActive(false);
        Image24.SetActive(true);
        StartCoroutine(OpeartionsAfterOpenCombatUnits());
    }

    IEnumerator OpeartionsAfterOpenCombatUnits()
    {
        DisableButton(defendersbutton);
        yield return new WaitForSeconds(2);

        Image25.SetActive(true);
        yield return new WaitForSeconds(2);
        Image25.SetActive(false);
        Image26.SetActive(true);
        EnableButton(nextdaybutton);

    }
    public void ResumeNavMeshAgent(NavMeshAgent agent)
    {
        agent.speed = originalSpeed;
        Debug.Log("NavMeshAgent speed restored");
    }
    public void LockMovement(MonoBehaviour movementScript)
    {
        movementScript.enabled = false;
    }

    public void UnlockMovement(MonoBehaviour movementScript)
    {
        movementScript.enabled = true;
    }

}
