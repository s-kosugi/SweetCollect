using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayFabPlayerData : MonoBehaviour
{
    // 受信した全データ
    public Dictionary<string, UserDataRecord> m_Data { get; private set; } = default;

    private PlayFabLogin m_PlayFabLogin = null;
    private PlayFabAutoRequest m_AutoRequest = null;
    private PlayFabWaitConnect m_WaitConnect = null;
    public bool m_isGet { get; private set; }

    /// <summary>
    /// ID指定してプレイヤーデータの呼び出し
    /// </summary>
    public string nominationID { get; set; } = default;


    public void Start()
    {
        GameObject playFabManager = GameObject.Find("PlayFabManager");
        m_PlayFabLogin = playFabManager.GetComponent<PlayFabLogin>();
        m_WaitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        m_AutoRequest = GetComponent<PlayFabAutoRequest>();
        m_isGet = false;
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
    /// <param name="dataname">データ名</param>
    /// <param name="data">データ内容</param>
    public void SetPlayerData(string dataname,string data)
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(gameObject.name + dataname))
        {
            // 自分のID以外でユーザーデータを更新しようとしていたら止める
            if (nominationID == m_PlayFabLogin._PlayfabID) return;

            // 通信待ちに設定する
            m_WaitConnect.AddWait(gameObject.name + dataname);

            var change = new Dictionary<string, string>
            {
                { dataname, data }
            };
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
            {
                Data = change,Permission = UserDataPermission.Public
            }, result =>
            {
                Debug.Log("ユーザーデータの更新に成功");

                // キーがまだない場合には作成
                if( !m_Data.ContainsKey(dataname) )
                {
                    UserDataRecord record = new UserDataRecord();
                    m_Data.Add(dataname, record);
                }
                m_Data[dataname].Value = data;

                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name + dataname);
            }, error =>
            {
                Debug.Log(error.GenerateErrorReport());

                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name + dataname);
            });
        }
    }

    /// <summary>
    /// ユーザーデータの取得
    /// </summary>
    /// <param name="name">データ名</param>
    private void GetUserData()
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(gameObject.name))
        {
            // 通信待ちに設定する
            m_WaitConnect.AddWait(gameObject.name);

            string ID = m_PlayFabLogin._PlayfabID;

            // ID指定があった場合には指定したIDで取得する
            if (nominationID != default) ID = nominationID;

            Debug.Log("ユーザーデータの取得開始: ユーザーID : "+ID);

            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = ID
            }, result =>
            {
                m_isGet = true;
                m_Data = result.Data;
                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name);

                Debug.Log("ユーザーデータの取得に成功");
            }, error =>
            {
                Debug.Log(error.GenerateErrorReport());
                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name);
            });
        }
    }
}
