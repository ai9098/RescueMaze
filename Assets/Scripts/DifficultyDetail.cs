using UnityEngine;
using UnityEngine.EventSystems;

public class DifficultyDetail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 表示したいテキスト
    [SerializeField] private GameObject DetailText;

    // このスクリプトがついたオブジェクトにマウスが触れたら
    public void OnPointerEnter(PointerEventData eventData)
    {
        // テキストが設定されていたら表示
        if (DetailText != null) DetailText.SetActive(true);
    }

    // このスクリプトがついたオブジェクトからマウスが離れたら
    public void OnPointerExit(PointerEventData eventData)
    {
        // テキストが設定されていたら非表示
        if (DetailText != null) DetailText.SetActive(false);
    }

}
