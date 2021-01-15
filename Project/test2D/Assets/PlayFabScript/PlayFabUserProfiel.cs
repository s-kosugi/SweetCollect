using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

// PlayFabのプロフィール関連に関するクラス
public class PlayFabUserProfiel : MonoBehaviour
{
    // ユーザー名
    public string DisplayName { get; private set;}
    public bool isGet { get; private set; }
    private PlayFabAutoRequest m_AutoRequest = null;
    private PlayFabWaitConnect m_WaitConnect = null;
    public SETNAME_RESULT setNameResult { get; private set; }

    public enum SETNAME_RESULT
    {
        NONE,
        SUCCESS,
        ERROR,
    }


    private void Start()
    {
        m_AutoRequest = GetComponent<PlayFabAutoRequest>();
        setNameResult = SETNAME_RESULT.NONE;

        GameObject playFabManager = GameObject.Find("PlayFabManager");
        m_WaitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
    }

    public void Update()
    {
        // ユーザー名が未取得の場合は取得する。
        if(!isGet)
        {
            if(m_AutoRequest.IsRequest()) GetUserName();
        }
    }

    // PlayfabへUserName(DisplayName)を更新する。
    public void SetUserName(string userName)
    {
        // 取得した名前と同じだった場合は更新しない
        if (userName == DisplayName)
        {
            // 成功したことにする
            setNameResult = SETNAME_RESULT.SUCCESS;
            return;
        }
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(gameObject.name))
        {
            // 通信待ちに設定する
            m_WaitConnect.AddWait(gameObject.name);

            var request = new UpdateUserTitleDisplayNameRequest { DisplayName = userName };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSuccess, OnError);

            void OnSuccess(UpdateUserTitleDisplayNameResult result)
            {
                Debug.Log("SetDisplayName : success! " + result.DisplayName);
                DisplayName = result.DisplayName;
                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name);

                setNameResult = SETNAME_RESULT.SUCCESS;
            }

            void OnError(PlayFabError error)
            {
                Debug.Log($"{error.Error}");
                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name);

                setNameResult = SETNAME_RESULT.ERROR;
            }
        }
    }

    // Playfabからユーザー名を取得する。
    public void GetUserName()
    {
        // PlayFabにログイン済みかどうかを確認する
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            // 通信待ちでなかったら通信開始
            if (!m_WaitConnect.GetWait(gameObject.name))
            {
                // 通信待ちに設定する
                m_WaitConnect.AddWait(gameObject.name);

                PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
                {

                    PlayFabId = transform.parent.GetComponent<PlayFabLogin>()._PlayfabID,
                    ProfileConstraints = new PlayerProfileViewConstraints
                    {
                        ShowDisplayName = true
                    }
                },
                result =>
                {
                    DisplayName = result.PlayerProfile.DisplayName;
                    Debug.Log($"DisplayName: {DisplayName}");
                    isGet = true;
                    // 通信終了
                    m_WaitConnect.RemoveWait(gameObject.name);
                },
                error =>
                {
                    Debug.LogError(error.GenerateErrorReport());
                    // 通信終了
                    m_WaitConnect.RemoveWait(gameObject.name);
                }
                );
            }
        }
    }
}
