using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayFabPlayerData : MonoBehaviour
{
    [SerializeField] string DataName = "";
    public string m_Value { get; private set; }

    private PlayFabLogin m_PlayFabLogin = null;

    public bool m_isGet { get; private set; }
    /// <summary>
    /// 問い合わせ間隔
    /// </summary>
    private const float REQ_INTERVAL = 1.0f;
    /// <summary>
    ///  問い合わせ用タイマー
    /// </summary>
    private float m_RequestTimer = 0.5f;

    public void Start()
    {
        m_PlayFabLogin = GameObject.Find("PlayFabManager").GetComponent<PlayFabLogin>();
        m_isGet = false;
        m_Value = "";
    }
    public void Update()
    {
        // ユーザー情報は2回以上自動取得しない
        if (!m_isGet)
        {
            // Playfabにログイン済みかを確認する
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                m_RequestTimer += Time.deltaTime;
                // 問い合わせタイマーを満たしていたら問い合わせる
                if (m_RequestTimer >= REQ_INTERVAL)
                {
                    m_RequestTimer = 0.0f;
                    GetUserData();
                }
            }
        }
    }

    /// <summary>
    /// ユーザーデータの更新
    /// </summary>
    /// <param name="data">データ内容</param>
    public void SetPlayerData(string data)
    {
        var change = new Dictionary<string, string>
        {
            { DataName, data }
        };
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = change
        }, result =>
        {
            Debug.Log("ユーザーデータの更新に成功");
            m_Value = data;
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    /// <summary>
    /// ユーザーデータの取得
    /// </summary>
    /// <param name="name">データ名</param>
    public void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = m_PlayFabLogin._PlayfabID
        }, result => {
            Debug.Log(result.Data[DataName].Value);
            m_Value = result.Data[DataName].Value;
            m_isGet = true;
        }, error => {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
