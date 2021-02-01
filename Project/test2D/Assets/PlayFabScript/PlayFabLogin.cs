using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayFabのログイン処理を行うクラス
/// </summary>
public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] float AutoLoginTime = 1f;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    private float AutoLoginCount = 0f;

    //アカウントを作成するか
    private bool _shouldCreateAccount;

    //ログイン時に使うID
    private string _customID;

    // PlayFabID
    private string _playfabID;
    public string _PlayfabID
    {
        private set
        { this._playfabID = value; }
        get
        { return this._playfabID; }
    }

    //=================================================================================
    //ログイン処理
    //=================================================================================
    public void Start()
    {
        if (waitConnect == default) waitConnect = GetComponent<PlayFabWaitConnect>();
        Login();
    }

    //ログイン実行
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
            _customID = LoadCustomID();
            var request = new LoginWithCustomIDRequest { CustomId = _customID, CreateAccount = _shouldCreateAccount };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

            // 通信待ちに設定する
            waitConnect.AddWait(gameObject.name);
        }
    }

    //ログイン成功
    private void OnLoginSuccess(LoginResult result)
    {
        // 通信待ちを解除する
        waitConnect.RemoveWait(gameObject.name);

        //アカウントを作成しようとしたのに、IDが既に使われていて、出来なかった場合
        if (_shouldCreateAccount && !result.NewlyCreated)
        {
            Debug.LogWarning($"CustomId : {_customID} は既に使われています。");
            Login();//ログインしなおし
            return;
        }

        //アカウント作成時にIDを保存
        if (result.NewlyCreated)
        {
            SaveCustomID();
        }
        Debug.Log($"PlayFabのログインに成功\nPlayFabId : {result.PlayFabId}, CustomId : {_customID}\nアカウントを作成したか : {result.NewlyCreated}");
        // PlayFabIDを保存
        _playfabID = result.PlayFabId;
    }
    private void Update()
    {
        // 未ログインならログインをn秒毎に試行する
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            AutoLoginCount += Time.deltaTime;
            if (AutoLoginCount >= AutoLoginTime)
            {
                AutoLoginCount = 0f;
                Login();
            }
        }
    }
    //ログイン失敗
    private void OnLoginFailure(PlayFabError error)
    {

        // 通信待ちを解除する
        waitConnect.RemoveWait(gameObject.name);

        Debug.LogError($"PlayFabのログインに失敗\n{error.GenerateErrorReport()}");

    }

    //=================================================================================
    //カスタムIDの取得
    //=================================================================================

    //IDを保存する時のKEY
    private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";

    //IDを取得
    private string LoadCustomID()
    {
        //IDを取得
        string id = PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY);

        //保存されていなければ新規生成
        _shouldCreateAccount = string.IsNullOrEmpty(id);
        return _shouldCreateAccount ? GenerateCustomID() : id;
    }

    //IDの保存
    private void SaveCustomID()
    {
        PlayerPrefs.SetString(CUSTOM_ID_SAVE_KEY, _customID);
    }

    //=================================================================================
    //カスタムIDの生成
    //=================================================================================

    //IDに使用する文字
    private static readonly string ID_CHARACTERS = "0123456789abcdefghijklmnopqrstuvwxyz";

    //IDを生成する
    private string GenerateCustomID()
    {
        int idLength = 32;//IDの長さ
        StringBuilder stringBuilder = new StringBuilder(idLength);
        var random = new System.Random();

        //ランダムにIDを生成
        for (int i = 0; i < idLength; i++)
        {
            stringBuilder.Append(ID_CHARACTERS[random.Next(ID_CHARACTERS.Length)]);
        }

        return stringBuilder.ToString();
    }

    //=================================================================================
    //取得
    //=================================================================================

    /// <summary>
    /// タイトルデータの取得
    /// </summary>
    public void GetTitleData()
    {
        //GetTitleDataRequestのインスタンスを生成
        var request = new GetTitleDataRequest();

        //タイトルデータ取得開始
        Debug.Log($"タイトルデータの取得開始");
        PlayFabClientAPI.GetTitleData(request, OnSuccess, OnError);
    }

    //=================================================================================
    //取得結果
    //=================================================================================

    //タイトルデータの取得に成功
    private void OnSuccess(GetTitleDataResult result)
    {
        Debug.Log($"タイトルデータの取得に成功しました");

        //result.Data(Dictionary)に全タイトルデータが入っていて、Keyを文字列で指定すると値が取り出せる
        //_text.text = $"Test1 : {result.Data["Test1"]}\nTest2 : {result.Data["Test2"]}";
    }

    //タイトルデータの取得に失敗
    private void OnError(PlayFabError error)
    {
        Debug.LogWarning($"タイトルデータの取得に失敗しました : {error.GenerateErrorReport()}");
    }
}