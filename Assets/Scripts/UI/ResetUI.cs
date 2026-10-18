using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetUI : UIBase
{
    [SerializeField] private GameObject ParentUI;

    protected override void Awake()
    {
        base.Awake(); // 親クラスのAwake()

        // 親オブジェクトを取得
        ParentUI = transform.parent.gameObject;
    }

    // Yesボタンが押されたら
    public void OnClickYes()
    {
        // 現在のシーンを取得
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // シーンをリロード
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Noボタンが押されたら
    public void OnClickNo()
    {
        if (ParentUI != null)
        {
            // UIを閉じる
            ParentUI.gameObject.SetActive(false);
        }
    }
}
