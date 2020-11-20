using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayFabPlayerData : MonoBehaviour
{
    [SerializeField] string DataName = "";
    public string m_Value { get; private set; }

    private PlayFabLogin m_PlayFabLogin = null;
    private PlayFabAutoRequest m_AutoRequest = null;
    private PlayFabWaitConnect m_WaitConnect = null;
    public bool m_isGet { get; private set; }

    public void Start()
    {
        GameObject playFabManager = GameObject.Find("PlayFabManager");
        m_PlayFabLogin = playFabManager.GetComponent<PlayFabLogin>();
        m_WaitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        m_AutoRequest = GetComponent<PlayFabAutoRequest>();
        m_isGet = false;
        m_Value = "";
    }
    public void Update()
    {
        // ユーザー情報は2回以上自動取得しない
        if (!m_isGet)
        {
            if (m_AutoRequest.IsRequest()) GetUserData();
        }
    }

    /// <summary>
    /// ユーザーデータの更新
    /// </summary>
    /// <param name="data">データ内容</param>
    public void SetPlayerData(string data)
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(transform))
        {
            // 通信待ちに設定する
            m_WaitConnect.SetWait(transform, true);

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

            // 通信終了
            m_WaitConnect.SetWait(transform, false);
            }, error =>
            {
                Debug.Log(error.GenerateErrorReport());

            // 通信終了
            m_WaitConnect.SetWait(transform, false);
            });
        }
    }

    /// <summary>
    /// ユーザーデータの取得
    /// </summary>
    /// <param name="name">データ名</param>
    public void GetUserData()
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(transform))
        {
            // 通信待ちに設定する
            m_WaitConnect.SetWait(transform, true);

            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = m_PlayFabLogin._PlayfabID
            }, result =>
            {
                Debug.Log(result.Data[DataName].Value);
                m_Value = result.Data[DataName].Value;
                m_isGet = true;
                // 通信終了
                m_WaitConnect.SetWait(transform, false);
            }, error =>
            {
                Debug.Log(error.GenerateErrorReport());
                // 通信終了
                m_WaitConnect.SetWait(transform, false);
            });
        }
    }
}
