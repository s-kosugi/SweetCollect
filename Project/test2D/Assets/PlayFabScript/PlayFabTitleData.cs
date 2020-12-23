using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;


public class PlayFabTitleData : MonoBehaviour
{
    // 受信した全データ
    public Dictionary<string, string> titleData { get; private set; } = default;

    private PlayFabAutoRequest m_AutoRequest = null;

    public bool m_isGet { get; private set; }

    void Start()
    {
        GameObject playFabManager = GameObject.Find("PlayFabManager");
        m_AutoRequest = GetComponent<PlayFabAutoRequest>();
        m_isGet = false;
    }

    // Update is called once per frame
    void Update()
    {
        // タイトル情報は2回以上自動取得しない
        if (!m_isGet)
        {
            // 自動リクエストができる且つログイン済みだったらタイトルデータ取得を試みる
            if (m_AutoRequest.IsRequest() && PlayFabClientAPI.IsClientLoggedIn()) GetTitleData();
        }
    }

    private void GetTitleData()
    {
        var request = new GetTitleDataRequest();
        PlayFabClientAPI.GetTitleData(request, OnSuccess, OnError);

        void OnSuccess(GetTitleDataResult result)
        {
            Debug.Log("PlayFabTitleData:タイトルデータの取得に成功");

            // タイトルデータのコピー
            titleData = result.Data;

            m_isGet = true;

        }

        void OnError(PlayFabError error)
        {
            Debug.Log("PlayFabTitleData:タイトルデータの取得に失敗");
            Debug.Log(error.GenerateErrorReport());
        }
    }
}
