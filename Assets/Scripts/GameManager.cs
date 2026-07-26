using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // GameManagerの関数を呼び出せるようにする
    public static GameManager Instance;

    private int totalTarget;
    private int rescuedCount = 0;

    [SerializeField] private TextMeshProUGUI targetNumUI;  // 救出対象の数を表示するUI

    // クリア演出用のUI
    [SerializeField] private GameObject BandUI;
    [SerializeField] private GameObject ClearUI1;
    [SerializeField] private GameObject ClearUI2;
    [SerializeField] private GameObject NextUI;

    // ゲームオーバー演出用のUI
    [SerializeField] private GameObject GameOverBandUI;
    [SerializeField] private GameObject GameOverUI1;
    [SerializeField] private GameObject GameOverUI2;

    public bool LookFirstUI = false;  // 説明UI表示中かどうか
    public bool stageClear = false;   // ステージクリアしたかどうか
    public bool Timer = false;        // タイマーが0になったらステージ再読み込み
    
    private bool canReleoad = true;   // Rキー（ステージリロード）を受け付けるかどうかのフラグ
    private bool stageGameOver = true;   // ゲームオーバー演出は一度だけ

    // SE用変数
    private AudioSource audioSource;
    [SerializeField] private AudioClip ClearSE;
    [SerializeField] private AudioClip GameOverSE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        // ステージ上の救出対象数
        totalTarget = GameObject.FindGameObjectsWithTag("RescueTarget").Length;
    }

    // Update is called once per frame
    void Update()
    {
        // 救出した人数 / 救出する総人数
        targetNumUI.text = "Rescued Target\n" + rescuedCount.ToString() + " / " + totalTarget.ToString();
    
        // ステージクリアした後
        if (stageClear)
        {
            // Spaceキーが押されたら
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadNextScene();
            }
        }

        // Rキーを押してもよい状態でが押されたら、シーンを再読み込み
        if (canReleoad && Input.GetKeyDown(KeyCode.R))
        {
            // 現在のシーンを取得
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;           
            // シーンをリロード
            SceneManager.LoadScene(currentSceneIndex);
        }

        // タイマーが0になったらシーンを再読み込みする演出
        if (stageGameOver && Timer)
        {
            stageGameOver = false;
            StartCoroutine(GameOverUI());
        }
    }

    public void RescueOne()
    {
        // この関数が呼び出されたら、カウントを増やす
        rescuedCount++;

        if (rescuedCount >= totalTarget)
        {
            Debug.Log("ステージクリア");
            stageClear = true;

            // カウントUIは消す
            targetNumUI.gameObject.SetActive(false);

            // クリア演出
            StartCoroutine(ClearUI());
        }
    }

    // クリア時のアニメーション用コルーチン
    IEnumerator ClearUI()
    {
        // UIを表示する
        BandUI.SetActive(true);
        ClearUI1.SetActive(true);
        ClearUI2.SetActive(true);

        // SEを鳴らす
        audioSource.PlayOneShot(ClearSE);

        // 2秒待つ
        yield return new WaitForSeconds(2f);

        // 次のステージを促すUI表示
        NextUI.SetActive(true);

        yield break;
    }

    // ゲームオーバー時のアニメーション用コルーチン
    IEnumerator GameOverUI()
    {
        // 演出が終わるまでRキーが押せないようにする
        canReleoad = false;

        // UIを表示する
        GameOverBandUI.SetActive(true);
        GameOverUI1.SetActive(true);
        GameOverUI2.SetActive(true);

        // SEを鳴らす
        audioSource.PlayOneShot(GameOverSE);

        // 2秒待つ
        yield return new WaitForSeconds(2f);

        // Rキーを押せるようにする
        canReleoad = true;
    }

    private void LoadNextScene()
    {
        // 現在のシーンのインデックスと総シーン数を取得
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        // 現在のシーンのインデックス + 1以上シーンが存在するならば
        if (currentSceneIndex + 1 < totalScenes)
        {
            // 次のシーンをロードする
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else if (currentSceneIndex + 1 >= totalScenes)
        {
            // 総シーン数以上の場合はタイトルに戻る
            SceneManager.LoadScene(0);
        }
    }
}
