using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DefenderChoose : MonoBehaviour
{
    // Start is called before the first frame update
    // 在类的开头添加
    private Dictionary<int, int> defenderSelectionCounts = new Dictionary<int, int>();
    public GameObject defenderDisplayArea; // 指向右侧显示区域的引用

    public GameObject SummonerPanel;
    public GameObject MaxReminderPanel;
    public GameObject NoDefenderReminderPanel;
    public GameObject CloneReminderPanel;
    public GameObject ClonePanel;
    public List<Image> defenderDisplays; // 在Inspector中设置的Image列表
    public TextMeshProUGUI text;
    public TextMeshProUGUI clone;
    public TextMeshProUGUI reminder;
    private Defender def;


    public List<Defender> selectedDefenders = new List<Defender>();

    public static BreedManager Instance;

    public int itemsPerPage = 10;
    private int currentPage = 0;
    private int totalPageCount = 0;
    public Button nextPageButton;
    public Button previousPageButton;

    private List<int> selectedIndices = new List<int>();


    public PlayerDefenderInventory playerDefenderInventory;
    public List<Image> combatUnitImages;


    public Image defenderDisplayImage;
    public Button confirmButton;
    public GameObject thisPanel;

    public int i = 0;
    //public GameObject previousPanel; 

    private int lastInventoryCount = -1;

    public const int MAX_DEFENDERS = 8;


    private int selectedIndex = -1;
    void InitializeDefenderDisplays()
    {
        foreach (Transform child in defenderDisplayArea.transform)
        {
            //defenderDisplays.Add(child.gameObject); // 将每个子GameObject添加到列表中
            child.gameObject.SetActive(false); // 初始化时隐藏所有防御者
        }
    }
    void Start()
    {
        // 初始化其他组件
        UpdatePageCount();
        RefreshDisplay();
        UpdatePageButtons();
        UpdateCountsDisplay();

        // 清空所有展示项
        foreach (var display in defenderDisplays)
        {
            display.sprite = null;
        }
    }
   
    public void IncreaseCount(int globalIndex)
    {
        Debug.Log("IncreaseCount called for index " + globalIndex);
        EventSystem.current.SetSelectedGameObject(null);  // 取消所有UI元素的选中状态
        if (selectedIndices.Contains(globalIndex))
        {
            int maxCount = playerDefenderInventory.ownedDefenders[globalIndex].count;
            defenderSelectionCounts[globalIndex] = Mathf.Min(defenderSelectionCounts.GetValueOrDefault(globalIndex, 0) + 1, maxCount);
            UpdateCountText(globalIndex);
        }
    }




    public void UpdateCountsDisplay()
    {

        text.text = LootBackpack.Instance.lootCultureMedium.ToString();
    }

    public void DecreaseCount(int globalIndex)
    {
        Debug.Log("DecreaseCount called for index " + globalIndex);
        EventSystem.current.SetSelectedGameObject(null);  // 取消所有UI元素的选中状态
        if (selectedIndices.Contains(globalIndex) && defenderSelectionCounts.ContainsKey(globalIndex) && defenderSelectionCounts[globalIndex] > 0)
        {
            defenderSelectionCounts[globalIndex]--;
            UpdateCountText(globalIndex);
        }
    }


    public void NextPage()
    {
        if (currentPage < totalPageCount - 1)
        {
            currentPage++;
            RefreshDisplay();
            UpdatePageButtons();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            RefreshDisplay();
            UpdatePageButtons();
        }
    }

    private void UpdatePageCount()
    {
        totalPageCount = Mathf.CeilToInt((float)playerDefenderInventory.ownedDefenders.Count / itemsPerPage);
    }

    private void UpdatePageButtons()
    {
        // 控制翻页按钮的可用性
        nextPageButton.interactable = currentPage < totalPageCount - 1;
        previousPageButton.interactable = currentPage > 0;

        // 如果总项目数少于每页显示数，则隐藏翻页按钮
        nextPageButton.gameObject.SetActive(totalPageCount > 1);
        previousPageButton.gameObject.SetActive(totalPageCount > 1);
    }



    void Update()
    {
        UpdateCountsDisplay();
        // 检查库存数量是否有变化
        if (playerDefenderInventory.ownedDefenders.Count != lastInventoryCount)
        {
            RefreshDisplay(); // 如果有变化，更新显示
            lastInventoryCount = playerDefenderInventory.ownedDefenders.Count; // 更新最后的库存数量，以便下次检查
        }
        if (HasInventoryCountChanged())
        {
            UpdateDefenderDisplays();
        }
    }

    private bool HasInventoryCountChanged()
    {
        // 这个方法检查每个defender的数量是否有变化
        // 如果是，则返回true，否则返回false
        for (int i = 0; i < playerDefenderInventory.ownedDefenders.Count; i++)
        {
            if (playerDefenderInventory.ownedDefenders[i].count != lastInventoryCount)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateDefenderDisplays()
    {
        int itemCount = Mathf.Min(playerDefenderInventory.ownedDefenders.Count, combatUnitImages.Count);  // 使用较小的数量来避免越界
        for (int i = 0; i < itemCount; i++)  // 使用新的边界来循环
        {
            Transform countTextTransform = combatUnitImages[i].transform.Find("CountText");
            if (countTextTransform != null)
            {
                TextMeshProUGUI countTextComponent = countTextTransform.GetComponent<TextMeshProUGUI>();
                if (countTextComponent != null)
                {
                    // 更新数量显示
                    countTextComponent.text = "Count: " + playerDefenderInventory.ownedDefenders[i].count.ToString();
                }
            }
        }
        // 如果需要，这里可以加上额外的逻辑来处理不足一行的元素
    }

    public void RefreshDisplay()
    {
        for (int i = 0; i < combatUnitImages.Count; i++)
        {
            int globalIndex = currentPage * itemsPerPage + i; // 计算全局索引
            if (globalIndex < playerDefenderInventory.ownedDefenders.Count)
            {
                combatUnitImages[i].sprite = playerDefenderInventory.ownedDefenders[globalIndex].defender.defenderImage;
                combatUnitImages[i].gameObject.SetActive(true);
                UpdateText(combatUnitImages[i], "NameText", playerDefenderInventory.ownedDefenders[globalIndex].defender.defenderName);
                UpdateText(combatUnitImages[i], "CountText", "Count: " + playerDefenderInventory.ownedDefenders[globalIndex].count.ToString());

                if (globalIndex == selectedIndex)
                {
                    combatUnitImages[i].color = Color.green;
                    ShowControls(i);
                }
                else
                {
                    combatUnitImages[i].color = Color.white;
                    HideControls(i);
                }

                int indexCopy = globalIndex; // 捕获当前全局索引以在lambda表达式中使用
                combatUnitImages[i].GetComponent<Button>().onClick.RemoveAllListeners();
                combatUnitImages[i].GetComponent<Button>().onClick.AddListener(() => ToggleSelection(indexCopy));
            }
            else
            {
                combatUnitImages[i].gameObject.SetActive(false);
            }
        }
    }




    private void ClearSelections()
    {
        foreach (var image in combatUnitImages)
        {
            image.color = Color.white;  // 清除高亮
        }
        //selectedIndices.Clear();  // 清空选中列表
    }



    private void ShowControls(int imageIndex)
    {
        /*Transform controlsContainer = combatUnitImages[imageIndex].transform.Find("ControlsContainer");
        if (controlsContainer != null)
        {
            controlsContainer.gameObject.SetActive(true);
        }*/
    }

    private void HideControls(int imageIndex)
    {
        /* Transform controlsContainer = combatUnitImages[imageIndex].transform.Find("ControlsContainer");
         if (controlsContainer != null)
         {
             controlsContainer.gameObject.SetActive(false);
         }*/
    }


    private void UpdateText(Image parentImage, string childName, string text)
    {
        Transform childTransform = parentImage.transform.Find(childName);
        if (childTransform != null)
        {
            TextMeshProUGUI textComponent = childTransform.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = text;
            }
        }
    }





    private void ToggleSelection(int globalIndex)
    {
        int localIndex = globalIndex % itemsPerPage; // 计算本地索引，即页面上的位置

        if (selectedIndex == globalIndex)
        {
            // 如果点击的是已选中的项目，取消选中
            combatUnitImages[localIndex].color = Color.white;
            selectedIndex = -1;
            //defenderDisplayImage.sprite = null;
        }
        else
        {
            if (selectedIndex >= 0 && selectedIndex < playerDefenderInventory.ownedDefenders.Count)
            {
                // 取消之前选中的项目的高亮，只有当之前选中的项目在当前页时才需要取消高亮
                int previousLocalIndex = selectedIndex % itemsPerPage;
                if (currentPage == selectedIndex / itemsPerPage)
                {
                    combatUnitImages[previousLocalIndex].color = Color.white;
                }
            }

            // 设置新选中的项目的高亮
            combatUnitImages[localIndex].color = Color.green;
            selectedIndex = globalIndex;
            //defenderDisplayImage.sprite = playerDefenderInventory.ownedDefenders[globalIndex].defender.defenderImage;
        }
    }










    public void ConfirmSelection()
    {
        if (selectedDefenders.Count >= MAX_DEFENDERS)
        {
            ShowMaxReachedMessage();


            if (selectedIndex >= 0 && selectedIndex < combatUnitImages.Count)
            {
                int localIndex = selectedIndex % itemsPerPage;
                if (currentPage == selectedIndex / itemsPerPage)
                {
                    combatUnitImages[localIndex].color = Color.white;
                }
            }

            selectedIndex = -1;
            return;
        }


        if (selectedIndex >= 0 && selectedIndex < playerDefenderInventory.ownedDefenders.Count)
        {
            Defender selectedDefender = playerDefenderInventory.ownedDefenders[selectedIndex].defender;
            playerDefenderInventory.DecreaseDefenderCount(selectedDefender);

            selectedDefenders.Add(selectedDefender);

            // 找到第一个空的位置并添加，不管之前是否添加过相同的defender
            for (int i = 0; i < defenderDisplays.Count; i++)
            {
                if (defenderDisplays[i].sprite == null)
                {
                    defenderDisplays[i].sprite = selectedDefender.defenderImage;
                    defenderDisplays[i].gameObject.SetActive(true);
                    Transform nameTextTransform = defenderDisplays[i].transform.Find("NameText");
                    if (nameTextTransform != null)
                    {
                        TextMeshProUGUI nameText = nameTextTransform.GetComponent<TextMeshProUGUI>();
                        nameText.text = selectedDefender.defenderName;
                        nameText.gameObject.SetActive(true);
                    }

                    // 绑定移除事件
                    Button button = defenderDisplays[i].GetComponent<Button>();
                    if (button == null)
                    {
                        button = defenderDisplays[i].gameObject.AddComponent<Button>();
                    }
                    button.onClick.RemoveAllListeners();
                    int indexCopy = i;  // Capture the index for use in the lambda expression
                    button.onClick.AddListener(() => RemoveDefenderFromSelection(selectedDefender, indexCopy));

                    break;
                }
            }

            selectedIndex = -1; // 清除选中状态
            ClearSelections();
        }
    }

    public void ToggleDefenderClone()
    {

        ClonePanel.SetActive(true);
        if (selectedIndex >= 0 && selectedIndex < playerDefenderInventory.ownedDefenders.Count)
        {
            Defender selectedDefender = playerDefenderInventory.ownedDefenders[selectedIndex].defender;
            def = selectedDefender;
        }

    }

    public void ConfirmClone(){
        int num = int.Parse(clone.text);
        int cost = num * 10;
        if (cost >= LootBackpack.Instance.lootCultureMedium)
        {
            reminder.text = "You don't have enough CultureMedium";
        }
        else
        {
            for(int i=0; i <= num; i++) {
                playerDefenderInventory.AddDefenderToInventory(def);
            }
            reminder.text = null;
            LootBackpack.Instance.lootCultureMedium -= cost;
            ClearSelections();
            clone.text = "0";
            ClonePanel.SetActive(false);
        }

    }


    public void IncreaseCloneNumber()
    {
        if (int.TryParse(clone.text, out int num))
        {
            num++;
            clone.text = num.ToString();
        }
        else
        {
            clone.text = "0"; 
        }
    }

    public void DecreaseCloneNumber()
    {
        if (int.TryParse(clone.text, out int num) && num > 0)
        {
            num--;
            clone.text = num.ToString();
        }
    }

    private void RemoveDefenderFromSelection(Defender defender, int displayIndex)
    {
        if (selectedDefenders.Contains(defender))
        {
            selectedDefenders.Remove(defender);
            playerDefenderInventory.AddDefenderToInventory(defender);

            // 清理UI
            defenderDisplays[displayIndex].sprite = null;
            Transform nameTextTransform = defenderDisplays[displayIndex].transform.Find("NameText");
            if (nameTextTransform != null)
            {
                nameTextTransform.gameObject.SetActive(false);
            }

            // Optional: Update UI if needed
            RefreshDisplay();
        }
    }

    private void ShowMaxReachedMessage()
    {
        MaxReminderPanel.SetActive(true);
    }

    public void ShowNoDefenderMessage()
    {
        NoDefenderReminderPanel.SetActive(true);
    }

    public void HideNoDefenderMessage()
    {
        NoDefenderReminderPanel.SetActive(false);
    }
    public void HideMaxReachedMessage()
    {
        MaxReminderPanel.SetActive(false);
    }
    private void UpdateCountText(int globalIndex)
    {
        // Only update if the current page matches the global index
        int localIndex = globalIndex % itemsPerPage;
        if (currentPage == globalIndex / itemsPerPage)
        {
            Transform controlsContainer = combatUnitImages[localIndex].transform.Find("ControlsContainer");
            if (controlsContainer != null)
            {
                TextMeshProUGUI countText = controlsContainer.Find("ChooseCount").GetComponent<TextMeshProUGUI>();
                if (countText != null)
                {
                    countText.text = defenderSelectionCounts.GetValueOrDefault(globalIndex, 0).ToString();
                }
            }
        }
    }
    public void ClosePanel()
    {
        thisPanel.SetActive(false);

    }


    public void ClearDisplayArea()
    {

        if (selectedDefenders.Count == 0)
        {
            ShowNoDefenderMessage();
            return;
        }

        foreach (var display in defenderDisplays)
        {
            display.sprite = null;  
            display.transform.Find("NameText").gameObject.SetActive(false);  
        }
        foreach(Defender df in selectedDefenders)
        {
            DefenderBackpack.Instance.AddDefenderToBackpack(df);
            
            
        }
        
            
        SummonerPanel.SetActive(true);
    }


}
