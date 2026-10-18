using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 難易度ごとに、マウスで触れると詳細を表示
public class UIBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Color defaultColor; // 元も色を覚えておく変数
    protected Image touchMouse; // 色を変えるための変数

    protected virtual void Awake()
    {
        // 元の色を保存
        touchMouse = GetComponent<Image>();
        defaultColor = touchMouse.color;
    }

    // このスクリプトがついたオブジェクトにマウスが触れたら
    public void OnPointerEnter(PointerEventData eventData)
    {
        // protected virtualを使用するため
        HandlePointerEnter(eventData);
    }
    protected virtual void HandlePointerEnter(PointerEventData eventData)
    {
        touchMouse.color = Color.gray; // ボタンの色をグレーに変更
    }

    // このスクリプトがついたオブジェクトからマウスが離れたら
    public void OnPointerExit(PointerEventData eventData)
    {
        // protected virtualを使用するため
        HandlePointerExit(eventData);
    }
    protected virtual void HandlePointerExit(PointerEventData eventData)
    {
        touchMouse.color = defaultColor; // ボタンの色を元のに変更
    }

    protected void OnDisable()
    {
        // UIが非表示になったら、強制的に元の色に戻す
        if (touchMouse != null)
        {
            touchMouse.color = defaultColor;
        }
    }
}
