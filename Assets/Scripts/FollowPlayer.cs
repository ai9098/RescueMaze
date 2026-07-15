using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 180f;  // マウス感度
    private Vector3 offset;

    void Start()
    {
        // プレイヤーとカメラの位置を計算
        offset = transform.position - player.position;
    }

    void Update()
    {
        // プレイヤーとカメラの距離を一定に保つ
        transform.position = player.position + offset;

        // マウスの移動量を取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // 回転
        transform.Rotate(Vector3.up * mouseX);
    }
}
