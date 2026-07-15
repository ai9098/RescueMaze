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

    public bool stageClear = false;  // ステージクリアしたかどうか

    // SE用変数
    private AudioSource audioSource;
    [SerializeField] private AudioClip ClearSE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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

        // Rキーが押されたらカメラを切り替える
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 現在のシーンを取得
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            // シーンをリロード
            SceneManager.LoadScene(currentSceneIndex);
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

        // 3秒待つ
        yield return new WaitForSeconds(2f);

        // 次のステージを促すUI表示
        NextUI.SetActive(true);

        yield break;
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
