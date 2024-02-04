using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BreedManager : MonoBehaviour
{
    // UI 引用
    
    public List<GameObject> tissueSlots; 
    public TextMeshProUGUI tissueCountText; 
    public TextMeshProUGUI cultureMediumCountText; 
    public Button nextPageButton; 
    public Button previousPageButton; 
    public Sprite tissueSprite;
    public GameObject analyzeUI;
    public TextMeshProUGUI geneTypeInfoText;

    private GameObject selectedSlot = null;

    // 分页数据
    private List<GeneInfo.geneTypes> tissues; 
    private int itemsPerPage = 10; 
    private int currentPage = 0; 
    private int totalPage; 

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
        tissues = LootBackpack.Instance.lootGeneTypes;
        totalPage = Mathf.CeilToInt((float)tissues.Count / itemsPerPage);
        currentPage = 0; // 重置为第一页

        analyzeUI.SetActive(false);
        UpdateCountsDisplay();

        
        
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

                // 添加监听器以改变选中状态
                int index = i; // 避免闭包中的问题
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
        Defender newDefender = Instantiate(defender.defenderPrefab).GetComponent<Defender>();


        if (newGeneType == GeneInfo.geneTypes.ADom ||
            defender.geneTypes.Contains(GeneInfo.geneTypes.ADom))
        {

            newDefender.geneTypes.Add(GeneInfo.geneTypes.ADom);
        }
        else if (newGeneType == GeneInfo.geneTypes.AHet &&
                 !defender.geneTypes.Contains(GeneInfo.geneTypes.ADom))
        {

            newDefender.geneTypes.Add(GeneInfo.geneTypes.AHet);
        }
        else
        {

            newDefender.geneTypes.Add(GeneInfo.geneTypes.ARec);
        }


        return newDefender;
    }
    public void EndBreedingPhase()
    {

    }
}
