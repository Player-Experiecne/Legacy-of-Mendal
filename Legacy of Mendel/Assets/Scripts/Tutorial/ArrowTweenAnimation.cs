using UnityEngine;
using DG.Tweening; // 引用DOTween

public class ArrowTweenAnimation : MonoBehaviour
{
    void Start()
    {
        // 使箭头在垂直方向上下移动
        transform.DOMoveY(1f, 1f).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
    }
}
