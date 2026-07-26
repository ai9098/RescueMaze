using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DifficultyScene : MonoBehaviour
{
    // SE用変数
    private AudioSource audioSource;
    [SerializeField] private AudioClip SelectSE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Easyボタンが押されたら
    public void OnClickEasy()
    {
        // Easyに設定
        GameDifficulty.difficulty = 0;
        Debug.Log("difficulty : Easy");

        // SEと、シーン移動のコルーチン
        StartCoroutine(PressButton());
    }

    // Nomalボタンが押されたら
    public void OnClickNomal()
    {
        // Nomalに設定
        GameDifficulty.difficulty = 1;
        Debug.Log("difficulty : Nomal");

        // SEと、シーン移動のコルーチン
        StartCoroutine(PressButton());
    }

    // Hardボタンが押されたら（制作予定）
    //public void OnClickDifficult()
    //{
    //    // Hardに設定
    //    GameDifficulty.difficulty = 1;
    //    Debug.Log("difficulty : Hard");

    //    // SEと、シーン移動のコルーチン
    //    StartCoroutine(PressButton());
    //}

    // Backボタンが押されたら
    public void OnClickBack()
    {
        // シーン０（タイトル画面）に移動
        SceneManager.LoadScene(0);
    }

    IEnumerator PressButton()
    {
        // SEを鳴らす
        audioSource.PlayOneShot(SelectSE);

        yield return new WaitForSeconds(0.5f);  // 1間秒再生

        // シーン２（ゲーム画面）に移動
        SceneManager.LoadScene(2);
    }
}

// 難易度を保存しておく
public static class GameDifficulty
{
    // 0:Easy 1:Normalを指す
    public static int difficulty = 0;  // 初期値には0を入れておく
}
