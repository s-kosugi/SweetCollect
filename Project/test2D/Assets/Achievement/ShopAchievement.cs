﻿using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAchievement : MonoBehaviour
{
    [SerializeField] PlayFabInventory inventory = default;
    [SerializeField] PlayFabPlayerData playerData = default;
    public bool isHaveClothesCount { get; set; } = false;       // 服の所持数をカウント済みかどうか

    void Update()
    {
        // 服の所持数の送信
        SendHaveClothesCount();

    }

    /// <summary>
    /// 服の所持数の送信
    /// </summary>
    private void SendHaveClothesCount()
    {
        // アイテムを数える
        if (inventory.m_isGet && playerData.m_isGet && !isHaveClothesCount)
        {
            int count = inventory.CountItemsCategory("clothes");
            UserDataRecord record = default;
            isHaveClothesCount = true;

            if (playerData.m_Data.TryGetValue(PlayerDataName.COUNT_HAVECLOTHES, out record))
            {
                // 現在の数と同数だったら送信しない
                if (int.Parse(record.Value) == count) return;
            }

            // 服の所持数を送信する
            playerData.SetPlayerData(PlayerDataName.COUNT_HAVECLOTHES, count.ToString());
        }
    }
}
