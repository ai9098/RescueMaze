using UnityEngine;

public class PopupPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject targetUI;  // ゲーム説明用のUI
    [SerializeField] private bool openOnStart = false; // スタート時に自動で開くか
    [SerializeField] private bool closeButton = false; // 閉じる用のボタンがあるか

    // SE用変数
    private AudioSource audioSource;
    [SerializeField] private AudioClip ClickSE;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // スタート時に自動で開く必要がある場合は、UIを表示
        if (openOnStart && targetUI != null)
        {
            OpenUI();
        }
    }

    private void Update()
    {
        // 閉じる用のボタンが用意されているならreturn
        if (closeButton) return;

        // UIが表示中かつ、いずれかのキーが押されたらUIを閉じる
        if (targetUI != null && targetUI.activeSelf && Input.anyKeyDown)
        {
            CloseUI();
        }
    }

    // UIを表示する（ボタンから操作するため関数化）
    public void OpenUI()
    {
        // 表示中であればreturn
        if (targetUI == null || targetUI.activeSelf) return;

        // UI表示
        targetUI.SetActive(true);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LookFirstUI = true;  // UI表示中フラグオン
        }

        // SE再生
        PlaySE();
    }

    // UIを閉じる
    public void CloseUI()
    {
        // 非表示中であればreturn
        if (targetUI == null || !targetUI.activeSelf) return;

        // UI非表示
        targetUI.SetActive(false);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LookFirstUI = false;  // UI表示中フラグオン
        }

        // SE再生
        PlaySE();
    }

    private void PlaySE()
    {
        if (ClickSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(ClickSE, 0.3f);
        }
    }
}
