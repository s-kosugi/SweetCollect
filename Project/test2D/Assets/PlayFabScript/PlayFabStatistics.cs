using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

/// <summary>
/// PlayFab統計情報クラス
/// </summary>
public class PlayFabStatistics : MonoBehaviour
{
    
    /// <summary>
    /// 統計情報を取得したかどうか
    /// </summary>
    public bool isGet{ get; private set;}

    private PlayFabAutoRequest autoRequest = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;

    // 統計情報リスト
    private List<StatisticValue> valueList;

    void Start()
    {
        valueList = new List<StatisticValue>();
        isGet = false;
        autoRequest = GetComponent<PlayFabAutoRequest>();
        if(waitConnect == default)
        {
            GameObject playFabManager = GameObject.Find("PlayFabManager");
            waitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        }
    }
    void Update()
    {
        // 自動でPlayFabから統計情報の取得をしておく
        if(!isGet)
        {
            if(autoRequest.IsRequest()) GetPlayerStatistics();
        }
    }

    /// <summary>
    /// 統計情報を更新する
    /// </summary>
    /// <param name="rankingName">ランキング名</param>
    /// <param name="value">更新する値</param>
    public void UpdatePlayerStatistics(string rankingName, int value)
    {
        // Playfabにログイン済みかを確認する
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            // 通信待ちでなかったら通信開始
            if (!waitConnect.GetWait(gameObject.name))
            {
                // 通信待ちに設定する
                waitConnect.AddWait(gameObject.name);

                // UpdatePlayerStatisticsRequestのインスタンスを生成
                var request = new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>{
                        new StatisticUpdate{
                        StatisticName = rankingName,   //ランキング名(統計情報名)
                        Value = value, // スコア(int)
                        }
                    }
                };
                Debug.Log($"統計情報名:" + rankingName + " Value:" + value);

                // スコア情報の更新
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
    
    /// <summary>
    /// 統計情報の更新成功
    /// </summary>
    /// <param name="result">更新結果</param>
    private void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        // 通信終了
        waitConnect.RemoveWait(gameObject.name);

        Debug.Log($"スコア(統計情報)の更新が成功しました");
    }

    /// <summary>
    /// 統計情報の更新失敗
    /// </summary>
    /// <param name="error">エラー内容</param>
    private void OnUpdatePlayerStatisticsFailure(PlayFabError error)
    {
        // 通信終了
        waitConnect.RemoveWait(gameObject.name);

        Debug.LogError($"スコア(統計情報)更新に失敗しました\n{error.GenerateErrorReport()}");
    }

    /// <summary>
    /// 統計情報を取得する
    /// </summary>
    public void GetPlayerStatistics()
    {
        // Playfabにログイン済みかを確認する
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            // 通信待ちでなかったら通信開始
            if (!waitConnect.GetWait(gameObject.name))
            {
                // 通信待ちに設定する
                waitConnect.AddWait(gameObject.name);

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
        valueList.Clear();

        // 通信終了
        waitConnect.RemoveWait(gameObject.name);

        Debug.Log("スコア(統計情報)の取得に成功:");
        foreach (var eachStat in result.Statistics)
        {
            valueList.Add(eachStat);
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
        }

        isGet = true;
    }

    /// <summary>
    /// 統計情報の取得に失敗
    /// </summary>
    /// <param name="error">エラー内容</param>
    private void OnGetErrorStatistics(PlayFabError error)
    {
        // 通信終了
        waitConnect.RemoveWait(gameObject.name);
        Debug.LogError("統計情報の取得に失敗しました。");
    }

    /// <summary>
    /// クラス内に保存されている統計情報の取得
    /// </summary>
    /// <param name="StatisticName">統計情報名</param>
    /// <returns>統計情報名が見つからない場合は0を返す</returns>
    public int GetStatisticValue(string StatisticName)
    {
        StatisticValue staValue = valueList.Find(n => n.StatisticName == StatisticName);
        if (staValue == null) return 0;
        return staValue.Value;

    }
}
