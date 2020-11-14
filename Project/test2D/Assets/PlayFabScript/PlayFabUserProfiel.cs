using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

// PlayFabのプロフィール関連に関するクラス
public class PlayFabUserProfiel : MonoBehaviour
{
    // ユーザー名
    private string DisplayName = "";
    public string displayName
    {
        get { return DisplayName; }
    }

    // 問い合わせ用タイマー
    private float RequestTimer = 0.5f;
    public void Update()
    {
        // ユーザー名が未取得の場合は取得する。
        if(displayName == "")
        {
            // 1秒毎にユーザー名を問い合わせする
            RequestTimer += Time.deltaTime;
            if (RequestTimer > 1.0f)
            {
                RequestTimer = 0f;
                GetUserName();
            }
        }
    }

    // PlayfabへUserName(DisplayName)を更新する。
    public void SetUserName(string userName)
    {
        // 取得した名前と同じだった場合は更新しない
        if (userName == displayName) return;

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

                PlayFabId = GetComponent<PlayFabLogin>()._PlayfabID,
                ProfileConstraints = new PlayerProfileViewConstraints
                {
                    ShowDisplayName = true
                }
            },
            result =>
            {
                DisplayName = result.PlayerProfile.DisplayName;
                Debug.Log($"DisplayName: {DisplayName}");
            },
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
            }
            );
        }
    }
}
