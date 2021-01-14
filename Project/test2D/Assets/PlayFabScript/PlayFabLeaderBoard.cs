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
    [SerializeField] int MaxRecordCount= 3;
    private PlayFabAutoRequest m_AutoRequest = null;
    private PlayFabWaitConnect m_WaitConnect = null;
    public List<PlayerLeaderboardEntry> entries { get; private set; } = new List<PlayerLeaderboardEntry>();

    [SerializeField] bool isSelfCount = false;      // 自身から数えたランキングかどうか

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
            if (m_AutoRequest.IsRequest())
            {
                if (!isSelfCount) GetLeaderboard(RankingName, StartPosition, MaxRecordCount);
                else GetLeaderboardAroundSelfPlayer(RankingName, MaxRecordCount);
            }
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
        // 子の全削除
        foreach (Transform n in transform)
        {
            GameObject.Destroy(n.gameObject);
        }
        //result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている
        foreach (var entry in result.Leaderboard)
        {
            entries.Add(entry);

            // PlayFabPlayerDataを人数分取得する
            string objectName = "PlayFabPlayerData" + "Rank" + entry.Position;
            Transform trs = transform.parent.Find(objectName);
            // 該当のゲームオブジェクトが作成されていなかったら作成する
            GameObject obj;
            if (trs == null)
            {
                obj = new GameObject(objectName);
                obj.transform.parent = this.transform;
                obj.AddComponent<PlayFabAutoRequest>();
                var playerData = obj.AddComponent<PlayFabPlayerData>();
                // ID指定をしてランキング内のプレイヤーデータの読み込みをする
                playerData.nominationID = entry.PlayFabId;
            }
            else
            {
                // 作成済みの場合はプレイヤーデータの更新をかける
                PlayFabPlayerData playerData = trs.gameObject.GetComponent<PlayFabPlayerData>();
                playerData.nominationID = entry.PlayFabId;
                // プレイヤーデータの取得要求をする
                playerData.RequestGetUserData();
            }
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


    /// <summary>
    /// 自身周囲のランキング(リーダーボード)を取得
    /// </summary>
    private void GetLeaderboardAroundSelfPlayer(string rankingName, int maxResultsCount)
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(gameObject.name))
        {
            // 通信待ちに設定する
            m_WaitConnect.AddWait(gameObject.name);

            // リストを空にしてから受け取る
            entries.Clear();

            // 子の全削除
            foreach (Transform n in transform)
            {
                GameObject.Destroy(n.gameObject);
            }

            //ランキング(リーダーボード)を取得
            Debug.Log($"自身の周囲のランキング(リーダーボード)の取得開始");
            Debug.Log(PlayFabSettings.staticPlayer.PlayFabId);
            PlayFabClientAPI.GetLeaderboardAroundPlayer( new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = rankingName,            //ランキング名(統計情報名)
                PlayFabId = PlayFabSettings.staticPlayer.PlayFabId,         // PlayFabID
                MaxResultsCount = maxResultsCount       //ランキングデータを何件取得するか(最大100)
            }, result =>
            {
                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name);

                foreach (var entry in result.Leaderboard)
                {
                    entries.Add(entry);
                    Debug.Log("自身の周囲のランキングのID : " + entry.PlayFabId);
                    Debug.Log("自身の周囲のランキングのポジション : " + entry.Position);

                    // PlayFabPlayerDataを人数分取得する
                    string objectName = "PlayFabPlayerData" + "Rank" + entry.Position;
                    Transform trs = transform.parent.Find(objectName);
                    // 該当のゲームオブジェクトが作成されていなかったら作成する
                    if (trs == null)
                    {
                        GameObject obj = new GameObject(objectName);
                        obj.transform.parent = this.transform;
                        obj.AddComponent<PlayFabAutoRequest>();
                        var playerData = obj.AddComponent<PlayFabPlayerData>();
                        // ID指定をしてランキング内のプレイヤーデータの読み込みをする
                        playerData.nominationID = entry.PlayFabId;
                    }
                }
                isGet = true;
            }, error =>
            {
                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name);
                Debug.LogError($"ランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
            });
        }
    }


    /// <summary>
    /// 最大レコード数の取得
    /// </summary>
    /// <returns>レコード数</returns>
    public int GetMaxRecord()
    {
        return MaxRecordCount;
    }

    /// <summary>
    /// ランキング名の取得
    /// </summary>
    /// <returns>ランキング名</returns>
    public string GetRankingName()
    {
        return RankingName;
    }
    /// <summary>
    /// ランキング名の設定
    /// </summary>
    /// <param name="rankingname">ランキング名</param>
    public void SetRankingName(string rankingname)
    {
        RankingName = rankingname;
    }

    /// <summary>
    /// リーダーボードの再取得要求
    /// </summary>
    public void RequestGetLeaderBoard()
    {
        RequestGetLeaderBoard(RankingName);
    }
    /// <summary>
    /// リーダーボードの再取得要求
    /// </summary>
    /// <param name="rankingName">ランキング名</param>
    public void RequestGetLeaderBoard(string rankingName)
    {
        RankingName = rankingName;
        isGet = false;
    }
}
