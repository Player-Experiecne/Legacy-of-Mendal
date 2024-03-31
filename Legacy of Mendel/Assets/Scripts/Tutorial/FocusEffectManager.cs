using UnityEngine;

public class FocusEffectManager : MonoBehaviour
{
    public GameObject focusEffectPrefab; // 聚焦效果的预制体
    private GameObject currentFocusEffect; // 当前的聚焦效果实例

    // 聚焦到指定目标
    public void FocusOn(Transform target)
    {
        // 如果当前已有聚焦效果实例，则先销毁
        if (currentFocusEffect != null)
        {
            Destroy(currentFocusEffect);
        }

        // 创建新的聚焦效果实例并定位到目标对象
        currentFocusEffect = Instantiate(focusEffectPrefab, target.position, Quaternion.identity);
        currentFocusEffect.transform.SetParent(target, false); // 使聚焦效果跟随目标移动
    }

    // 取消当前的聚焦效果
    public void ClearFocus()
    {
        if (currentFocusEffect != null)
        {
            Destroy(currentFocusEffect);
            currentFocusEffect = null;
        }
    }
}
