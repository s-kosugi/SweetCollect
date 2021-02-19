using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

/// <summary>
/// PlayFabのログイン処理を行うクラス
/// </summary>
public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] float autoLoginTime = 1f;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    private float autoLoginCount = 0f;

    //アカウントを作成するか
    private bool shouldCreateAccount;

    //ログイン時に使うID
    private string customID;

    // IDの長さ
    public int idLength { get; private set; } = 32;

    // PlayFabID
    private string _playfabID;
    public string _PlayfabID
    {
        private set
        { this._playfabID = value; }
        get
        { return this._playfabID; }
    }

    //IDを保存する時のKEY
    public static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";

    //IDに使用する文字
    private static readonly string ID_CHARACTERS = "0123456789abcdefghijklmnopqrstuvwxyz";

    //=================================================================================
    //ログイン処理
    //=================================================================================
    public void Start()
    {
        if (waitConnect == default) waitConnect = GetComponent<PlayFabWaitConnect>();
        Login();
    }

    /// <summary>
    /// ログイン実行
    /// </summary>
    private void Login()
    {
        // 既にログイン済みだったのでIDを保存して終了
        if(PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.Log("PlayFabへログイン済み：IDをPlayFabLoginクラスへ保存します");
            PlayFabAuthenticationContext player = PlayFabSettings.staticPlayer;
            _playfabID = player.PlayFabId;
            return;
        }

        // 通信待ちでなかったら通信開始
        if (!waitConnect.GetWait(gameObject.name))
        {
            customID = LoadCustomID();
            var request = new LoginWithCustomIDRequest { CustomId = customID, CreateAccount = shouldCreateAccount };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

            // 通信待ちに設定する
            waitConnect.AddWait(gameObject.name);
        }
    }

    /// <summary>
    /// ログイン成功
    /// </summary>
    /// <param name="result">成功結果</param>
    private void OnLoginSuccess(LoginResult result)
    {
        // 通信待ちを解除する
        waitConnect.RemoveWait(gameObject.name);

        // アカウントを作成しようとしたのに、IDが既に使われていて、出来なかった場合
        if (shouldCreateAccount && !result.NewlyCreated)
        {
            Debug.LogWarning($"CustomId : {customID} は既に使われています。");
            Login();//ログインしなおし
            return;
        }

        // アカウント作成時にIDを保存
        if (result.NewlyCreated)
        {
            SaveCustomID();
        }
        Debug.Log($"PlayFabのログインに成功\nPlayFabId : {result.PlayFabId}, CustomId : {customID}\nアカウントを作成したか : {result.NewlyCreated}");
        // PlayFabIDを保存
        _playfabID = result.PlayFabId;
    }
    private void Update()
    {
        // 未ログインならログインをn秒毎に試行する
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            autoLoginCount += Time.deltaTime;
            if (autoLoginCount >= autoLoginTime)
            {
                autoLoginCount = 0f;
                Login();
            }
        }
    }

    /// <summary>
    /// ログイン失敗
    /// </summary>
    /// <param name="error">エラー内容</param>
    private void OnLoginFailure(PlayFabError error)
    {

        // 通信待ちを解除する
        waitConnect.RemoveWait(gameObject.name);

        Debug.LogError($"PlayFabのログインに失敗\n{error.GenerateErrorReport()}");

    }

    //=================================================================================
    //カスタムIDの取得
    //=================================================================================

    /// <summary>
    /// CustomIDの読み込み
    /// </summary>
    /// <returns>ID文字列</returns>
    private string LoadCustomID()
    {
        //IDを取得
        string id = PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY);

        //保存されていなければ新規生成
        shouldCreateAccount = string.IsNullOrEmpty(id);
        return shouldCreateAccount ? GenerateCustomID() : id;
    }

    /// <summary>
    /// CustomIDの保存
    /// </summary>
    private void SaveCustomID()
    {
        PlayerPrefs.SetString(CUSTOM_ID_SAVE_KEY, customID);
    }

    /// <summary>
    /// CustomIDの生成
    /// </summary>
    /// <returns>生成されたID</returns>
    private string GenerateCustomID()
    {
        StringBuilder stringBuilder = new StringBuilder(idLength);
        var random = new System.Random();

        //ランダムにIDを生成
        for (int i = 0; i < idLength; i++)
        {
            stringBuilder.Append(ID_CHARACTERS[random.Next(ID_CHARACTERS.Length)]);
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// ログアウト処理
    /// </summary>
    public void LogOut()
    {
        // ログアウト処理
        PlayFabClientAPI.ForgetAllCredentials();
    }

    
    /// <summary>
    /// 保存されているCustomIDの取得
    /// </summary>
    /// <returns>CustomID文字列</returns>
    public string GetCustomID()
    {
        return PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY);
    }

}