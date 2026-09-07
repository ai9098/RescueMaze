using System.Collections;
using TMPro;
using UnityEngine;

public class RescueTarget : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 3.0f;
    [SerializeField] Vector3 offset;

    private Vector3 worldPos; // ゲーム開始時の座標を格納する

    [Header("参照オブジェクト")] 
    [SerializeField] GameObject ResetLight;
    [SerializeField] GameObject CheckMark;
    public Transform player; // プレイヤーの位置を格納する変数
    public Transform playerCamera; // プレイヤーの一人称カメラ
    private TextMeshProUGUI targetLostUI;  // はぐれたときに表示するUI
    private Canvas canvas; // シーン内のCanvasを格納

    [Header("サウンド設定")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip RescueSE;
    [SerializeField] private AudioClip EnemyAtackSE;

    private bool followPlayer = false;  // Playerについていく状態かどうか
    private bool clear = false;  // 救出済みかどうか

    void Start()
    {
        // プレイヤーの位置を取得
        player = GameObject.FindWithTag("Player").transform;
        // プレイヤーカメラの位置を取得
        playerCamera = GameObject.FindWithTag("MainCamera").transform;

        // シーン内のCanvasを検索
        canvas = FindFirstObjectByType<Canvas>();

        if (canvas != null) {
            // シーン内のTargetLostを探す
            targetLostUI = canvas.transform.Find("TargetLost").GetComponent<TextMeshProUGUI>();
        }

        // ゲーム開始したタイミングの座標を取得する
        worldPos = transform.position;

        audioSource = GetComponent<AudioSource>();  
    }

    void Update()
    {
        // Playerについていく状態でなければreturn
        if (!followPlayer) return;

        // プレイヤーの1.5m後ろをついていく(向きはカメラの正面の逆側に)
        Vector3 targetPos = player.position - playerCamera.forward * 1.5f;

        // 少しずつ近づく(offset分ずらす)
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos + offset,
            MoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーに触れたらついていく
        if (other.CompareTag("Player"))
        {
            // 救出前であれば追跡開始
            if (!clear)
            {
                // 仲間になった時のみ
                if (!followPlayer)
                {
                    // ESを鳴らす
                    audioSource.PlayOneShot(RescueSE);
                }

                followPlayer = true;
            }
        }

        // プレイヤー追跡中に敵に触れられたら、初期位置に戻る
        if (followPlayer && other.CompareTag("HitDetection"))
        {
            Debug.Log("敵に触れられました");
            transform.position = worldPos;

            // ESを鳴らす
            audioSource.PlayOneShot(EnemyAtackSE);

            // コルーチン開始
            StartCoroutine(LostUI());

            // 追跡中止
            followPlayer = false;
        }

        // クリアエリアに到達したら
        if (other.CompareTag("ClearFloor"))
        {
            Debug.Log("救出成功");

            // 追跡中止
            followPlayer = false;
            // 救出成功後は追跡しない
            clear = true;

            // 救出完了のチェックマークを表示
            CheckMark.SetActive(true);

            // ESを鳴らす
            audioSource.PlayOneShot(RescueSE);

            // 助けた人数のカウントを増やす
            GameManager.Instance.RescueOne();
        }
    }

    IEnumerator LostUI()
    {
        // UIを表示する
        targetLostUI.gameObject.SetActive(true);
        // 初期位置を光らせる
        ResetLight.SetActive(true);

        yield return new WaitForSeconds(3f);  // 3間秒表示

        // UIを非表示にする
        targetLostUI.gameObject.SetActive(false);
        // 初期位置を光らせるのをやめる
        ResetLight.SetActive(false);

        yield break;
    }
}