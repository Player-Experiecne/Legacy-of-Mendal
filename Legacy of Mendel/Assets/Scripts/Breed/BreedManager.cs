using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class BreedManager : MonoBehaviour
{
    // UI 引用
    public static BreedManager Instance;

    public GeneDatabase geneDatabase;

    public GeneDropdownPopulator geneDropdownPopulator;

    public DefenderLibrary defenderLibrary;
    public Image breedResultImage; 

    public List<GameObject> hiddenUIs;
    public List<GameObject> tissueSlots;
    public List<GameObject> actionsAfterBreed;
    public GameObject clickBreed;
    public TextMeshProUGUI tissueCountText; 
    public TextMeshProUGUI cultureMediumCountText;

    public TextMeshProUGUI breedResult;
    public Button nextPageButton; 
    public Button previousPageButton; 
    public Sprite tissueSprite;
    public GameObject analyzeUI;
    public TextMeshProUGUI geneTypeInfoText;

    public Image defenderDisplayImage;
    public GameObject chooseBreedDefendersPanel;
    public PlayerDefenderInventory playerDefenderInventory;
    public Defender selectedDefender = null;
    public GeneTypeEntry selectedGeneType = null;

    private GameObject selectedSlot = null;

    public GeneLibrary geneLibrary;


    public GeneLibraryDisplay geneLibraryDisplay;

    // 分页数据
    private List<GeneTypeEntry> tissues = new List<GeneTypeEntry>();
    private int itemsPerPage = 10; 
    private int currentPage = 0; 
    private int totalPage;

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
        // 初始化按钮点击事件

        StartBreedingPhase();
        
        foreach (Defender defer in DefenderBackpack.Instance.defendersInBackpack){
            playerDefenderInventory.AddDefenderToInventory(defer);
        }
        DefenderBackpack.Instance.ClearBackpack();
        nextPageButton.onClick.AddListener(NextPage);
        previousPageButton.onClick.AddListener(PreviousPage);

        // 默认隐藏翻页按钮
        nextPageButton.gameObject.SetActive(false);
        previousPageButton.gameObject.SetActive(false);
    }
    private void Update()
    {


        UpdateCountsDisplay();



        }
    public void StartBreedingPhase()
    {
        // 获取 LootBackpack 实例中的数据
     /*   foreach (GameObject uiElement in hiddenUIs)
        {
            uiElement.SetActive(false); // 隐藏所有 UI 元素
        }
*/
        // 清除现有的 tissues 列表
        tissues.Clear();

        // 遍历 LootBackpack 中所有的基因型计数
        foreach (var geneTypeCount in LootBackpack.Instance.geneTypeCountsList)
        {
            // 对于每种基因型，根据其计数添加到 tissues 列表中
            for (int i = 0; i < geneTypeCount.count; i++)
            {
                tissues.Add(new GeneTypeEntry { geneName = geneTypeCount.geneType.geneName, geneType = geneTypeCount.geneType.geneType });
            }
        }

        Debug.Log(tissues.Count);

        Debug.Log("-------------");
        // 更新页码和分页显示
        totalPage = Mathf.CeilToInt((float)tissues.Count / itemsPerPage);
        currentPage = 0; // 重置为第一页

        // 刷新界面显示
        analyzeUI.SetActive(false); // 如果有分析 UI，则将其设置为不可见
        UpdateCountsDisplay(); // 更新显示以反映新的 tissues 列表




    }
    public void SetSelectedDefender(Defender defender)
    {
        selectedDefender = defender;
    }

    // 调用这个方法来设置选中的基因型
    public void SetSelectedGeneType(GeneTypeEntry geneType)
    {
        selectedGeneType = geneType;
    }

    public void PopUpChooseDefenderUI()
    {
        chooseBreedDefendersPanel.SetActive(true);

    }
    public void ClickAnalyzeChoose()
    {
        PopulateTissueList();
    }
    void UpdateCountsDisplay()
    {
        tissueCountText.text = $"Tissue Count: {tissues.Count}";
        cultureMediumCountText.text = $"Culture Medium Count: {LootBackpack.Instance.lootCultureMedium}";
    }

    void PopulateTissueList()
    {

        analyzeUI.SetActive(true);
        int start = currentPage * itemsPerPage;
        int end = Mathf.Min(start + itemsPerPage, tissues.Count);

        for (int i = 0; i < tissueSlots.Count; i++)
        {
            GameObject slot = tissueSlots[i];
            if (start + i < end)
            {
                slot.SetActive(true);
                Image imageComponent = slot.GetComponent<Image>();
                imageComponent.sprite = tissueSprite; // 设置相同的Sprite

                // 重置颜色为默认值，以防之前选中
                imageComponent.color = Color.white;

                Button button = slot.GetComponent<Button>();
                if (button == null)
                {
                    button = slot.AddComponent<Button>();
                }

                // 移除之前的监听器，防止重复添加
                button.onClick.RemoveAllListeners();

                
                int index = i; 
                button.onClick.AddListener(() => ToggleHighlight(slot, index));
            }
            else
            {
                slot.SetActive(false);
            }
        }


        nextPageButton.gameObject.SetActive(totalPage > 1);
        previousPageButton.gameObject.SetActive(totalPage > 1);

     
        nextPageButton.interactable = currentPage < totalPage - 1;
        previousPageButton.interactable = currentPage > 0;

        Debug.Log("22222222");
    }

    public void NextPage()
    {
        if (currentPage < totalPage - 1)
        {
            currentPage++;
            PopulateTissueList();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            PopulateTissueList();
        }
    }

    void ToggleHighlight(GameObject slot, int index)
    {
        if (selectedSlot == slot) // 如果点击的是当前选中的槽位，则取消选中
        {
            slot.GetComponent<Image>().color = Color.white;
            selectedSlot = null; // 重置当前选中的槽位
        }
        else
        {
            if (selectedSlot != null) // 如果之前有选中的槽位，则取消其选中状态
            {
                selectedSlot.GetComponent<Image>().color = Color.white;
            }

            // 设置新选中的槽位为高亮颜色，并更新引用
            slot.GetComponent<Image>().color = Color.green;
            selectedSlot = slot;
        }
    }

    public void OnBreedButtonClick()
    {
        Debug.Log($"At Button Click: Selected Defender is {(selectedDefender == null ? "null" : selectedDefender.defenderName)}, Selected Gene Type is {(selectedGeneType == null ? "null" : selectedGeneType.geneName.ToString())}");

        if (selectedDefender != null && selectedGeneType != null )
        {
            Debug.Log(selectedDefender);
            if(LootBackpack.Instance.lootCultureMedium < 10)
            {
                breedResult.text = "You dont have enough CultureMedium";

            }
            else
            {
                if (selectedDefender.geneTypes.Count == 3 && selectedGeneType.geneName != GeneInfo.geneTypesName.Null && selectedGeneType.geneType != GeneInfo.geneTypes.Null)
                {
                    Defender newDefender = PerformFusion(selectedDefender, selectedGeneType);
                    LootBackpack.Instance.lootCultureMedium = LootBackpack.Instance.lootCultureMedium - 10;
                    if (newDefender != null)
                    {
                        playerDefenderInventory.AddDefenderToInventory(newDefender);
                        playerDefenderInventory.RemoveDefenderFromInventory(selectedDefender);
                        //clickBreed.SetActive(false);
                        UpdateDefenderDetailsFromLibrary(newDefender);
                        breedResultImage.sprite = newDefender.defenderImage;
                        string geneTypesStr = "You have got a defender" + newDefender.defenderName + " with gene type ";
                        foreach (GeneTypeEntry gene in newDefender.geneTypes)
                        {
                            if (gene.geneName.Equals(GeneInfo.geneTypesName.A))
                            {
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Dom))
                                {
                                    geneTypesStr = geneTypesStr + "AA";
                                }
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Het))
                                {
                                    geneTypesStr = geneTypesStr + "Aa";
                                }
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Rec))
                                {
                                    geneTypesStr = geneTypesStr + "aa";
                                }
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Null))
                                {

                                }


                            }
                            if (gene.geneName.Equals(GeneInfo.geneTypesName.B))
                            {
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Dom))
                                {
                                    geneTypesStr = geneTypesStr + "BB";
                                }
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Het))
                                {
                                    geneTypesStr = geneTypesStr + "Bb";
                                }
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Rec))
                                {
                                    geneTypesStr = geneTypesStr + "bb";
                                }
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Null))
                                {

                                }

                            }
                            if (gene.geneName.Equals(GeneInfo.geneTypesName.C))
                            {
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Dom))
                                {
                                    geneTypesStr = geneTypesStr + "CC";
                                }
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Het))
                                {
                                    geneTypesStr = geneTypesStr + "Cc";
                                }
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Rec))
                                {
                                    geneTypesStr = geneTypesStr + "cc";
                                }
                                if (gene.geneType.Equals(GeneInfo.geneTypes.Null))
                                {

                                }


                            }

                        }
                        breedResult.text = geneTypesStr + "!";
                        Debug.Log(newDefender.geneTypes.Count);
                        /*foreach (GameObject a in actionsAfterBreed)
                        {
                            a.SetActive(true);
                        }*/
                        // 重置培育界面
                        ResetBreedingUI();

                    }
                }
                else
                {
                    breedResult.text = "Please choose a defender and a gene type";
                }
            }
       
            // 执行融合逻辑
            
            

        }
        else
        {
            // 如果未选择Defender或基因型，则弹出提示
            Debug.LogWarning("Defender or GeneType not selected!");
            // 可以在这里添加UI反馈，例如显示错误消息
        }

    }


    public Defender UpdateDefenderDetailsFromLibrary(Defender fusedDefender)
    {
        foreach (var libDefender in defenderLibrary.defenders)  // 假设allDefenders是你库中所有Defenders的列表
        {
            // 检查基因型是否相等
            if (AreGeneTypesEqualTwoDefenders(fusedDefender, libDefender))
            {
                // 更新fusedDefender的详情
                fusedDefender.defenderName = libDefender.defenderName;
                fusedDefender.defenderPrefab = libDefender.defenderPrefab;
                fusedDefender.defenderImage = libDefender.defenderImage;
                return fusedDefender;  // 返回更新后的Defender
            }
        }

        // 如果没有找到匹配的Defender
        Debug.LogError("No matching defender found in the library for the fused gene types.");
        return null; 
    }

    public bool AreGeneTypesEqualTwoDefenders(Defender defender1, Defender defender2)
    {
        // 首先检查两个防御者的基因型列表长度是否相同
        if (defender1.geneTypes.Count != defender2.geneTypes.Count)
        {
            return false;
        }

        // 比较两个防御者的每个基因型
        for (int i = 0; i < defender1.geneTypes.Count; i++)
        {
            GeneTypeEntry gene1 = defender1.geneTypes[i];
            GeneTypeEntry gene2 = defender2.geneTypes[i];

            // 如果任一基因的名称或类型不相同，则认为两个防御者的基因型不相等
            if (gene1.geneName != gene2.geneName || gene1.geneType != gene2.geneType)
            {
                return false;
            }
        }

        // 所有基因型都相同
        return true;
    }


    private bool AreGeneTypesEqual(DefenderWithCount defenderWithCount, Defender defenderB)
    {
        // 获取两个防御者的基因型列表
        List<GeneTypeEntry> geneTypesA = defenderWithCount.defender.geneTypes;
        List<GeneTypeEntry> geneTypesB = defenderB.geneTypes;

        // 比较列表长度是否相等
        if (geneTypesA.Count != geneTypesB.Count) return false;

        // 检查每个基因型是否相等
        for (int i = 0; i < geneTypesA.Count; i++)
        {
            if (geneTypesA[i].geneName != geneTypesB[i].geneName || geneTypesA[i].geneType != geneTypesB[i].geneType)
            {
                return false;
            }
        }
        return true;
    }


    public void PerformAnalysis()
    {
        if (selectedSlot != null)
        {
            int index = tissueSlots.IndexOf(selectedSlot);
            if (index < tissues.Count)
            {
                GeneTypeEntry selectedGeneType = tissues[index];
                // 显示基因型信息
                //geneTypeInfoText.text = $"Selected Gene Type: {selectedGeneType}";

                string geneTypeText = $"The gene you have analyzed is  ";
                switch (selectedGeneType.geneName)
                {
                    case GeneInfo.geneTypesName.A:
                        switch (selectedGeneType.geneType)
                        {
                            case GeneInfo.geneTypes.Dom:
                                geneTypeInfoText.text = geneTypeText + "AA";
                                break;
                            case GeneInfo.geneTypes.Het:
                                geneTypeInfoText.text = geneTypeText + "Aa";
                                break;
                            case GeneInfo.geneTypes.Rec:
                                geneTypeInfoText.text = geneTypeText + "aa";
                                break;
                        }
                        break;
                    case GeneInfo.geneTypesName.B:
                        switch (selectedGeneType.geneType)
                        {
                            case GeneInfo.geneTypes.Dom:
                                geneTypeInfoText.text = geneTypeText + "BB";
                                break;
                            case GeneInfo.geneTypes.Het:
                                geneTypeInfoText.text = geneTypeText + "Bb";
                                break;
                            case GeneInfo.geneTypes.Rec:
                                geneTypeInfoText.text = geneTypeText + "bb";
                                break;
                        }
                        break;
                    case GeneInfo.geneTypesName.C:
                        switch (selectedGeneType.geneType)
                        {
                            case GeneInfo.geneTypes.Dom:
                                geneTypeInfoText.text = geneTypeText + "CC";
                                break;
                            case GeneInfo.geneTypes.Het:
                                geneTypeInfoText.text = geneTypeText + "Cc";
                                break;
                            case GeneInfo.geneTypes.Rec:
                                geneTypeInfoText.text = geneTypeText + "cc";
                                break;
                        }
                        break;
                        /*  case GeneInfo.geneTypes.Dom:
                              geneTypeInfoText.text = geneTypeText + "Dominant";
                              break;
                          case GeneInfo.geneTypes.Het:
                              geneTypeInfoText.text = geneTypeText + "Heterozygous";
                              break;
                          case GeneInfo.geneTypes.Rec:
                              geneTypeInfoText.text = geneTypeText + "Recessive";
                              break;*/

                }
                UpdateGeneLibrary(selectedGeneType);


                tissues.RemoveAt(index);

              
                selectedSlot.GetComponent<Image>().color = Color.white;
                selectedSlot = null;

               
                UpdateCountsDisplay();

                PopulateTissueList();

                // 检查是否需要调整当前页码
                CheckCurrentPage();

                analyzeUI.SetActive(false);
            }
        }
        else
        {
            geneTypeInfoText.text = "No tissue selected.";
            analyzeUI.SetActive(false);
        }
    }

    private void UpdateGeneLibrary(GeneTypeEntry geneTypeEntry)
    {
        bool updated = false;

        // 遍历所有基因数据
        foreach (GeneTypeEntry geneData in geneDatabase.allGenes)
        {
            // 检查基因名称和类型是否匹配
            if (geneData.geneName == geneTypeEntry.geneName && geneData.geneType == geneTypeEntry.geneType && !geneData.isOwned)
            {
                geneData.isOwned = true; // 更新为拥有
                updated = true; // 标记为已更新
                Debug.Log("Updated");
                break; // 找到并更新后退出循环

            }
        }

        // 如果有基因型状态更新
        if (updated)
        {
            // 通知基因库显示组件和下拉菜单组件刷新显示
            if (geneLibraryDisplay != null) geneLibraryDisplay.RefreshDisplay();
            if (geneDropdownPopulator != null) geneDropdownPopulator.RefreshDisplay();
        }
    }


    void CheckCurrentPage()
    {
        totalPage = Mathf.CeilToInt((float)tissues.Count / itemsPerPage);
        currentPage = Mathf.Clamp(currentPage, 0, totalPage - 1);

        // 更新翻页按钮的状态
        nextPageButton.gameObject.SetActive(totalPage > 1);
        previousPageButton.gameObject.SetActive(totalPage > 1);
        nextPageButton.interactable = currentPage < totalPage - 1;
        previousPageButton.interactable = currentPage > 0;
    }




    public Defender PerformFusion(Defender defender, GeneTypeEntry newGeneType)
    {
        if (defender != null)
        {
            Defender newDefender = new Defender(defender);


            // 遍历defender中的基因类型
            foreach (var existingGene in newDefender.geneTypes)
            {
                // 如果当前的基因在新的基因类型中存在，则进行融合
                if (existingGene.geneName == newGeneType.geneName)
                {
                    existingGene.geneType = MendelianInheritance(existingGene.geneType, newGeneType.geneType);
                    // 由于只有一个新的基因类型，我们可以在找到匹配项后终止循环
                    break;
                }
            }

            return newDefender;
        }
        return null;
    }


    private GeneInfo.geneTypes MendelianInheritance(GeneInfo.geneTypes geneType1, GeneInfo.geneTypes geneType2)
    {
    
        if (geneType1 == GeneInfo.geneTypes.Null) return geneType2;
        if (geneType2 == GeneInfo.geneTypes.Null) return geneType1;

        List<GeneInfo.geneTypes> possibleOutcomes = new List<GeneInfo.geneTypes>();

        // 根据组合添加可能的结果
        if (geneType1 == GeneInfo.geneTypes.Het && geneType2 == GeneInfo.geneTypes.Het)
        {
            // Aa + Aa 的情况
            possibleOutcomes.Add(GeneInfo.geneTypes.Dom); // Dom的几率为25%
            possibleOutcomes.Add(GeneInfo.geneTypes.Het); // Het的几率为50%
            possibleOutcomes.Add(GeneInfo.geneTypes.Het);
            possibleOutcomes.Add(GeneInfo.geneTypes.Rec); // Rec的几率为25%
        }
        else if ((geneType1 == GeneInfo.geneTypes.Dom && geneType2 == GeneInfo.geneTypes.Het) ||
                 (geneType1 == GeneInfo.geneTypes.Het && geneType2 == GeneInfo.geneTypes.Dom))
        {
            
            possibleOutcomes.Add(GeneInfo.geneTypes.Dom); // ADom
            possibleOutcomes.Add(GeneInfo.geneTypes.Het); // AHet
        }
        else if (geneType1 == GeneInfo.geneTypes.Het && geneType2 == GeneInfo.geneTypes.Rec ||
                 geneType1 == GeneInfo.geneTypes.Rec && geneType2 == GeneInfo.geneTypes.Het)
        {
            
            possibleOutcomes.Add(GeneInfo.geneTypes.Het); // AHet
            possibleOutcomes.Add(GeneInfo.geneTypes.Rec); // ARec
        }
        else if ((geneType1 == GeneInfo.geneTypes.Dom && geneType2 == GeneInfo.geneTypes.Rec) ||
        (geneType1 == GeneInfo.geneTypes.Rec && geneType2 == GeneInfo.geneTypes.Dom))
        {
            
            return GeneInfo.geneTypes.Het;
            
        }
        else if (geneType1 == GeneInfo.geneTypes.Dom && geneType2 == GeneInfo.geneTypes.Dom)
        {
           
            return GeneInfo.geneTypes.Dom;
        }
        else if (geneType1 == GeneInfo.geneTypes.Rec && geneType2 == GeneInfo.geneTypes.Rec)
        {
           
            return GeneInfo.geneTypes.Rec;
        }


        // 随机选择一个可能的结果
        int index = Random.Range(0, possibleOutcomes.Count);

        /*Debug.Log("--------------------");
        Debug.Log(possibleOutcomes[index]);
        Debug.Log("----------------------------------------");*/
        return possibleOutcomes[index];

       
    }



    private bool IsSameGeneType(List<GeneInfo.geneTypes> geneTypes1, List<GeneInfo.geneTypes> geneTypes2)
    {
        
        return geneTypes1.Count == geneTypes2.Count && geneTypes1[0] == geneTypes2[0];
    }
    public void EndBreedingPhase()
    {

    }

    public void ResetBreedingUI()
    {
        // 清空展示培育结果的Image
        //breedResultImage.sprite = null;

        // 重置Dropdown到默认选项
        geneDropdownPopulator.ResetToDefaultOption();

        // 隐藏选择Defender的面板
        chooseBreedDefendersPanel.SetActive(false);

        defenderDisplayImage.sprite = null;




    }


}
