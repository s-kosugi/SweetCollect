using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTest : MonoBehaviour
{
    [SerializeField] string ItemID = "003_PASTRYCHEF";
    PlayFabStore store = null;
    PlayFabInventory inventory = null;
    PlayFabPlayerData playerdata = null;
    bool m_isRequest = false;       // リクエスト中かどうか

    void Start()
    {
        store = GameObject.Find("PlayFabManager").GetComponent<PlayFabStore>();
        inventory = GameObject.Find("PlayFabManager").GetComponent<PlayFabInventory>();
        playerdata = GameObject.Find("PlayFabManager").GetComponent<PlayFabPlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // アイテムリストの表示
            store.DebugShowStoreList();
        }
        if(Input.GetMouseButtonDown(1))
        {
            // アイテムの購入
            store.BuyItem(ItemID, "HA");
        }

        // アイテムの装備
        // 更新要求中でなければ入力受付
        if (!m_isRequest)
        {
            if (Input.GetMouseButtonDown(2))
            {
                // インベントリ情報を更新
                inventory.RequestUpdate();
                m_isRequest = true;
            }
        }
        else
        {
            // 更新要求中は更新が終わったかを確認する
            if(inventory.m_isGet)
            {
                // プレイヤーが装備を持っていたら
                if (inventory.IsHaveItem(ItemID))
                {
                    // プレイヤーの装備を更新する
                    playerdata.SetPlayerData(ItemID);
                }
                m_isRequest = false;
            }
        }
    }
}
