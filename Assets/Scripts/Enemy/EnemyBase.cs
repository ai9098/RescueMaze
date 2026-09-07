using System.Collections;
using UnityEngine;

// 基底クラス
public class EnemyBase : MonoBehaviour
{
    [Header("視界・追跡設定")]
    [SerializeField] protected float followSpeed = 1.0f;  // 追跡時の移動速度
    [SerializeField] protected float angle = 45f;  // 視界の角度
    [SerializeField] protected float viewDistance = 4f;  // 視界距離
    public int segments = 20;  // 可視化された視野を作る三角形の数

    [Header("参照オブジェクト")] 
    [SerializeField] protected MeshFilter viewMeshFilter;
    [SerializeField] protected GameObject exclamationMark;
    protected Transform player; // プレイヤーの位置を格納する変数

    [Header("サウンド設定")]
    [SerializeField] protected AudioClip EnemyFollowSE;
    protected AudioSource audioSource;

    // 内部変数
    protected Rigidbody rb;
    protected bool isPlayerSpotted = false;  // プレイヤーを追いかけているかどうか
    protected bool isPlaySound = false; // 音が鳴っているかどうか
    protected Color defaultColor;  // ゲーム開始時の元も色を覚えておく変数
    protected Renderer enemyColor;  // 敵の色を変えるための変数

    protected virtual void Start()
    {
        // プレイヤーの位置を取得
        player = GameObject.FindWithTag("Player").transform;

        rb = GetComponent<Rigidbody>();
        enemyColor = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        if (enemyColor != null)
        {
            // ゲーム開始時の色を保存
            defaultColor = enemyColor.material.color;
        }

        // 視界の可視化
        CreateConeMesh();
    }

    protected virtual void FixedUpdate()
    {
        // プレイヤーを追いかけている状態であれば
        if (isPlayerSpotted && player != null)
        {
            // プレイヤーとの方向ベクトルを計算し、長さを1にする
            Vector3 direction = (player.position - transform.position).normalized;
            // y軸のめり込み防止
            direction.y = 0;

            // Rigidbodyで追いかける
            rb.linearVelocity = direction * followSpeed;

            // 追跡時は色を変更
            if (enemyColor != null) enemyColor.material.color = Color.red;  // 赤色に変更
            if (exclamationMark != null) exclamationMark.SetActive(true);  // 「！」マーク表示
        }
        else
        {
            // 発見されていない時の処理（個別の通常移動を呼び出す）
            NormalMovement();

            if (enemyColor != null) enemyColor.material.color = defaultColor;  // 色を元に戻す
            if (exclamationMark != null) exclamationMark.SetActive(false);  // 「！」マーク非表示

            //  見失ったら、音フラグをfalseに戻す
            isPlaySound = false;

            if (audioSource != null && audioSource.isPlaying)
            {
                // ESを止める
                audioSource.Stop();
            }
        }
        // 毎フレームリセット（OnTriggerStayで毎フレーム判定されるため）
        isPlayerSpotted = false;
    }

    // 子クラスで固有の移動処理を書くための仮想メソッド
    protected virtual void NormalMovement()
    {

    }

    // 触れている間、ずっと毎フレーム処理する
    private void OnTriggerStay(Collider other)
    {
        // 視界にPlayerが入ったら
        if (other.CompareTag("Player"))
        {
            // プレイヤーの位置 - 敵の位置により、敵からPlayerへ向かう方向ベクトルを計算
            Vector3 posDelta = other.transform.position - this.transform.position;

            // 敵の正面方向とPlayer方向の角度差
            float target_angle = Vector3.Angle(this.transform.forward, posDelta);

            // 視野角内か確認であれば
            if (target_angle < angle / 1.5f)
            {
                // Rayを飛ばして何かに当たったら、その情報を hit に入れる
                if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit))
                {
                    // 壁ではなくPlayerを視界にとらえたら
                    if (hit.collider == other)
                    {
                        Debug.Log("プレイヤー発見");

                        if (!isPlaySound)
                        {
                            // SEをクリップを登録する
                            audioSource.clip = EnemyFollowSE;

                            audioSource.Play();
                            isPlaySound = true;   // 鳴らしたフラグ
                        }

                        // プレイヤーの追跡開始フラグ
                        isPlayerSpotted = true;

                    }
                }
            }
        }
    }

    // 視界を作成
    void CreateConeMesh()
    {
        // メッシュ作成
        Mesh mesh = new Mesh();

        // 頂点配列を作る
        Vector3[] vertices = new Vector3[segments + 2];
        // 三角形の頂点数なので*3する
        int[] triangles = new int[segments * 3];

        // 中心座標
        vertices[0] = Vector3.zero;
        // 左端の角度
        float startAngle = -angle / 2;

        // セグメント数に合わせた外周を作る
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startAngle + (angle * i / segments);
            float rad = currentAngle * Mathf.Deg2Rad;  // ラジアンに変換

            // 座標を求める
            vertices[i + 1] = new Vector3(
                Mathf.Sin(rad) * viewDistance,
                0,
                Mathf.Cos(rad) * viewDistance
            );
        }

        // 三角形を作る
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        // Meshに頂点を設定
        mesh.vertices = vertices;
        // 三角形を設定
        mesh.triangles = triangles;
        // 法線を設定
        mesh.RecalculateNormals();

        // 子オブジェクトのMeshFilterに形をセットする
        viewMeshFilter.mesh = mesh;
    }
}