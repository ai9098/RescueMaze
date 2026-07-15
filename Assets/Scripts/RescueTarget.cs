using System.Collections;
using TMPro;
using UnityEngine;

public class RescueTarget : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float MoveSpeed = 3.0f;
    [SerializeField] Vector3 offset;
    private bool followPlayer = false;  // Playerについていく状態かどうか
    private bool clear = false;  // 救出済みかどうか

    private Vector3 worldPos;

    [SerializeField] private TextMeshProUGUI targetLostUI;  // はぐれたときに表示するUI
    [SerializeField] GameObject ResetLight;
    [SerializeField] GameObject CheckMark;

    // SE用変数
    private AudioSource audioSource;
    [SerializeField] private AudioClip RescueSE;
    [SerializeField] private AudioClip EnemyAtackSE;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // ゲーム開始したタイミングの座標を取得する
        worldPos = transform.position;
    }

    void Update()
    {
        // Playerについていく状態でなければreturn
        if (!followPlayer) return;

        // プレイヤーの1.5m後ろをついていく(向きはカメラの正面の逆側に)
        Vector3 targetPos = player.position - cameraTransform.forward * 1.5f;

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