using UnityEngine;

public class RaycastArea : MonoBehaviour
{
    private Transform player; // プレイヤーを格納するための変数
    private EnemyBase enemybase; // 親オブジェクトの敵スクリプトを取得するための変数

    void Start()
    {
        // プレイヤーを取得
        player = GameObject.FindWithTag("Player").transform;
        // 親オブジェクトの敵スクリプトを取得
        enemybase = transform.parent.GetComponent<EnemyBase>();
    }

    private void OnTriggerStay(Collider other)
    {
        // プレイヤーがエリア用のオブジェクトに触れたら
        if (other.CompareTag("Player"))
        {
            // Raycast開始フラグ
            enemybase.isRaycastArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // プレイヤーがエリア用のオブジェクトから離れたら
        if (other.CompareTag("Player"))
        {
            // Raycast停止フラグ
            enemybase.isRaycastArea = false;
        }
    }
}
