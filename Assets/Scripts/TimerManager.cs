using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private float CountTime;  // 何分何秒から始めるか

    private float timer;

    private void Start()
    {
        // 初めに時間を設定
        timer = CountTime; 
    }

    void Update()
    {
        // UI表示中ならタイマーを進めない
        if (GameManager.Instance.LookFirstUI) return;

        // 0秒になったらフラグをオンにする
        if (timer < 0) GameManager.Instance.Timer = true;

        timer -= Time.deltaTime;  // 時間を経過せせる（蓄積）
        int minutes = Mathf.FloorToInt(timer / 60);  // 分
        int seconds = Mathf.FloorToInt(timer % 60);  // 秒

        uiFill.fillAmount = Mathf.InverseLerp(0, CountTime, timer);  // CountTimeから0にtimerずつ減る
        uiText.text = minutes.ToString("00") + ":" + seconds.ToString("00");  // それぞれ二桁で表示
    }
}
