using UnityEngine;
using TMPro; // 引用TextMeshPro命名空间
using System.Collections.Generic;

public class GeneDropdownPopulator : MonoBehaviour
{
    public GeneDatabase genedatabase; 
    public TMP_Dropdown geneDropdown; 

    private List<GeneTypeEntry> dropdownGeneTypes = new List<GeneTypeEntry>();
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
        //geneDropdown.options.Add(new TMP_Dropdown.OptionData("Choose the gene type"));

        geneDropdown.options.Add(new TMP_Dropdown.OptionData("Choose the gene type"));
        var defaultEntry = new GeneTypeEntry { geneName = GeneInfo.geneTypesName.Null, geneType = GeneInfo.geneTypes.Null };
        dropdownGeneTypes.Add(defaultEntry);
        //dropdownGeneTypes.Add(GeneInfo.geneTypes.Null); // 为默认选项匹配一个Null类型

        foreach (GeneTypeEntry gene in genedatabase.allGenes)
          {
              if (gene.isOwned)
              {
                string displayText = $"{gene.geneName} - {gene.geneType}";
                geneDropdown.options.Add(new TMP_Dropdown.OptionData(displayText));
                var newEntry = new GeneTypeEntry { geneName = gene.geneName, geneType = gene.geneType }; // 创建新的GeneTypeEntry实例
                dropdownGeneTypes.Add(newEntry);
                
              }
          }
  
        geneDropdown.RefreshShownValue();
    }

    void OnDropdownValueChanged(int index)
    {
        // 通过索引从存储的基因型列表中获取选中的基因型
        GeneTypeEntry selectedGeneType = dropdownGeneTypes[index];
        BreedManager.Instance.SetSelectedGeneType(selectedGeneType);
    }

    public void ResetToDefaultOption()
    {
        geneDropdown.value = 0; 
        geneDropdown.RefreshShownValue(); 
    }

}
