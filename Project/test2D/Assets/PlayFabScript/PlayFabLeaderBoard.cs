using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

// ランキングクラス
public class PlayFabLeaderBoard : MonoBehaviour
{
    public string m_RankingText = default;
    [SerializeField] string RankingName="";
    [SerializeField] int StartPosition = 0;
    [SerializeField] int MaxResultsCount = 3;
    private bool m_Success = false;
    /// <summary>
    /// 問い合わせ間隔
    /// </summary>
    private const float REQ_INTERVAL = 1.0f;
    /// <summary>
    ///  問い合わせ用タイマー
    /// </summary>
    private float m_RequestTimer = 0.0f;

    /// <summary>
    /// リーダーボード取得済みか
    /// </summary>
    public bool success
    {
        get
        { return m_Success; }
    }

    // 開始時にリーダーボードの取得をする
    private void Start()
    {
        m_RequestTimer = REQ_INTERVAL;
    }
    private void Update()
    {
        // リーダーボードは2回以上取得しない
        if (m_Success == false)
        {
            // Playfabにログイン済みかを確認する
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                m_RequestTimer += Time.deltaTime;
                // 問い合わせタイマーを満たしていたら問い合わせる
                if (m_RequestTimer >= REQ_INTERVAL)
                {
                    m_RequestTimer = 0.0f;

                    // リーダーボードの取得に成功するまで続ける
                    GetLeaderboard(RankingName, StartPosition, MaxResultsCount);
                }
            }
            
        }
    }
    /// <summary>
    /// ランキング(リーダーボード)を取得
    /// </summary>
    public void GetLeaderboard(string rankingName, int startPosition, int maxResultsCount)
    {
        //GetLeaderboardRequestのインスタンスを生成
        var request = new GetLeaderboardRequest
        {
            StatisticName = rankingName,            //ランキング名(統計情報名)
            StartPosition = startPosition,          //何位以降のランキングを取得するか
            MaxResultsCount = maxResultsCount       //ランキングデータを何件取得するか(最大100)
        };

        //ランキング(リーダーボード)を取得
        Debug.Log($"ランキング(リーダーボード)の取得開始");
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
    }

    //ランキング(リーダーボード)の取得成功
    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log($"ランキング(リーダーボード)の取得に成功しました");

        //result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている
        m_RankingText = "";
        foreach (var entry in result.Leaderboard)
        {
            m_RankingText += $"\n順位 : {entry.Position+1} スコア : {entry.StatValue} なまえ : {entry.DisplayName}";
        }
        m_Success = true;
    }

    //ランキング(リーダーボード)の取得失敗
    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        Debug.LogError($"ランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }
}
