using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BreedManager : MonoBehaviour
{
    // UI 引用
    public static BreedManager Instance;

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
                // 假设你有一个方法来将新的Defender添加到玩家的库存中
                playerDefenderInventory.AddDefender(newDefender);
                playerDefenderInventory.DecreaseDefenderCount(selectedDefender);
                // 可能还需要一些UI更新逻辑来显示培育的结果
                //UpdateUIAfterBreeding(newDefender);
                Debug.Log(newDefender);
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

    public Defender PerformFusion(Defender defender, GeneInfo.geneTypes newGeneType)
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
    }


    public void EndBreedingPhase()
    {

    }

}
