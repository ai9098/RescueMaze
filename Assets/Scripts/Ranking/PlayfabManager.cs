using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;

public class PlayfabManager : MonoBehaviour
{
    // スクリプトが動いたとき
    private void OnEnable()
    {      
        PlayFabAuthService.OnLoginSuccess += PlayFabAuthService_OnLoginSuccess; // ログインに成功した時  
        PlayFabAuthService.OnPlayFabError += PlayFabAuthService_OnPlayFabError; // ログインに失敗した時
    }

    // スクリプトが停止した時
    private void OnDisable()
    {
        PlayFabAuthService.OnLoginSuccess -= PlayFabAuthService_OnLoginSuccess; // ログインに成功した時  
        PlayFabAuthService.OnPlayFabError -= PlayFabAuthService_OnPlayFabError; // ログインに失敗した時
    }

    private void PlayFabAuthService_OnLoginSuccess(LoginResult success)
    {
        Debug.Log("ログイン成功");
        // SubmitScore(123);
        GetLeaderboard();
    }

    private void PlayFabAuthService_OnPlayFabError(PlayFabError error)
    {
        Debug.Log("ログイン失敗");
        Debug.LogError(error.GenerateErrorReport());
    }

    void Start()
    {
        // 匿名ログイン
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
    }

    // スコアを登録する関数
    public void SubmitScore(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // StatisticUpdate型のデータを複数まとめて入れられるリストを作成
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore", // 統計情報
                    Value = playerScore // 実際の値
                } 
            }

        }, result =>
        {
            Debug.Log("スコア送信成功");
        }, error =>
        {
            Debug.Log("スコア送信失敗");
        });
    }

    // データを取得する
    public void GetLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,   // 何番目の順位から取ってくるか
            MaxResultsCount = 10 // いくつ取ってくるか
        }, result =>
        {
            foreach (var item in result.Leaderboard)
            {
                string displayName = item.DisplayName;

                // 名前が登録されていなかったら
                if (displayName == null)
                {
                    displayName = "Player";
                }

                Debug.Log($"{item.Position + 1}位:{displayName} " + $"スコア {item.StatValue}");
            }
        }, error =>
        {
            Debug.Log("リーダーボード取得失敗");
        });
    }
}
