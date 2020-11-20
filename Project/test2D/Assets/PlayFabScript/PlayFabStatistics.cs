using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayFabStatistics : MonoBehaviour
{
    // 統計情報を取得したかどうか
    public bool isGet{ get; private set;}
    private PlayFabAutoRequest m_AutoRequest = null;
    private PlayFabWaitConnect m_WaitConnect = null;

    // 統計情報リスト
    private List<StatisticValue> m_ValueList;

    void Start()
    {
        GameObject playFabManager = GameObject.Find("PlayFabManager");
        m_ValueList = new List<StatisticValue>();
        isGet = false;
        m_AutoRequest = GetComponent<PlayFabAutoRequest>();
        m_WaitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
    }
    void Update()
    {
        // 自動でPlayFabから統計情報の取得をしておく
        if(!isGet)
        {
            if(m_AutoRequest.IsRequest()) GetPlayerStatistics();
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
            // 通信待ちでなかったら通信開始
            if (!m_WaitConnect.GetWait(transform))
            {
                // 通信待ちに設定する
                m_WaitConnect.SetWait(transform, true);

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
        }
        else
        {
            Debug.Log("統計情報設定に失敗：PlayFabに未ログイン");
        }
    }
    //スコア(統計情報)の更新成功
    private void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        // 通信終了
        m_WaitConnect.SetWait(transform, false);

        Debug.Log($"スコア(統計情報)の更新が成功しました");
    }

    //スコア(統計情報)の更新失敗
    private void OnUpdatePlayerStatisticsFailure(PlayFabError error)
    {
        // 通信終了
        m_WaitConnect.SetWait(transform, false);

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
            // 通信待ちでなかったら通信開始
            if (!m_WaitConnect.GetWait(transform))
            {
                // 通信待ちに設定する
                m_WaitConnect.SetWait(transform, true);

                PlayFabClientAPI.GetPlayerStatistics(
                new GetPlayerStatisticsRequest(),
                OnGetStatistics,
                OnGetErrorStatistics
                );
            }
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

        // 通信終了
        m_WaitConnect.SetWait(transform, false);

        Debug.Log("スコア(統計情報)の取得に成功:");
        foreach (var eachStat in result.Statistics)
        {
            m_ValueList.Add(eachStat);
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
        }

        isGet = true;
    }

    private void OnGetErrorStatistics(PlayFabError obj)
    {
        // 通信終了
        m_WaitConnect.SetWait(transform, false);
        Debug.LogError("統計情報の取得に失敗しました。");
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
