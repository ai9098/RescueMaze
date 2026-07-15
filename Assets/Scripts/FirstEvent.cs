using UnityEngine;

public class FirstEvent : MonoBehaviour
{
    [SerializeField] private GameObject FirstUI;  // ゲーム説明用のUI

    // SE用変数
    private AudioSource audioSource;
    [SerializeField] private AudioClip ClickSE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // UIを表示させる
        FirstUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // UIが表示中かつ、いずれかのキーが押されたら
        if (FirstUI.activeSelf && Input.anyKeyDown)
        {
            // UIを非表示にする
            FirstUI.SetActive(false);

            // ESを鳴らす
            audioSource.PlayOneShot(ClickSE, 0.3f);
        }
    }

    // 「？」マークが押された時
    public void OnClick()
    {
        // UIが非表示なら
        if (!FirstUI.activeSelf)
        {
            // UIを表示させる
            FirstUI.SetActive(true);

            // ESを鳴らす
            audioSource.PlayOneShot(ClickSE, 0.3f);
        }
    }
}
