using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 実績解放通知クラス
/// </summary>
public class NoticeAchievement : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] ReachAchievement reachAchievement = default;
    [SerializeField] PlayFabStore store = default;
    bool isSend = true;


    void Update()
    {
        if (playerData.isGet && reachAchievement.isSet && store.isStoreGet && !isSend)
        {
            foreach( var item in store.StoreItems)
            {
                UserDataRecord record = default;
                string key = "NOTICE_" + item.ItemId;
                // 通知キーが未生成なら通知をONにする
                if (!playerData.data.TryGetValue(key, out record))
                {
                    // 実績達成済みなら送信
                    if (reachAchievement.isSet && reachAchievement.IsReachAchievement(item.ItemId))
                    {
                        playerData.SetPlayerData(key, "TRUE");
                    }
                }
            }
            isSend = true;
        }
    }

    /// <summary>
    /// 実績通知を要求する
    /// </summary>
    public void RequestNotice()
    {
        isSend = false;
        reachAchievement.RequestUpdate();
    }
}
