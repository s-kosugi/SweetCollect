﻿using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ランキングのアバタークラス
/// </summary>
public class RankingAvatar : MonoBehaviour
{
    [SerializeField] RankingRecord rankingRecord = default;
    [SerializeField] Image image = default;
    bool isSet = false;

    void Update()
    {
        if (!isSet)
        {
            // データ取得完了していたらスプライトをロードする
            if (rankingRecord.playerData != default && rankingRecord.playerData.isGet)
            {
                UserDataRecord record = default;
                // ランキングのレコードからプレイヤーの装着している服を取得する
                if (rankingRecord.playerData.data.TryGetValue(PlayerDataName.ECLOTHES, out record))
                {
                    image.sprite = Resources.Load<Sprite>("Player\\" + record.Value);

                    if( image.sprite == null) image.sprite = Resources.Load<Sprite>("Player\\001_NORMAL");
                }

                isSet = true;
            }
        }
    }
}
