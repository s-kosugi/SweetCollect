using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabInventory : MonoBehaviour
{
    public bool m_isGet { get; private set; }

    /// <summary>
    /// 問い合わせ間隔
    /// </summary>
    private const float REQ_INTERVAL = 1.0f;
    /// <summary>
    ///  問い合わせ用タイマー
    /// </summary>
    private float m_RequestTimer = 0.0f;


    void Start()
    {
        m_isGet = false;
    }


    void Update()
    {
        // インベントリ情報は2回以上自動取得しない
        if (!m_isGet)
        {
            // Playfabにログイン済みかを確認する
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                m_RequestTimer += Time.deltaTime;
                // 問い合わせタイマーを満たしていたら問い合わせる
                if (m_RequestTimer >= REQ_INTERVAL)
                {
                    m_RequestTimer = 0.0f;
                    GetUserInventory();
                }
            }
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
        m_RequestTimer = REQ_INTERVAL;
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
