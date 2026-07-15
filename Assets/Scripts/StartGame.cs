using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
            // ゲーム終了
            Application.Quit();
            Debug.Log("Finish Game");
        }
    }

    IEnumerator PressKey()
    {
        // SEを鳴らす
        audioSource.PlayOneShot(SelectSE);

        yield return new WaitForSeconds(0.5f);  // 1間秒再生

        // シーン１に移動
        SceneManager.LoadScene(1);
    }
}
