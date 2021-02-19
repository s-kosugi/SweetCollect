using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

/// <summary>
/// PlayFab仮想通貨クラス
/// </summary>
public class PlayFabVirtualCurrency : MonoBehaviour
{
    /// <summary>
    /// 仮想通貨を取得済みかどうか
    /// </summary>
    public bool isGet{ get; private set; }

    private PlayFabAutoRequest autoRequest = null;
    [SerializeField] PlayFabWaitConnect waitConnect = default;

    /// <summary>
    /// 仮想通貨の連想配列
    /// </summary>
    private Dictionary<string, int> m_DicVirtualCurrency = new Dictionary<string,int>();
    public Dictionary<string, int> VirtualCurrency
    {
        get { return this.m_DicVirtualCurrency; }
    }


    void Start()
    {
        autoRequest = GetComponent<PlayFabAutoRequest>();
        if (waitConnect == default)
        {
            GameObject playFabManager = GameObject.Find("PlayFabManager");
            waitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        }
    }


    void Update()
    {
        // 取得済みの場合は自動で取得しない
        if (!isGet)
        {
            // Playfabにログイン済みかを確認する
            if(autoRequest.IsRequest())
                GetUserVirtualCurrency();
        }
    }

    /// <summary>
    /// 仮想通貨の更新要求
    /// </summary>
    public void RequestUpdate()
    {
        isGet = false;
        autoRequest.FinishTimer();
        Update();
    }

    /// <summary>
    /// 仮想通貨の情報を取得
    /// </summary>
    public void GetUserVirtualCurrency()
    {
        // 通信待ちでなかったら通信開始
        if (!waitConnect.GetWait(gameObject.name))
        {
            // 通信待ちに設定する
            waitConnect.AddWait(gameObject.name);

            //GetUserInventoryRequestのインスタンスを生成
            var userInventoryRequest = new GetUserInventoryRequest();

            //インベントリの情報の取得
            Debug.Log($"仮想通貨の情報の取得開始");
            PlayFabClientAPI.GetUserInventory(userInventoryRequest, OnSuccessGet, OnErrorGet);
        }
    }

    /// <summary>
    /// 仮想通貨の情報の取得に成功
    /// </summary>
    /// <param name="result">実行結果</param>
    private void OnSuccessGet(GetUserInventoryResult result)
    {
        //result.Inventoryがインベントリの情報
        Debug.Log($"仮想通貨の情報の取得に成功");

        // 通信終了
        waitConnect.RemoveWait(gameObject.name);

        foreach (var virtualCurrency in result.VirtualCurrency)
        {
            //所持している仮想通貨の情報をログで表示
            Debug.Log($"仮想通貨 {virtualCurrency.Key} : {virtualCurrency.Value}");

            // 仮想通貨をクラス内に格納する
            if (!m_DicVirtualCurrency.ContainsKey(virtualCurrency.Key))
                m_DicVirtualCurrency.Add(virtualCurrency.Key, virtualCurrency.Value);
            else
                m_DicVirtualCurrency[virtualCurrency.Key] = virtualCurrency.Value;
        }

        isGet = true;
    }


    /// <summary>
    /// 仮想通貨の情報の取得に失敗
    /// </summary>
    /// <param name="error">エラー内容</param>
    private void OnErrorGet(PlayFabError error)
    {
        // 通信終了
        waitConnect.RemoveWait(gameObject.name);

        Debug.LogError($"仮想通貨の情報の取得に失敗\n{error.GenerateErrorReport()}");
    }


    /// <summary>
    /// 仮想通貨を追加
    /// </summary>
    public void AddUserVirtualCurrency(string VCcode, int value)
    {
        // 通信待ちでなかったら通信開始
        if (!waitConnect.GetWait(gameObject.name))
        {
            // 通信待ちに設定する
            waitConnect.AddWait(gameObject.name);
            //AddUserVirtualCurrencyRequestのインスタンスを生成
            var addUserVirtualCurrencyRequest = new AddUserVirtualCurrencyRequest
            {
                Amount = value,           //追加する金額
                VirtualCurrency = VCcode, //仮想通貨のコード
            };

            //仮想通貨の追加
            Debug.Log($"仮想通貨の追加開始");
            PlayFabClientAPI.AddUserVirtualCurrency(addUserVirtualCurrencyRequest, OnSuccessAdd, OnErrorAdd);
        }
    }

    /// <summary>
    /// 仮想通貨の追加に成功
    /// </summary>
    /// <param name="result">追加結果</param>
    private void OnSuccessAdd(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log($"仮想通貨の追加に成功");
        // 通信終了
        waitConnect.RemoveWait(gameObject.name);

        //仮想通貨の情報をログで表示
        Debug.Log($"変更した仮想通貨のコード : {result.VirtualCurrency}");
        Debug.Log($"変更後の残高 : {result.Balance}");
        Debug.Log($"加算額 : {result.BalanceChange}");
    }

    /// <summary>
    /// 仮想通貨の追加に失敗
    /// </summary>
    /// <param name="error">エラー内容</param>
    private void OnErrorAdd(PlayFabError error)
    {
        // 通信終了
        waitConnect.RemoveWait(gameObject.name);

        Debug.LogError($"仮想通貨の追加に失敗\n{error.GenerateErrorReport()}");
    }

    /// <summary>
    /// 仮想通貨を減らす
    /// </summary>
    /// <param name="VCcode">仮想通貨コード</param>
    /// <param name="value">減らす値</param>
    public void SubtractUserVirtualCurrency(string VCcode, int value)
    {
        // 通信待ちでなかったら通信開始
        if (!waitConnect.GetWait(gameObject.name))
        {
            // 通信待ちに設定する
            waitConnect.AddWait(gameObject.name);

            //AddUserVirtualCurrencyRequestのインスタンスを生成
            var subtractUserVirtualCurrencyRequest = new SubtractUserVirtualCurrencyRequest
            {
                Amount = value,   //減らす金額
                VirtualCurrency = VCcode, //仮想通貨のコード
            };

            // 仮想通貨の減額
            Debug.Log($"仮想通貨の減額開始");
            PlayFabClientAPI.SubtractUserVirtualCurrency(subtractUserVirtualCurrencyRequest, OnSuccessSub, OnErrorSub);
        }
    }

    //=================================================================================
    //減額結果
    //=================================================================================

    /// <summary>
    /// 仮想通貨の減額に成功
    /// </summary>
    /// <param name="result">減額結果内容</param>
    private void OnSuccessSub(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log($"仮想通貨の減額に成功");
        // 通信終了
        waitConnect.RemoveWait(gameObject.name);

        // 仮想通貨の情報をログで表示
        Debug.Log($"変更した仮想通貨のコード : {result.VirtualCurrency}");
        Debug.Log($"変更後の残高 : {result.Balance}");
        Debug.Log($"減額した額 : {result.BalanceChange}");
    }

    /// <summary>
    /// 仮想通貨の減額に失敗
    /// </summary>
    /// <param name="error">エラー内容</param>
    private void OnErrorSub(PlayFabError error)
    {
        // 通信終了
        waitConnect.RemoveWait(gameObject.name);
        Debug.LogError($"仮想通貨の減額に失敗\n{error.GenerateErrorReport()}");
    }
}
