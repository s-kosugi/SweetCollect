using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

/// <summary>
/// PlayFabストアクラス
/// </summary>
public class PlayFabStore : MonoBehaviour
{
    /// <summary>
    /// カタログを取得済みかどうか
    /// </summary>
    public bool isCatalogGet { get; private set; }
    /// <summary>
    /// ストアを取得済みかどうか
    /// </summary>
    public bool isStoreGet { get; private set; }
    /// <summary>
    /// カタログアイテムリスト
    /// </summary>
    public List<CatalogItem> CatalogItems { get; private set; }
    /// <summary>
    /// ストアアイテムリスト
    /// </summary>
    public List<StoreItem> StoreItems { get; private set; }

    [SerializeField] string CatalogName = "clothes";
    [SerializeField] string StoreName = "StandardStore";

    /// <summary>
    /// 自動更新するインベントリ　未設定の場合は自動更新しない
    /// </summary>
    [SerializeField] PlayFabInventory Inventory = default;

    /// <summary>
    /// 自動更新する仮想通貨　未設定の場合は自動更新しない
    /// </summary>
    [SerializeField] PlayFabVirtualCurrency VirtualCurrency = default;

    private PlayFabAutoRequest autoRequest = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;

    private const string connectCatalogTaskName = "CatalogData";
    private const string connectStoreTaskName = "StoreData";

    void Start()
    {
        isCatalogGet = false;
        isStoreGet = false;
        autoRequest = GetComponent<PlayFabAutoRequest>();
        if(waitConnect == default)
        {
            GameObject playFabManager = GameObject.Find("PlayFabManager");
            waitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        }
    }

    void Update()
    {
        // ストア情報は2回以上取得しない
        if (isCatalogGet == false && isStoreGet == false)
        {
            if(autoRequest.IsRequest())
            {
                if ( !isCatalogGet ) GetCatalogData();
                if ( !isStoreGet ) GetStoreData();
            }
        }
    }

    /// <summary>
    /// カタログデータの取得
    /// </summary>
    private void GetCatalogData()
    {
        // 通信タスク名はゲームオブジェクト+Catalog
        string taskName = gameObject.name + connectCatalogTaskName;
        if (!waitConnect.GetWait(taskName))
        {
            // 通信待ちに設定する
            waitConnect.AddWait(taskName);

            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
            {
                CatalogVersion = CatalogName,
            }
            , result =>
            {
                Debug.Log("カタログデータ取得成功！");
                CatalogItems = result.Catalog;
                isCatalogGet = true;

            // 通信終了
            waitConnect.RemoveWait(taskName);
            }
            , error =>
            {
                Debug.Log(error.GenerateErrorReport());
            // 通信終了
            waitConnect.RemoveWait(taskName);
            });
        }
    }

    /// <summary>
    /// ストアデータの取得
    /// </summary>
    private void GetStoreData()
    {
        // 通信タスク名はゲームオブジェクト+Store
        string taskName = gameObject.name + connectStoreTaskName;

        if (!waitConnect.GetWait(taskName))
        {
            // 通信待ちに設定する
            waitConnect.AddWait(taskName);

            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
            {
                CatalogVersion = CatalogName,
                StoreId = StoreName
            }
            , (result) =>
            {
                Debug.Log("ストアデータ取得成功！");
                StoreItems = result.Store;
                isStoreGet = true;

                // 通信終了
                waitConnect.RemoveWait(taskName);

            }
            , (error) =>
            {
                Debug.Log(error.GenerateErrorReport());
                // 通信終了
                waitConnect.RemoveWait(taskName);
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
        if (!waitConnect.GetWait(gameObject.name))
        {
            StoreItem item = StoreItems.Find(x => x.ItemId == itemID);
            // アイテムがなかった
            if (item == null)
            {
                Debug.Log("BuyItem is Failed : itemID( " + itemID +" ) storeID( " + StoreName +")");
                return;
            }

            // 通信待ちに設定する
            waitConnect.AddWait(gameObject.name);

            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest()
            {
                CatalogVersion = CatalogName,
                StoreId = StoreName,
                ItemId = itemID,
                VirtualCurrency = virtualCurrency,
                Price = (int)item.VirtualCurrencyPrices[virtualCurrency],
                // キャラクターを使う場合は CharacterId のセットも必要
            }, purchaseResult =>
            {

                // 通信終了
                waitConnect.RemoveWait(gameObject.name);
                Debug.Log($"{purchaseResult.Items[0].DisplayName}購入成功！");
                // インベントリの更新要求
                if( Inventory != default) Inventory.RequestUpdate();
                // 仮想通貨の更新要求
                if (VirtualCurrency != default) VirtualCurrency.RequestUpdate();

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
                waitConnect.RemoveWait(gameObject.name);

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
