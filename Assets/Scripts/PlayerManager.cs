using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float moveSpeed = 3.0f;

    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject subCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 初めはサブカメラをオフに
        subCamera.SetActive(false);
    }

    void Update()
    {
        // Tabキーが押されたらカメラを切り替える
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerCamera.SetActive(!playerCamera.activeSelf);
            subCamera.SetActive(!subCamera.activeSelf);
        }
    }

    void FixedUpdate()
    {
        // すでにステージをクリアしていたら動けなくする
        if (GameManager.Instance.stageClear) return;

        // プレイヤーカメラ出なければストップ
        if (!playerCamera.activeSelf) return;

        // 入力を処理
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // カメラの前方向を取得(Y成分を消し、長さを1に)
        Vector3 cameraForward 
            = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        
        // 移動方向を計算(斜め移動にも対応)
        Vector3 moveDirection 
            = cameraForward * vertical + Camera.main.transform.right * horizontal;

        // 移動処理
        rb.linearVelocity
            = moveDirection * moveSpeed + new Vector3(0, rb.linearVelocity.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RescueTarget"))
        {
            Debug.Log("救出しました。");
        }
    }
}
