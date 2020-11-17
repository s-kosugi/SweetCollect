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

    private void Start()
    {
        m_AutoRequest = GetComponent<PlayFabAutoRequest>();
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
        if (userName == DisplayName) return;

        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = userName };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSuccess, OnError);

        void OnSuccess(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log("SetDisplayName : success! " + result.DisplayName);
            DisplayName = result.DisplayName;
        }

        void OnError(PlayFabError error)
        {
            Debug.Log($"{error.Error}");
        }
    }

    // Playfabからユーザー名を取得する。
    public void GetUserName()
    {
        // PlayFabにログイン済みかどうかを確認する
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
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
            },
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
            }
            );
        }
    }
}
