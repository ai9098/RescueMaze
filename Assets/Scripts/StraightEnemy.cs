using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class StraightEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;  // 通常の移動速度
    [SerializeField] private float followSpeed = 1.0f;  // 追跡時の移動速度

    [SerializeField] private float angle = 45f;  // 視界の角度
    private Rigidbody rb;

    [SerializeField] private float viewDistance = 4f;  // 視界距離
    public int segments = 20;  // 可視化された視野を作る三角形の数

    [SerializeField] private Transform Player;
    [SerializeField] private MeshFilter viewMeshFilter;
    [SerializeField] private GameObject exclamationMark;

    private bool isPlayerSpotted = false;  // プレイヤーを追いかけているかどうか
    private bool isPlaySound = false; // 音が鳴っているかどうか

    private Color defaultColor;  // ゲーム開始時の元も色を覚えておく変数
    private Renderer enemyColor;  // 敵の色を変えるための変数

    // SE用変数
    private AudioSource audioSource;
    [SerializeField] private AudioClip EnemyFollowSE;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyColor = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        // ゲーム開始時の色を保存
        defaultColor = enemyColor.material.color;

        // 視界の可視化
        CreateConeMesh();
    }

    void FixedUpdate()
    {
        Vector3 move = transform.forward * moveSpeed;

        // 真っすぐに進む
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    
        if (isPlayerSpotted)
        {
            // プレイヤーの座標を取得
            Vector3 playerPos = Player.position;

            // プレイヤーの方向ベクトルを計算し、長さを1にする
            Vector3 direction = (playerPos - transform.position).normalized;
            // y軸のめり込み防止
            direction.y = 0;

            // Rigidbodyで追いかける
            rb.linearVelocity = direction * followSpeed;

            // 追跡時は色を変更
            enemyColor.material.color = Color.red;  // 赤色に変更
            exclamationMark.SetActive(true);  // 「！」マーク表示
        }
        else
        {
            enemyColor.material.color = defaultColor;  // 色を元に戻す
            exclamationMark.SetActive(false);  // 「！」マーク非表示

            //  見失ったら、音フラグをfalseに戻す
            isPlaySound = false;

            // ESを止める
            audioSource.Stop();
        }
        // 見失ったら追跡終了
        isPlayerSpotted = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 壁にぶつかったら反対方向に
        if (collision.gameObject.CompareTag("Wall"))
        {
            // y軸回転
            transform.Rotate(0, 180, 0);
        }
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
            if (target_angle < angle)
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
