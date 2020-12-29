using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabStore : MonoBehaviour
{
    public bool m_isCatalogGet { get; private set; }
    public bool m_isStoreGet { get; private set; }

    [SerializeField] string CatalogName = "clothes";
    [SerializeField] string StoreName = "StandardStore";

    private PlayFabAutoRequest m_AutoRequest = null;
    private PlayFabWaitConnect m_WaitConnect = null;

    private const string connectCatalogTaskName = "CatalogData";
    private const string connectStoreTaskName = "StoreData";

    void Start()
    {
        GameObject playFabManager = GameObject.Find("PlayFabManager");
        m_isCatalogGet = false;
        m_isStoreGet = false;
        m_AutoRequest = GetComponent<PlayFabAutoRequest>();
        m_WaitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
    }

    void Update()
    {
        // ストア情報は2回以上取得しない
        if (m_isCatalogGet == false && m_isStoreGet == false)
        {
            if(m_AutoRequest.IsRequest())
            {
                if ( !m_isCatalogGet ) GetCatalogData();
                if ( !m_isStoreGet ) GetStoreData();
            }
        }
    }

    public List<CatalogItem> CatalogItems { get; private set; }
    public List<StoreItem> StoreItems { get; private set; }

    private void GetCatalogData()
    {
        // 通信タスク名はゲームオブジェクト+Catalog
        string taskName = gameObject.name + connectCatalogTaskName;
        if (!m_WaitConnect.GetWait(taskName))
        {
            // 通信待ちに設定する
            m_WaitConnect.AddWait(taskName);

            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
            {
                CatalogVersion = CatalogName,
            }
            , result =>
            {
                Debug.Log("カタログデータ取得成功！");
                CatalogItems = result.Catalog;
                m_isCatalogGet = true;

            // 通信終了
            m_WaitConnect.RemoveWait(taskName);
            }
            , error =>
            {
                Debug.Log(error.GenerateErrorReport());
            // 通信終了
            m_WaitConnect.RemoveWait(taskName);
            });
        }
    }

    private void GetStoreData()
    {
        // 通信タスク名はゲームオブジェクト+Store
        string taskName = gameObject.name + connectStoreTaskName;

        if (!m_WaitConnect.GetWait(taskName))
        {
            // 通信待ちに設定する
            m_WaitConnect.AddWait(taskName);

            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
            {
                CatalogVersion = CatalogName,
                StoreId = StoreName
            }
            , (result) =>
            {
                Debug.Log("ストアデータ取得成功！");
                StoreItems = result.Store;
                m_isStoreGet = true;

                // 通信終了
                m_WaitConnect.RemoveWait(taskName);

            }
            , (error) =>
            {
                Debug.Log(error.GenerateErrorReport());
                // 通信終了
                m_WaitConnect.RemoveWait(taskName);
            });
        }
    }

    /// <summary>
    /// アイテムの購入
    /// </summary>
    /// <param name="itemID">アイテムID</param>
    /// <param name="virtualCurrency">通貨コード</param>
    public void BuyItem(string itemID,string virtualCurrency)
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(gameObject.name))
        {
            // 通信待ちに設定する
            m_WaitConnect.AddWait(gameObject.name);

            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest()
            {
                CatalogVersion = CatalogName,
                StoreId = StoreName,
                ItemId = itemID,
                VirtualCurrency = virtualCurrency,
                Price = (int)StoreItems.Find(x => x.ItemId == itemID).VirtualCurrencyPrices[virtualCurrency],
                // キャラクターを使う場合は CharacterId のセットも必要
            }, purchaseResult =>
            {

                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name);
                Debug.Log($"{purchaseResult.Items[0].DisplayName}購入成功！");
                PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest()
                , result =>
                {
                    Debug.Log("購入後のお金は " + result.VirtualCurrency[virtualCurrency].ToString("#,0") + "だよ");
                }, error =>
                {
                    Debug.Log(error.GenerateErrorReport());
                });
            }, error =>
            {
                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name);

                // 金額不足
                if (error.Error == PlayFabErrorCode.InsufficientFunds)
                {
                    Debug.Log("金額が不足しているため購入できません。");
                }
                Debug.Log(error.GenerateErrorReport());
            });
        }
    }

    /// <summary>
    /// デバッグ表示用ストアアイテム一覧表示
    /// </summary>
    public void DebugShowStoreList()
    {
        // ストアアイテムの一覧
        foreach (var storeItem in StoreItems)
        {
            // カタログと一致するアイテムの取得
            var catalogItem = CatalogItems.Find(x => x.ItemId == storeItem.ItemId);

            string itemId = storeItem.ItemId; // アイテムID
            string displayName = catalogItem.DisplayName; // 表示名
            string description = catalogItem.Description; // アイテムの説明
            uint price = storeItem.VirtualCurrencyPrices["HA"]; // 仮想通貨の価格

            Debug.Log(" ID : " + itemId + "  Name : " + displayName + "  Desc : " + description + "  Price : " + price);
        }
    }
}
