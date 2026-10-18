using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 難易度ごとに、マウスで触れると詳細を表示
public class DifficultyDetail : UIBase
{
    [SerializeField] private GameObject DetailText;

    // このスクリプトがついたオブジェクトにマウスが触れたら
    protected override void HandlePointerEnter(PointerEventData eventData)
    {
        // 親クラスのHandlePointerEnter()を実行する
        base.HandlePointerEnter(eventData); 

        // テキストが設定されていたら表示
        if (DetailText != null) DetailText.SetActive(true);
    }

    // このスクリプトがついたオブジェクトからマウスが離れたら
    protected override void HandlePointerExit(PointerEventData eventData)
    {
        // 親クラスのHandlePointerExit()を実行する
        base.HandlePointerExit(eventData); 

        // テキストが設定されていたら非表示
        if (DetailText != null) DetailText.SetActive(false);
    }

}
