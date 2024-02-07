using UnityEngine;
using TMPro; // 引用TextMeshPro命名空间
using System.Collections.Generic;

public class GeneDropdownPopulator : MonoBehaviour
{
    public GeneLibrary geneLibrary; // 引用你的GeneLibrary ScriptableObject
    public TMP_Dropdown geneDropdown; // 引用TextMeshPro Dropdown组件

    private List<GeneInfo.geneTypes> dropdownGeneTypes = new List<GeneInfo.geneTypes>();
    void Start()
    {
        // 在开始时刷新一次下拉菜单
        RefreshDisplay();
        // 添加监听器以处理选项改变事件
        geneDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void RefreshDisplay()
    {
        PopulateDropdown(); // 刷新Dropdown以显示新的选项
    }

    void PopulateDropdown()
    {
        geneDropdown.options.Clear();
        dropdownGeneTypes.Clear();

        // 添加一个默认选项
        geneDropdown.options.Add(new TMP_Dropdown.OptionData("Choose the gene type"));
        dropdownGeneTypes.Add(GeneInfo.geneTypes.Null); // 为默认选项匹配一个Null类型

        foreach (GeneTypeData gene in geneLibrary.allGenes)
        {
            if (gene.isOwned)
            {
                geneDropdown.options.Add(new TMP_Dropdown.OptionData(gene.name));
                dropdownGeneTypes.Add(gene.geneType); // 存储每个选项的基因型
            }
        }

        geneDropdown.RefreshShownValue();
    }

    void OnDropdownValueChanged(int index)
    {
        // 通过索引从存储的基因型列表中获取选中的基因型
        GeneInfo.geneTypes selectedGeneType = dropdownGeneTypes[index];
        BreedManager.Instance.SetSelectedGeneType(selectedGeneType);
    }

    public void ResetToDefaultOption()
    {
        geneDropdown.value = 0; 
        geneDropdown.RefreshShownValue(); 
    }

}
