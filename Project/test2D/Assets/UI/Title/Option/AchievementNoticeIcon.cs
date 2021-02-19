using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementNoticeIcon : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] PlayFabStore achievementStore = default;
    private bool isInit = false;


    void Update()
    {
        // 初回値取得時にオブジェクトを非表示にするかを設定
        SetFirstActive();
    }
    void SetFirstActive()
    {
        if (playerData.isGet && achievementStore.m_isStoreGet &&!isInit)
        {
            gameObject.SetActive(false);

            foreach (var item in achievementStore.StoreItems)
            {
                UserDataRecord record = default;
                if (playerData.data.TryGetValue("NOTICE_"+item.ItemId, out record))
                {
                    // 通知アイコンが表示状態を見て表示
                    if (record.Value == "TRUE")
                    {
                        gameObject.SetActive(true);
                        break;
                    }
                }
            }

            isInit = true;
        }
    }
}
