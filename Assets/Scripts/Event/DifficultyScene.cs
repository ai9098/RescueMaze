using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// 難易度を設定する
public class DifficultyScene : MonoBehaviour
{
    // SE用変数
    private AudioSource audioSource;
    [SerializeField] private AudioClip SelectSE;

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    // 難易度を設定してゲームを開始する
    public void SetDifficulty(int difficulty)
    {
        // 難易度を設定
        GameDifficulty.difficulty = difficulty;

        // SEと、シーン移動のコルーチン
        StartCoroutine(PressButton());
    }

    // Easyボタンが押されたら
    public void OnClickEasy()
    {
        // Easyに設定
        SetDifficulty(0);
    }

    // Nomalボタンが押されたら
    public void OnClickNomal()
    {
        // Nomalに設定
        SetDifficulty(1);
    }

    // Hardボタンが押されたら
    public void OnClickHard()
    {
        // Hardに設定
        SetDifficulty(2);
    }

    // Backボタンが押されたら
    public void OnClickBack()
    {
        // タイトルシーンに移動
        SceneManager.LoadScene("Title");
    }

    IEnumerator PressButton()
    {
        // SEを鳴らす
        audioSource.PlayOneShot(SelectSE);

        yield return new WaitForSeconds(0.5f);  // 1間秒再生

        // ステージシーンに移動
        SceneManager.LoadScene("Stage1");
    }
}

// シーン間で難易度を共有
public static class GameDifficulty
{
    // 0:Easy 1:Normal 2:Hardを指す
    public static int difficulty = 0;  // バグ防止のため、初期値には0を入れておく
}
