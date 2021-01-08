using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

// ランキングクラス
public class PlayFabLeaderBoard : MonoBehaviour
{
    [SerializeField] string RankingName="";
    [SerializeField] int StartPosition = 0;
    [SerializeField] int MaxResultsCount = 3;
    private PlayFabAutoRequest m_AutoRequest = null;
    private PlayFabWaitConnect m_WaitConnect = null;
    public List<PlayerLeaderboardEntry> entries { get; private set; } = new List<PlayerLeaderboardEntry>();

    /// <summary>
    /// リーダーボード取得済みか
    /// </summary>
    public bool isGet { get; private set; }
    private void Start()
    {
        GameObject playFabManager = transform.parent.gameObject;
        m_AutoRequest = GetComponent<PlayFabAutoRequest>();
        m_WaitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
    }

    private void Update()
    {
        // リーダーボードは2回以上取得しない
        if (isGet == false)
        {
            // リーダーボードの取得に成功するまで続ける
            if(m_AutoRequest.IsRequest()) GetLeaderboard(RankingName, StartPosition, MaxResultsCount);
        }
    }
    /// <summary>
    /// ランキング(リーダーボード)を取得
    /// </summary>
    private void GetLeaderboard(string rankingName, int startPosition, int maxResultsCount)
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait( gameObject.name))
        {
            // 通信待ちに設定する
            m_WaitConnect.AddWait(gameObject.name);

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
    }

    //ランキング(リーダーボード)の取得成功
    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log($"ランキング(リーダーボード)の取得に成功しました");

        // 通信終了
        m_WaitConnect.RemoveWait(gameObject.name);

        // リストを空にしてから受け取る
        entries.Clear();
        //result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている
        foreach (var entry in result.Leaderboard)
        {
            entries.Add(entry);
            //m_RankingText += $"\n順位 : {entry.Position+1} スコア : {entry.StatValue} なまえ : {entry.DisplayName}";
        }
        isGet = true;
    }

    //ランキング(リーダーボード)の取得失敗
    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        // 通信終了
        m_WaitConnect.RemoveWait(gameObject.name);

        Debug.LogError($"ランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }
}
