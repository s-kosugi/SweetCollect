using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabInventory : MonoBehaviour
{
    public bool m_isGet { get; private set; }

    // 自動リクエストクラス
    private PlayFabAutoRequest m_Request = null;
    // 通信待ちクラス
    private PlayFabWaitConnect m_WaitConnect = null;

    // インベントリ情報
    private Dictionary<string, ItemInstance> m_InventoryItems = new Dictionary<string, ItemInstance>();

    void Start()
    {
        GameObject playFabManager = GameObject.Find("PlayFabManager");
        m_isGet = false;
        m_Request = GetComponent<PlayFabAutoRequest>();
        m_WaitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
    }


    void Update()
    {
        // インベントリ情報は2回以上自動取得しない
        if (!m_isGet)
        {
            if(m_Request.IsRequest()) GetUserInventory();
        }
    }

    /// <summary>
    /// インベントリの情報を取得
    /// </summary>
    private void GetUserInventory()
    {
        // 通信待ちでなかったら通信開始
        if (!m_WaitConnect.GetWait(gameObject.name))
        {
            // 通信待ちに設定する
            m_WaitConnect.AddWait(gameObject.name);

            //インベントリの情報の取得
            Debug.Log($"インベントリの情報の取得開始");
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest()
            {
            }, result =>
            {
                m_InventoryItems.Clear();
                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name);

                //result.Inventoryがインベントリの情報
                Debug.Log($"インベントリの情報の取得に成功 : インベントリに入ってるアイテム数 {result.Inventory.Count}個");
                //インベントリに入ってる各アイテムの情報をログで表示
                foreach (ItemInstance item in result.Inventory)
                {
                    Debug.Log($"ID : {item.ItemId}, Name : {item.DisplayName}, ItemInstanceId : {item.ItemInstanceId}");
                    // ローカルに保存
                    m_InventoryItems.Add(item.ItemId, item);
                }
                // 取得済みフラグON
                m_isGet = true;
            }, error =>
            {
                // 通信終了
                m_WaitConnect.RemoveWait(gameObject.name);
                Debug.LogError($"インベントリの情報の取得に失敗\n{error.GenerateErrorReport()}");
            });
        }

    }

    // インベントリの更新要求
    public void RequestUpdate()
    {
        m_isGet = false;
    }

    /// <summary>
    /// アイテムを持っているか？
    /// </summary>
    /// <param name="itemID">アイテムID</param>
    /// <returns>true:持っている false:持っていない</returns>
    public bool IsHaveItem(string itemID)
    {
        if (m_InventoryItems.Count <= 0) return false;
        if (m_InventoryItems.ContainsKey(itemID)) return true;

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
        if(m_InventoryItems.Count != 0)
        {
            foreach( var item in m_InventoryItems)
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
