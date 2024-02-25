using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BreedManager : MonoBehaviour
{
    // UI 引用
    public static BreedManager Instance;

    public GeneDropdownPopulator geneDropdownPopulator;

    public DefenderLibrary defenderLibrary;
    public Image breedResultImage; 

    public List<GameObject> hiddenUIs;
    public List<GameObject> tissueSlots; 
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
    public Defender selectedDefender;
    public GeneInfo.geneTypes selectedGeneType;

    private GameObject selectedSlot = null;

    public GeneLibrary geneLibrary;
    public GeneLibraryDisplay geneLibraryDisplay;

    // 分页数据
    public List<GeneTypeEntry> tissues;
    private int itemsPerPage = 10; 
    private int currentPage = 0; 
    private int totalPage;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    void Start()
    {
        // 初始化按钮点击事件
        nextPageButton.onClick.AddListener(NextPage);
        previousPageButton.onClick.AddListener(PreviousPage);

        // 默认隐藏翻页按钮
        nextPageButton.gameObject.SetActive(false);
        previousPageButton.gameObject.SetActive(false);
    }

    public void StartBreedingPhase()
    {
        // 获取 LootBackpack 实例中的数据

        foreach (GameObject uiElement in hiddenUIs)
        {
            uiElement.SetActive(false); 
        }

        foreach (var geneTypeCount in LootBackpack.Instance.geneTypeCountsList)
        {
            // 根据每个基因型的计数添加到tissues列表中
            for (int i = 0; i < geneTypeCount.count; i++)
            {
                tissues.Add(geneTypeCount.geneType);
            }
        }
        totalPage = Mathf.CeilToInt((float)tissues.Count / itemsPerPage);
        currentPage = 0; // 重置为第一页

        analyzeUI.SetActive(false);
        UpdateCountsDisplay();

        
        
    }
    public void SetSelectedDefender(Defender defender)
    {
        selectedDefender = defender;
    }

    // 调用这个方法来设置选中的基因型
    public void SetSelectedGeneType(GeneInfo.geneTypes geneType)
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

   /* public void OnBreedButtonClick()
    {
        if (selectedDefender != null && selectedGeneType != GeneInfo.geneTypes.Null)
        {
            // 执行融合逻辑
            Defender newDefender = PerformFusion(selectedDefender, selectedGeneType);
            if (newDefender != null)
            {
                // 查找匹配的Defender在库中的图像和属性
                foreach (Defender defenderInLibrary in defenderLibrary.defenders)
                {
                    // 假设IsSameGeneType现在可以处理新的基因型设计
                    if (IsSameGeneType(newDefender.geneTypes, defenderInLibrary.geneTypes))
                    {
                        // 如果找到匹配的基因型，展示对应Defender的图像和更新属性
                        breedResultImage.sprite = defenderInLibrary.defenderImage;
                        newDefender.defenderPrefab = defenderInLibrary.defenderPrefab;
                        newDefender.defenderName = defenderInLibrary.defenderName;
                        newDefender.defenderImage = defenderInLibrary.defenderImage;
                        break; // 找到匹配后退出循环
                    }
                }
                playerDefenderInventory.AddDefender(newDefender);
                playerDefenderInventory.DecreaseDefenderCount(selectedDefender);

                // 更新UI以显示培育结果
                Debug.Log(newDefender.geneTypes);
                breedResult.text = "You have got defender: " + newDefender.defenderName;

                // 重置培育界面
                ResetBreedingUI();
            }
        }
        else
        {
            // 可能需要处理未选择Defender或基因型的情况
        }
    }
*/

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

                string geneTypeText = $"{selectedGeneType.geneName}: ";
                switch (selectedGeneType.geneType)
                {
                    case GeneInfo.geneTypes.Dom:
                        geneTypeInfoText.text = geneTypeText + "Dominant";
                        break;
                    case GeneInfo.geneTypes.Het:
                        geneTypeInfoText.text = geneTypeText + "Heterozygous";
                        break;
                    case GeneInfo.geneTypes.Rec:
                        geneTypeInfoText.text = geneTypeText + "Recessive";
                        break;
                    
                }
                //UpdateGeneLibrary(selectedGeneType);


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

    private void UpdateGeneLibrary(GeneInfo.geneTypes geneType)
    {
        bool updated = false;
        foreach (GeneTypeData geneData in geneLibrary.allGenes)
        {
            if (geneData.geneType == geneType && !geneData.isOwned)
            {
                geneData.isOwned = true; // 更新为拥有
                updated = true;
                break; // 退出循环，因为我们已经找到并更新了基因型
            }
        }

        if (updated)
        {
            geneLibraryDisplay.RefreshDisplay(); // 更新基因库显示
            geneDropdownPopulator.RefreshDisplay();
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




    public Defender PerformFusion(Defender defender, List<GeneTypeEntry> newGeneTypes)
    {
        if (defender != null)
        {
            Defender newDefender = new Defender(defender);

            foreach (var newGeneType in newGeneTypes)
            {
                var existingGene = newDefender.geneTypes.Find(g => g.geneName == newGeneType.geneName);
                if (existingGene != null)
                {
                    // 这里实现基因型融合逻辑
                    existingGene.geneType = MendelianInheritance(existingGene.geneType, newGeneType.geneType);
                }
                else
                {
                    newDefender.geneTypes.Add(new GeneTypeEntry { geneName = newGeneType.geneName, geneType = newGeneType.geneType });
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
            possibleOutcomes.Add(GeneInfo.geneTypes.Dom); // ADom的几率为25%
            possibleOutcomes.Add(GeneInfo.geneTypes.Het); // AHet的几率为50%
            possibleOutcomes.Add(GeneInfo.geneTypes.Het);
            possibleOutcomes.Add(GeneInfo.geneTypes.Rec); // ARec的几率为25%
        }
        else if ((geneType1 == GeneInfo.geneTypes.Dom && geneType2 == GeneInfo.geneTypes.Het) ||
                 (geneType1 == GeneInfo.geneTypes.Het && geneType2 == GeneInfo.geneTypes.Dom))
        {
            // AA + Aa 或 Aa + AA 的情况，有50%的几率是ADom (AA)，50%的几率是AHet (Aa)
            possibleOutcomes.Add(GeneInfo.geneTypes.Dom); // ADom
            possibleOutcomes.Add(GeneInfo.geneTypes.Het); // AHet
        }
        else if (geneType1 == GeneInfo.geneTypes.Het && geneType2 == GeneInfo.geneTypes.Rec ||
                 geneType1 == GeneInfo.geneTypes.Rec && geneType2 == GeneInfo.geneTypes.Het)
        {
            // Aa + aa 或 aa + Aa 的情况，有50%的几率是AHet, 50%的几率是ARec
            possibleOutcomes.Add(GeneInfo.geneTypes.Het); // AHet
            possibleOutcomes.Add(GeneInfo.geneTypes.Rec); // ARec
        }
        else if ((geneType1 == GeneInfo.geneTypes.Dom && geneType2 == GeneInfo.geneTypes.Rec) ||
        (geneType1 == GeneInfo.geneTypes.Rec && geneType2 == GeneInfo.geneTypes.Dom))
        {
            // 这种组合下，后代必定是杂合子Aa
            return GeneInfo.geneTypes.Het;
            
        }
        else if (geneType1 == GeneInfo.geneTypes.Dom && geneType2 == GeneInfo.geneTypes.Dom)
        {
            // AA + AA 的组合，后代必定是ADom
            return GeneInfo.geneTypes.Dom;
        }
        else if (geneType1 == GeneInfo.geneTypes.Rec && geneType2 == GeneInfo.geneTypes.Rec)
        {
            // aa + aa 的组合，后代必定是ARec
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
