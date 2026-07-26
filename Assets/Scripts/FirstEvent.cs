using UnityEngine;

public class FirstEvent : MonoBehaviour
{
    [SerializeField] private GameObject FirstUI;  // ゲーム説明用のUI
    [SerializeField] private GameObject NomalEnemies;  // Nomalモードの時に追加される敵
    [SerializeField] private GameObject Timer;  // Nomalモードの時に追加されるタイマー

    // SE用変数
    private AudioSource audioSource;
    [SerializeField] private AudioClip ClickSE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // UIを表示させる
        FirstUI.SetActive(true);
        GameManager.Instance.LookFirstUI = true;  // UI表示中フラグオン

        // 難易度表示
        Debug.Log("Difficulty: " + GameDifficulty.difficulty);
        // 難易度ノーマルだったら
        if (GameDifficulty.difficulty == 1)
        {
            // ノーマル用の敵を追加
            NomalEnemies.SetActive(true);

            // タイマー追加
            Timer.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // UIが表示中かつ、いずれかのキーが押されたら
        if (FirstUI.activeSelf && Input.anyKeyDown)
        {
            // UIを非表示にする
            FirstUI.SetActive(false);
            GameManager.Instance.LookFirstUI = false;  // UI表示中フラグオフ

            // ESを鳴らす（0:Easy 1:Normal）
            audioSource.PlayOneShot(ClickSE, 0.3f);
        }
    }

    // 「？」マークが押された時
    public void OnClick()
    {
        Debug.Log("押されたよ");
        // UIが非表示なら
        if (!FirstUI.activeSelf)
        {
            // UIを表示させる
            FirstUI.SetActive(true);
            GameManager.Instance.LookFirstUI = true;  // UI表示中フラグオン

            // ESを鳴らす
            audioSource.PlayOneShot(ClickSE, 0.3f);
        }
    }
}
