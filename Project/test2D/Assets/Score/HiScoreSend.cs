using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HiScoreSend : MonoBehaviour
{
    private PlayFabStatistics statistics = default;
    private ScoreManager scoreManager = default;
    public bool isPlayfabConnectEnd { get; private set; } = false;
    public bool isUpdate { get; private set; } = false;

    void Start()
    {
        statistics = GameObject.Find("PlayFabStatistics").GetComponent<PlayFabStatistics>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayfabConnectEnd)
        {
            if (statistics.isGet && scoreManager)
            {
                int staValue = statistics.GetStatisticValue("SweetsPoint");
                // ハイスコア更新で、統計情報が見つからなかった場合は既定値が返るので多分OK
                if (staValue < scoreManager.GetScore())
                {
                    Debug.Log("UpdateStatistics");
                    // ハイスコアを更新する
                    statistics.UpdatePlayerStatistics("SweetsPoint", scoreManager.GetScore());

                    // ハイスコア更新済みフラグ
                    isUpdate = true;
                }
                isPlayfabConnectEnd = true;
            }
        }
    }
}
