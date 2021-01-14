﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ランキングでのレコードを表示する為の親クラスプレイヤーデータの情報とリーダーボードを保持しておく
/// </summary>
public class RankingRecord : MonoBehaviour
{
    public PlayFabLeaderBoard leaderBoard { get; private set; } = default;
    GameObject leaderBoardObject = default;
    public PlayFabStore store { get; private set; } = default;

    private GameObject playerDataObj = null;
    public PlayFabPlayerData playerData { get; private set; } = default;

    /// <summary>
    /// 現在の参照する順位
    /// </summary>
    public int rankPosition { get; set; } = -1;

    void Start()
    {
        leaderBoardObject = GameObject.Find("PlayFabManager/PlayFabLeaderBoard");
        leaderBoard = leaderBoardObject.GetComponent<PlayFabLeaderBoard>();
        store = GameObject.Find("PlayFabManager/PlayFabStore").GetComponent<PlayFabStore>();
    }


    void Update()
    {
        // プレイヤーデータのオブジェクトが作成されたら取得する
        if (playerDataObj == null)
        {
            Transform trs = leaderBoardObject.transform.Find("PlayFabPlayerDataRank" + rankPosition);
            if( trs != null)  playerDataObj = trs.gameObject;
        }
        else
        {
            // プレイヤーデータを取得する
            if (playerData == default) playerData = playerDataObj.GetComponent<PlayFabPlayerData>();
        }
    }
}