using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

/// <summary>
/// PlayFabタイトルデータクラス
/// </summary>
public class PlayFabTitleData : MonoBehaviour
{
    /// <summary>
    /// 受信した全データ
    /// </summary>
    public Dictionary<string, string> titleData { get; private set; } = default;

    private PlayFabAutoRequest autoRequest = default;

    /// <summary>
    /// データ取得済みかどうか
    /// </summary>
    public bool isGet { get; private set; }

    void Start()
    {
        autoRequest = GetComponent<PlayFabAutoRequest>();
        isGet = false;
    }


    void Update()
    {
        // タイトル情報は2回以上自動取得しない
        if (!isGet)
        {
            // 自動リクエストができる且つログイン済みだったらタイトルデータ取得を試みる
            if (autoRequest.IsRequest() && PlayFabClientAPI.IsClientLoggedIn()) GetTitleData();
        }
    }

    /// <summary>
    /// タイトルデータの取得
    /// </summary>
    private void GetTitleData()
    {
        var request = new GetTitleDataRequest();
        PlayFabClientAPI.GetTitleData(request, OnSuccess, OnError);

        void OnSuccess(GetTitleDataResult result)
        {
            Debug.Log("PlayFabTitleData:タイトルデータの取得に成功");

            // タイトルデータのコピー
            titleData = result.Data;

            isGet = true;

        }

        void OnError(PlayFabError error)
        {
            Debug.Log("PlayFabTitleData:タイトルデータの取得に失敗");
            Debug.Log(error.GenerateErrorReport());
        }
    }
}
