using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// タイトル画面の処理
public class StartGame : MonoBehaviour
{
    // SE用変数
    private AudioSource audioSource;
    [SerializeField] private AudioClip SelectSE;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        // spaceキーが押されたら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // SEと、シーン移動のコルーチン
            StartCoroutine(PressKey());

        }

        // Escapeが押されたら
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Finish Game");
            Application.Quit(); // ゲーム終了
        }
    }

    IEnumerator PressKey()
    {
        // SEを鳴らす
        audioSource.PlayOneShot(SelectSE);

        yield return new WaitForSeconds(0.5f);  // 1間秒再生

        // 難易度選択シーンに移動
        SceneManager.LoadScene("Difficulty");
    }
}
