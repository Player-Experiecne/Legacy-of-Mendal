using UnityEngine;
using UnityEngine.UI; // 确保引用了UI命名空间
using UnityEngine.EventSystems; // 用于事件系统

public class HighlightOnClick : MonoBehaviour
{
    public Color highlightColor = Color.green; // 设置高亮颜色
    public GameObject Image1;
    public GameObject Image2;
    private Button button; // Button组件

    void Start()
    {
        // 获取Button组件，如果没有则报错
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found!");
            return;
        }

        // 为按钮添加点击事件监听
        button.onClick.AddListener(SetPermanentHighlight);
    }

    public void SetPermanentHighlight()
    {
        // 更改Button颜色为高亮颜色
        ColorBlock colors = button.colors;
        colors.normalColor = highlightColor;
        colors.highlightedColor = highlightColor;
        colors.pressedColor = highlightColor;
        colors.selectedColor = highlightColor;
        // 更新Button的颜色配置
        button.colors = colors;

        // 切换Image1和Image2的激活状态
        if (Image1 != null && Image2 != null)
        {
            Image1.SetActive(!Image1.activeSelf);
            Image2.SetActive(!Image2.activeSelf);
        }

        // 移除监听器，以确保颜色改变只发生一次
        button.onClick.RemoveListener(SetPermanentHighlight);
    }

    void OnDestroy()
    {
        // 确保在对象销毁时移除监听器，避免潜在的错误
        button.onClick.RemoveListener(SetPermanentHighlight);
    }
}
