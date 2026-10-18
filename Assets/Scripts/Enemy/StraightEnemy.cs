using UnityEngine;

public class StraightEnemy : EnemyBase
{
    [SerializeField] private float moveSpeed = 3.0f;  // 通常の移動速度

    // 通常時の移動処理を上書き
    protected override void NormalMovement()
    {
        // 直進に戻る
        Vector3 move = transform.forward * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        // 歩行アニメーション開始
        animator.SetBool("Walk", true);
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
}
