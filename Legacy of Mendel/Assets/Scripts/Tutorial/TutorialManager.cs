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


    public Image imageToHighlight;
    public Image imageToHighlight1;
    public Image def;
    public Image newImage;

    public Image defImg_1;
    public Image defImg_2;

    public GameObject resultImage;


    public TextMeshProUGUI tissueText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI countText;
    public NavMeshAgent agent;
    private float originalSpeed;
    public MonoBehaviour movementScript;
    public TutorialDefenderBackpack defenderBackpack;
    public GameObject[] tutorialSteps; // UI Elements for each step
    public TutorialBuildManager tbm;
    public int currentStep = 0;

    public int time=0;
    private Color originalColor;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public
    IEnumerator PrepareTutorial()
    {
        // 等待3秒开始教程
        yield return new WaitForSeconds(3);
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
            imageToHighlight1.color = originalColor;
        }
    }
    public void ConfirmSelection()
    {
        Image14.SetActive(false);
        Image13.SetActive(false);
        def.sprite = newImage.sprite;
        StartCoroutine(WaitForPlayerToConfirm());

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


    }
    public void OpenTissueSelection()
    {

        Image6.SetActive(true);


    }

    public void Analyze()
    {
        Debug.Log("----------");
        Image6.SetActive(false);
        Image7.SetActive(false);
        tissueText.text = "You have received gene type AA";
        StartCoroutine(BeginPhaseThree());


    }

    public void EnterBreed()
    {
        Image9.SetActive(false);
        Image10.SetActive(true);

    }

    public void EnterBreedPhase()
    {
        
        Image11.SetActive(false);
        StartCoroutine(BeginPhaseAfterChooseGene());


    }

    public void ChooseCombatUnits()
    {

        Image14.SetActive(true);

        StartCoroutine(ChooseExactDefenders());

    }
    IEnumerator ChooseExactDefenders()
    {
        yield return new WaitForSeconds(2);

        Image15.SetActive(true);

    }

    IEnumerator WaitForPlayerToConfirm()
    {
        yield return new WaitForSeconds(2);

        Image17.SetActive(true);

    }
    IEnumerator BeginPhaseAfterChooseGene()
    {

        yield return new WaitForSeconds(2);
        Image12.SetActive(true);
        yield return new WaitForSeconds(2);
        Image12.SetActive(false);
        Image13.SetActive(true);

    }
    IEnumerator BeginPhaseThree()
    {
        
        yield return new WaitForSeconds(1);
        Image8.SetActive(true);
        yield return new WaitForSeconds(3);
        Image8.SetActive(false);
        Image9.SetActive(true);

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
        resultText.text = "You have got defender : Rumble";
        Image17.SetActive(false);
        StartCoroutine(OpeartionsAfterConfiorm());

    }

    IEnumerator OpeartionsAfterConfiorm()
    {

        yield return new WaitForSeconds(1);
        Image19.SetActive(true);
        yield return new WaitForSeconds(2);
        Image19.SetActive(false);
        Image18.SetActive(true);

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

        yield return new WaitForSeconds(2);

        Image25.SetActive(true);
        yield return new WaitForSeconds(2);
        Image25.SetActive(false);
        Image26.SetActive(true);


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
