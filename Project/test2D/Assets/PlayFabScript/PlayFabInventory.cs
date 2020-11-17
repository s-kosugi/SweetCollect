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


    void Start()
    {
        m_isGet = false;
        m_Request = GetComponent<PlayFabAutoRequest>();
    }


    void Update()
    {
        // インベントリ情報は2回以上自動取得しない
        if (!m_isGet)
        {
            if(m_Request.IsRequest()) GetUserInventory();
        }
    }

    private Dictionary<string, ItemInstance> m_InventoryItems = new Dictionary<string, ItemInstance>();

    /// <summary>
    /// インベントリの情報を取得
    /// </summary>
    private void GetUserInventory()
    {
        //インベントリの情報の取得
        Debug.Log($"インベントリの情報の取得開始");
        PlayFabClientAPI.GetUserInventory( new GetUserInventoryRequest()
        {
        }, result =>
        {
            m_InventoryItems.Clear();

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
            Debug.LogError($"インベントリの情報の取得に失敗\n{error.GenerateErrorReport()}");
        });

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
        if (m_InventoryItems[itemID] != null) return true;

        return false;
    }
}
