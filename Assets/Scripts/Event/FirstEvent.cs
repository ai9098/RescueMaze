using UnityEngine;

// ステージ開始時
public class FirstEvent : MonoBehaviour
{
    [SerializeField] private GameObject NomalEnemies;  // Nomalモードの時に追加される敵
    [SerializeField] private GameObject HardEnemies;  // Hardモードの時に追加される敵

    void Start()
    {
        // ログに難易度表示
        Debug.Log("Difficulty: " + GameDifficulty.difficulty);

        // 難易度イージーなら追加なし
        // 難易度ノーマルなら
        if (GameDifficulty.difficulty == 1)
        {
            // ノーマル用の敵を追加
            NomalEnemies.SetActive(true);
        }
        // 難易度ハードなら
        else if (GameDifficulty.difficulty == 2)
        {
            // ノーマル用の敵を追加
            NomalEnemies.SetActive(true);
            // ハード用の敵を追加
            HardEnemies.SetActive(true);
        }
    }
}
