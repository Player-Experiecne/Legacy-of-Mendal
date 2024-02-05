using UnityEngine;
using TMPro; // 引用TextMeshPro命名空间
using System.Collections.Generic;

public class GeneDropdownPopulator : MonoBehaviour
{
    public GeneLibrary geneLibrary; // 引用你的GeneLibrary ScriptableObject
    public TMP_Dropdown geneDropdown; // 引用TextMeshPro Dropdown组件

    void Start()
    {
        PopulateDropdown();
        // 添加监听器以处理选项改变事件
        geneDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void PopulateDropdown()
    {
        geneDropdown.options.Clear(); // 清空现有的选项

        // 首先添加一个默认选项提示玩家选择基因型
        TMP_Dropdown.OptionData defaultOption = new TMP_Dropdown.OptionData("Choose the gene type");
        geneDropdown.options.Add(defaultOption);

        // 遍历所有基因型，添加已拥有的基因型到Dropdown
        foreach (GeneTypeData gene in geneLibrary.allGenes)
        {
            if (gene.isOwned)
            {
                TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData(gene.name);
                geneDropdown.options.Add(newOption);
            }
        }

        geneDropdown.RefreshShownValue(); // 刷新Dropdown以显示新的选项
    }

    void OnDropdownValueChanged(int index)
    {
        // 默认选项是第一个，所以index大于0时才处理
        if (index > 0)
        {
            // Dropdown的选项索引与geneLibrary.allGenes中的索引相匹配，但要考虑到默认选项的存在
            GeneTypeData selectedGeneData = geneLibrary.allGenes[index - 1];
            BreedManager.Instance.SetSelectedGeneType(selectedGeneData.geneType);
        }
        else
        {
            // 如果选择了默认选项，则将selectedGeneType设置为Null
            BreedManager.Instance.SetSelectedGeneType(GeneInfo.geneTypes.Null);
        }
    }
}
