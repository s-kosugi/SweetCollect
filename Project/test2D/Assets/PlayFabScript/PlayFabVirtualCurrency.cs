﻿using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayFabVirtualCurrency : MonoBehaviour
{
    // 取得済みかどうか？
    public bool isGet{ get; private set; }

    private PlayFabAutoRequest m_AutoRequest = null;
    private PlayFabWaitConnect m_WaitConnect = null;

    // 仮想通貨の連想配列
    private Dictionary<string, int> m_DicVirtualCurrency = new Dictionary<string,int>();
    public Dictionary<string, int> VirtualCurrency
    {
        get { return this.m_DicVirtualCurrency; }
    }


    void Start()
    {
        GameObject playFabManager = GameObject.Find("PlayFabManager");
        m_AutoRequest = GetComponent<PlayFabAutoRequest>();
        m_WaitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
    }

    // Update is called once per frame
    void Update()
    {
        // 取得済みの場合は自動で取得しない
        if (!isGet)
        {
            // Playfabにログイン済みかを確認する
            if(m_AutoRequest.IsRequest())
                GetUserVirtualCurrency();
        }
    }
    /// <summary>
    /// 仮想通貨の情報を取得
    /// </summary>
    public void GetUserVirtualCurrency()
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(transform))
        {
            // 通信待ちに設定する
            m_WaitConnect.SetWait(transform, true);

            //GetUserInventoryRequestのインスタンスを生成
            var userInventoryRequest = new GetUserInventoryRequest();

            //インベントリの情報の取得
            Debug.Log($"仮想通貨の情報の取得開始");
            PlayFabClientAPI.GetUserInventory(userInventoryRequest, OnSuccessGet, OnErrorGet);
        }
    }

    //仮想通貨の情報の取得に成功
    private void OnSuccessGet(GetUserInventoryResult result)
    {
        //result.Inventoryがインベントリの情報
        Debug.Log($"仮想通貨の情報の取得に成功");

        // 通信終了
        m_WaitConnect.SetWait(transform, false);

        //所持している仮想通貨の情報をログで表示
        foreach (var virtualCurrency in result.VirtualCurrency)
        {
            Debug.Log($"仮想通貨 {virtualCurrency.Key} : {virtualCurrency.Value}");

            // 仮想通貨をクラス内に格納する
            if (!m_DicVirtualCurrency.ContainsKey(virtualCurrency.Key))
                m_DicVirtualCurrency.Add(virtualCurrency.Key, virtualCurrency.Value);
            else
                m_DicVirtualCurrency[virtualCurrency.Key] = virtualCurrency.Value;
        }

        isGet = true;
    }

    //仮想通貨の情報の取得に失敗
    private void OnErrorGet(PlayFabError error)
    {
        // 通信終了
        m_WaitConnect.SetWait(transform, false);

        Debug.LogError($"仮想通貨の情報の取得に失敗\n{error.GenerateErrorReport()}");
    }


    /// <summary>
    /// 仮想通貨を追加
    /// </summary>
    public void AddUserVirtualCurrency(string VCcode, int value)
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(transform))
        {
            // 通信待ちに設定する
            m_WaitConnect.SetWait(transform, true);
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

    //仮想通貨の追加に成功
    private void OnSuccessAdd(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log($"仮想通貨の追加に成功");
        // 通信終了
        m_WaitConnect.SetWait(transform, false);

        //仮想通貨の情報をログで表示
        Debug.Log($"変更した仮想通貨のコード : {result.VirtualCurrency}");
        Debug.Log($"変更後の残高 : {result.Balance}");
        Debug.Log($"加算額 : {result.BalanceChange}");
    }

    //仮想通貨の追加に失敗
    private void OnErrorAdd(PlayFabError error)
    {
        // 通信終了
        m_WaitConnect.SetWait(transform, false);

        Debug.LogError($"仮想通貨の追加に失敗\n{error.GenerateErrorReport()}");
    }

    /// <summary>
    /// 仮想通貨を減らす
    /// </summary>
    public void SubtractUserVirtualCurrency(string VCcode, int value)
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(transform))
        {
            // 通信待ちに設定する
            m_WaitConnect.SetWait(transform, true);

            //AddUserVirtualCurrencyRequestのインスタンスを生成
            var subtractUserVirtualCurrencyRequest = new SubtractUserVirtualCurrencyRequest
            {
                Amount = value,   //減らす金額
                VirtualCurrency = VCcode, //仮想通貨のコード
            };

            //仮想通貨の減額
            Debug.Log($"仮想通貨の減額開始");
            PlayFabClientAPI.SubtractUserVirtualCurrency(subtractUserVirtualCurrencyRequest, OnSuccessSub, OnErrorSub);
        }
    }

    //=================================================================================
    //減額結果
    //=================================================================================

    //仮想通貨の減額に成功
    private void OnSuccessSub(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log($"仮想通貨の減額に成功");
        // 通信終了
        m_WaitConnect.SetWait(transform, false);

        //仮想通貨の情報をログで表示
        Debug.Log($"変更した仮想通貨のコード : {result.VirtualCurrency}");
        Debug.Log($"変更後の残高 : {result.Balance}");
        Debug.Log($"減額した額 : {result.BalanceChange}");
    }

    //仮想通貨の減額に失敗
    private void OnErrorSub(PlayFabError error)
    {
        // 通信終了
        m_WaitConnect.SetWait(transform, false);
        Debug.LogError($"仮想通貨の減額に失敗\n{error.GenerateErrorReport()}");
    }
}