using UnityEngine;
using System.Collections;

public class RotateEnemy : EnemyBase
{
    [SerializeField] private float moveAngle = 45f;  // 回転通常の角度
    private Quaternion originalRotation;

    protected override void Start()
    {
        base.Start(); // 親クラスのStart()を実行する

        // ゲーム開始時の角度を保存
        originalRotation = transform.rotation;
        // 回転させるコルーチン開始（一度だけ呼ばれる）
        StartCoroutine(MoveRoutine());
    }

    // 通常時の移動処理を上書き
    protected override void NormalMovement()
    {
        // 見失ったら止める
        rb.linearVelocity = Vector3.zero;
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            // プレイヤーを追いかけていない場合は
            while (!isPlayerSpotted)
            {
                // 指定した角度分回転
                transform.Rotate(new Vector3(0, moveAngle, 0));

                // プレイヤーを見つけたら回転の動きをやめ、追いかける
                if (isPlayerSpotted) break;

                // 3秒間待つ間にプレイヤーを見つけたら、すぐに追いかける
                float timer = 0f;
                while (timer < 3f && !isPlayerSpotted)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                // プレイヤーを見つけたら回転の動きをやめ、追いかける
                if (isPlayerSpotted) break;

                // 元の角度に戻す
                transform.rotation = originalRotation;

                // 3秒間待つ間にプレイヤーを見つけたら、すぐに追いかける
                timer = 0f;
                while (timer < 3f && !isPlayerSpotted)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            // 追跡中は回転の動きを止める
            while (isPlayerSpotted)
            {
                yield return null;
            }
        }
    }
}
