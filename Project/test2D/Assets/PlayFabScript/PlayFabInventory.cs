using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

/// <summary>
/// PlayFabインベントリクラス
/// </summary>
public class PlayFabInventory : MonoBehaviour
{
    public bool isGet { get; private set; }

    // 自動リクエストクラス
    private PlayFabAutoRequest request = default;
    // 通信待ちクラス
    [SerializeField] PlayFabWaitConnect waitConnect = default;

    // インベントリ情報
    private Dictionary<string, ItemInstance> inventoryItems = new Dictionary<string, ItemInstance>();

    void Start()
    {
        isGet = false;
        request = GetComponent<PlayFabAutoRequest>();

        if(waitConnect == default)
        {
            GameObject playFabManager = GameObject.Find("PlayFabManager");
            waitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        }
    }


    void Update()
    {
        // インベントリ情報は2回以上自動取得しない
        if (!isGet)
        {
            if(request.IsRequest()) GetUserInventory();
        }
    }

    /// <summary>
    /// インベントリの情報を取得
    /// </summary>
    private void GetUserInventory()
    {
        // 通信待ちでなかったら通信開始
        if (!waitConnect.GetWait(gameObject.name))
        {
            // 通信待ちに設定する
            waitConnect.AddWait(gameObject.name);

            //インベントリの情報の取得
            Debug.Log($"インベントリの情報の取得開始");
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest()
            {
            }, result =>
            {
                inventoryItems.Clear();
                // 通信終了
                waitConnect.RemoveWait(gameObject.name);

                //result.Inventoryがインベントリの情報
                Debug.Log($"インベントリの情報の取得に成功 : インベントリに入ってるアイテム数 {result.Inventory.Count}個");
                //インベントリに入ってる各アイテムの情報をログで表示
                foreach (ItemInstance item in result.Inventory)
                {
                    Debug.Log($"ID : {item.ItemId}, Name : {item.DisplayName}, ItemInstanceId : {item.ItemInstanceId}");
                    // ローカルに保存
                    inventoryItems.Add(item.ItemId, item);
                }
                // 取得済みフラグON
                isGet = true;
            }, error =>
            {
                // 通信終了
                waitConnect.RemoveWait(gameObject.name);
                Debug.LogError($"インベントリの情報の取得に失敗\n{error.GenerateErrorReport()}");
            });
        }

    }

    /// <summary>
    /// インベントリの更新要求
    /// </summary>
    public void RequestUpdate()
    {
        isGet = false;
        request.FinishTimer();
        Update();
    }

    /// <summary>
    /// アイテムを持っているか？
    /// </summary>
    /// <param name="itemID">アイテムID</param>
    /// <returns>true:持っている false:持っていない</returns>
    public bool IsHaveItem(string itemID)
    {
        if (inventoryItems.Count <= 0) return false;
        if (inventoryItems.ContainsKey(itemID)) return true;

        return false;
    }

    /// <summary>
    /// カテゴリー名を指定してアイテム数を返す
    /// </summary>
    /// <param name="categoryName">カテゴリー名</param>
    /// <returns>アイテム数</returns>
    public int CountItemsCategory(string categoryName)
    {
        int ret = 0;
        if(inventoryItems.Count != 0)
        {
            foreach( var item in inventoryItems)
            {
                if (item.Value.CatalogVersion == categoryName)
                {
                    ret++;
                }
            }
        }

        return ret;
    }
}
