using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabStatistics : MonoBehaviour
{
    // 統計情報を取得したかどうか
    private bool m_isGet = false;
    public bool isGet
    {
        get {return m_isGet;}
    }

    // 統計情報リスト
    private List<StatisticValue> m_ValueList;

    /// <summary>
    /// 問い合わせ間隔
    /// </summary>
    private const float REQ_INTERVAL = 1.0f;
    /// <summary>
    ///  問い合わせ用タイマー
    /// </summary>
    private float m_RequestTimer = 0.0f;

    void Start()
    {
        m_ValueList = new List<StatisticValue>();
        m_isGet = false;
        m_RequestTimer = 0.0f;
    }
    void Update()
    {
        // 自動でPlayFabから統計情報の取得をしておく
        if(!m_isGet)
        {
            m_RequestTimer += Time.deltaTime;
            // 問い合わせタイマーを満たしていたら問い合わせる
            if (m_RequestTimer >= REQ_INTERVAL)
            {
                GetPlayerStatistics();
                m_RequestTimer = 0;
            }
        }
    }

    /// <summary>
    /// スコア(統計情報)を更新する
    /// </summary>
    public void UpdatePlayerStatistics(string rankingName, int score)
    {
        // Playfabにログイン済みかを確認する
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            //UpdatePlayerStatisticsRequestのインスタンスを生成
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>{
                new StatisticUpdate{
                StatisticName = rankingName,   //ランキング名(統計情報名)
                Value = score, //スコア(int)
                }
            }
            };
            Debug.Log($"統計情報名:" + rankingName + " Value:" + score);

            //スコア情報の更新
            Debug.Log($"スコア(統計情報)の更新開始");
            PlayFabClientAPI.UpdatePlayerStatistics(request,
                OnUpdatePlayerStatisticsSuccess,
                OnUpdatePlayerStatisticsFailure);
        }
        else
        {
            Debug.Log("統計情報設定に失敗：PlayFabに未ログイン");
        }
    }
    //スコア(統計情報)の更新成功
    private void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log($"スコア(統計情報)の更新が成功しました");
    }

    //スコア(統計情報)の更新失敗
    private void OnUpdatePlayerStatisticsFailure(PlayFabError error)
    {
        Debug.LogError($"スコア(統計情報)更新に失敗しました\n{error.GenerateErrorReport()}");
    }

    /// <summary>
    /// スコア(統計情報)を取得する
    /// </summary>
    public void GetPlayerStatistics()
    {
        // Playfabにログイン済みかを確認する
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
                PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
            );
        }
        else 
        {
            Debug.Log("統計情報取得に失敗：PlayFabに未ログイン");
        }
    }
    /// <summary>
    /// 統計情報の取得に成功
    /// </summary>
    /// <param name="result"></param>
    private void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        m_ValueList.Clear();


        Debug.Log("スコア(統計情報)の取得に成功:");
        foreach (var eachStat in result.Statistics)
        {
            m_ValueList.Add(eachStat);
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
        }

        m_isGet = true;
    }

    /// <summary>
    /// クラス内に保存されている統計情報の取得
    /// </summary>
    /// <param name="StatisticName">統計情報名</param>
    /// <returns>統計情報名が見つからない場合は0を返す</returns>
    public int GetStatisticValue(string StatisticName)
    {
        StatisticValue staValue = m_ValueList.Find(n => n.StatisticName == StatisticName);
        if (staValue == null) return 0;
        return staValue.Value;

    }
}
