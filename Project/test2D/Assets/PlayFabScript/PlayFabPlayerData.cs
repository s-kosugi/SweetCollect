using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

/// <summary>
/// PlayFabプレイヤーデータクラス
/// </summary>
public class PlayFabPlayerData : MonoBehaviour
{
    /// <summary>
    /// 受信した全データ
    /// </summary>
    public Dictionary<string, UserDataRecord> data { get; private set; } = default;

    [SerializeField] PlayFabLogin playFabLogin = default;
    private PlayFabAutoRequest autoRequest = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    public bool isGet { get; private set; }

    /// <summary>
    /// ID指定してプレイヤーデータの呼び出し
    /// </summary>
    public string nominationID { get; set; } = default;


    void Start()
    {
        autoRequest = GetComponent<PlayFabAutoRequest>();
        isGet = false;

        if (playFabLogin == default || waitConnect == default)
        {
            GameObject playFabManager = GameObject.Find("PlayFabManager");
            playFabLogin = playFabManager.GetComponent<PlayFabLogin>();
            waitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        }
    }
    void Update()
    {
        // ユーザー情報は2回以上自動取得しない
        if (!isGet)
        {
            if (autoRequest.IsRequest()) GetData();
        }
    }

    /// <summary>
    /// プレイヤーデータの更新
    /// </summary>
    /// <param name="dataname">データ名</param>
    /// <param name="data">データ内容</param>
    public void SetPlayerData(string dataname,string data)
    {
        // 通信待ちでなかったら通信開始
        if (!waitConnect.GetWait(gameObject.name + dataname))
        {
            // 自分のID以外でユーザーデータを更新しようとしていたら止める
            if (nominationID != playFabLogin._PlayfabID && nominationID != default) return;

            // 通信待ちに設定する
            waitConnect.AddWait(gameObject.name + dataname);

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
                if( !this.data.ContainsKey(dataname) )
                {
                    UserDataRecord record = new UserDataRecord();
                    this.data.Add(dataname, record);
                }
                this.data[dataname].Value = data;

                // 通信終了
                waitConnect.RemoveWait(gameObject.name + dataname);
            }, error =>
            {
                Debug.Log(error.GenerateErrorReport());

                // 通信終了
                waitConnect.RemoveWait(gameObject.name + dataname);
            });
        }
    }

    /// <summary>
    /// プレイヤーデータの取得
    /// </summary>
    private void GetData()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            // 通信待ちでなかったら通信開始(インスタンスIDを付与して別オブジェクトから同時に取得できるようにする)
            if (!waitConnect.GetWait(gameObject.name+gameObject.GetInstanceID()))
            {
                // 通信待ちに設定する
                waitConnect.AddWait(gameObject.name + gameObject.GetInstanceID());

                string ID = playFabLogin._PlayfabID;

                // ID指定があった場合には指定したIDで取得する
                if (nominationID != default) ID = nominationID;

                Debug.Log("ユーザーデータの取得開始: ユーザーID : " + ID);

                PlayFabClientAPI.GetUserData(new GetUserDataRequest()
                {
                    PlayFabId = ID
                }, result =>
                {
                    isGet = true;
                    data = result.Data;
                    // 通信終了
                    waitConnect.RemoveWait(gameObject.name + gameObject.GetInstanceID());

                    Debug.Log("ユーザーデータの取得に成功");
                }, error =>
                {
                    Debug.Log(error.GenerateErrorReport());
                    // 通信終了
                    waitConnect.RemoveWait(gameObject.name + gameObject.GetInstanceID());
                });
            }
        }
    }
    /// <summary>
    /// プレイヤーデータの取得要求
    /// </summary>
    public void RequestGetData()
    {
        isGet = false;
        // 直ちに一度取得試行をする
        GetData();
    }
}
