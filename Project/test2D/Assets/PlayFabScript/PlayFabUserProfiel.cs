using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

/// <summary>
/// PlayFabのプロフィール関連に関するクラス
/// </summary>
public class PlayFabUserProfiel : MonoBehaviour
{
    /// <summary>
    /// ユーザー名
    /// </summary>
    public string DisplayName { get; private set;}
    public bool isGet { get; private set; }
    private PlayFabAutoRequest autoRequest = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    public SETNAME_RESULT setNameResult { get; private set; }

    /// <summary>
    /// 名前変更結果
    /// </summary>
    public enum SETNAME_RESULT
    {
        NONE,
        SUCCESS,
        ERROR,
    }


    private void Start()
    {
        autoRequest = GetComponent<PlayFabAutoRequest>();
        setNameResult = SETNAME_RESULT.NONE;

        if (waitConnect == default)
        {
            GameObject playFabManager = GameObject.Find("PlayFabManager");
            waitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        }
    }

    public void Update()
    {
        // ユーザー名が未取得の場合は取得する。
        if(!isGet)
        {
            if(autoRequest.IsRequest()) GetUserName();
        }
    }

    /// <summary>
    /// PlayfabへUserName(DisplayName)を更新する。
    /// </summary>
    /// <param name="userName">変更したい名前</param>
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
        if (!waitConnect.GetWait(gameObject.name))
        {
            // 通信待ちに設定する
            waitConnect.AddWait(gameObject.name);

            var request = new UpdateUserTitleDisplayNameRequest { DisplayName = userName };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSuccess, OnError);

            // 成功した場合
            void OnSuccess(UpdateUserTitleDisplayNameResult result)
            {
                Debug.Log("SetDisplayName : success! " + result.DisplayName);
                DisplayName = result.DisplayName;
                // 通信終了
                waitConnect.RemoveWait(gameObject.name);

                setNameResult = SETNAME_RESULT.SUCCESS;
            }

            // 失敗した場合
            void OnError(PlayFabError error)
            {
                Debug.Log($"{error.Error}");
                // 通信終了
                waitConnect.RemoveWait(gameObject.name);

                setNameResult = SETNAME_RESULT.ERROR;
            }
        }
    }

    /// <summary>
    /// Playfabからユーザー名を取得する。
    /// </summary>
    private void GetUserName()
    {
        // PlayFabにログイン済みかどうかを確認する
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            // 通信待ちでなかったら通信開始
            if (!waitConnect.GetWait(gameObject.name))
            {
                // 通信待ちに設定する
                waitConnect.AddWait(gameObject.name);

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
                    waitConnect.RemoveWait(gameObject.name);
                },
                error =>
                {
                    Debug.LogError(error.GenerateErrorReport());
                    // 通信終了
                    waitConnect.RemoveWait(gameObject.name);
                }
                );
            }
        }
    }
}
