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
    private List<GeneInfo.geneTypes> tissues; 
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

        tissues = LootBackpack.Instance.lootGeneTypes;
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

    public void OnBreedButtonClick()
    {
        if (selectedDefender != null && selectedGeneType != GeneInfo.geneTypes.Null)
        {
            // 执行融合逻辑
            Defender newDefender = PerformFusion(selectedDefender,selectedGeneType);
            if (newDefender != null)
            {
                foreach (Defender defenderInLibrary in defenderLibrary.defenders)
                {
                    // 这里我们比较基因型，假设基因型是一个GeneInfo.geneTypes的List
                    if (IsSameGeneType(newDefender.geneTypes, defenderInLibrary.geneTypes))
                    {
                        // 如果找到匹配的基因型，展示对应Defender的图像
                        breedResultImage.sprite = defenderInLibrary.defenderImage;
                        newDefender.defenderPrefab = defenderInLibrary.defenderPrefab;
                        newDefender.defenderName = defenderInLibrary.defenderName;
                        break; // 找到匹配后退出循环
                    }
                }
                playerDefenderInventory.AddDefender(newDefender);
                playerDefenderInventory.DecreaseDefenderCount(selectedDefender);
               
                //UpdateUIAfterBreeding(newDefender);
                Debug.Log(newDefender.geneTypes);
            }
          
        }
        else
        {
          
            
        }
    }

    public void PerformAnalysis()
    {
        if (selectedSlot != null)
        {
            int index = tissueSlots.IndexOf(selectedSlot);
            if (index < tissues.Count)
            {
                GeneInfo.geneTypes selectedGeneType = tissues[index];
                // 显示基因型信息
                geneTypeInfoText.text = $"Selected Gene Type: {selectedGeneType}";

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



    /* public Defender PerformFusion(Defender defender, GeneInfo.geneTypes newGeneType)
     {
         if (selectedDefender != null && selectedGeneType != GeneInfo.geneTypes.Null)
         {

             Defender newDefender = new Defender(defender);


             GeneInfo.geneTypes resultGeneType;


             if (newGeneType == GeneInfo.geneTypes.Null || defender.geneTypes[0] == GeneInfo.geneTypes.Null)
             {

                 resultGeneType = newGeneType == GeneInfo.geneTypes.Null ? defender.geneTypes[0] : newGeneType;
             }
             else if (newGeneType == GeneInfo.geneTypes.ADom || defender.geneTypes[0] == GeneInfo.geneTypes.ADom)
             {
                 resultGeneType = GeneInfo.geneTypes.ADom;
             }
             else if (newGeneType == GeneInfo.geneTypes.AHet && defender.geneTypes[0] != GeneInfo.geneTypes.ADom)
             {
                 resultGeneType = GeneInfo.geneTypes.AHet;
             }
             else if (newGeneType == GeneInfo.geneTypes.ARec && defender.geneTypes[0] != GeneInfo.geneTypes.ADom && defender.geneTypes[0] != GeneInfo.geneTypes.AHet)
             {
                 resultGeneType = GeneInfo.geneTypes.ARec;
             }
             else
             {

                 resultGeneType = newGeneType;
             }

             // 清除任何现有的基因型并添加确定的基因型
             newDefender.geneTypes.Clear();
             newDefender.geneTypes.Add(resultGeneType);

             return newDefender;
         }
         return null;
     }*/

    public Defender PerformFusion(Defender defender, GeneInfo.geneTypes newGeneType)
    {
        // 确保已经有选中的Defender和基因型
        if (selectedDefender != null && selectedGeneType != GeneInfo.geneTypes.Null)
        {
            // 创建新的Defender实例
            Defender newDefender = new Defender(defender); // 假设Defender有一个复制构造函数或相应的方法

            // 根据孟德尔遗传定律来确定新的基因型
            GeneInfo.geneTypes resultGeneType = MendelianInheritance(defender.geneTypes[0], newGeneType);

            // 清除任何现有的基因型并设置新的基因型
            newDefender.geneTypes.Clear();
            newDefender.geneTypes.Add(resultGeneType);

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
        if (geneType1 == GeneInfo.geneTypes.AHet && geneType2 == GeneInfo.geneTypes.AHet)
        {
            // Aa + Aa 的情况，有1/4的几率是ADom, 1/2的几率是AHet, 1/4的几率是ARec
            possibleOutcomes.Add(GeneInfo.geneTypes.ADom);
            possibleOutcomes.Add(GeneInfo.geneTypes.AHet);
            possibleOutcomes.Add(GeneInfo.geneTypes.AHet);
            possibleOutcomes.Add(GeneInfo.geneTypes.ARec);
        }
        else if (geneType1 == GeneInfo.geneTypes.AHet && geneType2 == GeneInfo.geneTypes.ARec || geneType1 == GeneInfo.geneTypes.ARec && geneType2 == GeneInfo.geneTypes.AHet)
        {
            // Aa + aa 或 aa + Aa 的情况，有1/2的几率是AHet, 1/2的几率是ARec
            possibleOutcomes.Add(GeneInfo.geneTypes.AHet);
            possibleOutcomes.Add(GeneInfo.geneTypes.ARec);
        }
        else
        {
            // 其他组合情况下，结果是确定的
            possibleOutcomes.Add(geneType1 == GeneInfo.geneTypes.ADom || geneType2 == GeneInfo.geneTypes.ADom ? GeneInfo.geneTypes.ADom : geneType1 == GeneInfo.geneTypes.ARec && geneType2 == GeneInfo.geneTypes.ARec ? GeneInfo.geneTypes.ARec : GeneInfo.geneTypes.AHet);
        }

        // 随机选择一个可能的结果
        int index = Random.Range(0, possibleOutcomes.Count);
        return possibleOutcomes[index];
    }



    private bool IsSameGeneType(List<GeneInfo.geneTypes> geneTypes1, List<GeneInfo.geneTypes> geneTypes2)
    {
        
        return geneTypes1.Count == geneTypes2.Count && geneTypes1[0] == geneTypes2[0];
    }
    public void EndBreedingPhase()
    {

    }

}
