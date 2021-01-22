using PlayFab.ClientModels;
using UnityEngine;

public class HiScoreSend : MonoBehaviour
{
    [SerializeField] PlayFabStatistics statistics = default;
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] NoticeAchievement noticeAchievement = default;
    private ScoreManager scoreManager = default;
    public bool isPlayfabConnectEnd { get; private set; } = false;
    public bool isUpdate { get; private set; } = false;

    void Start()
    {
        // ゲームメインシーンから引き継いだScoreManagerを取得する
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayfabConnectEnd)
        {
            if (statistics.isGet && scoreManager && playerData.m_isGet)
            {
                string selectDifficult = default;
                string rankingName = default;
                string updatePlayerDataName = default;

                UserDataRecord record = default;
                // 現在遊んでいる難易度の取得
                if (playerData.m_Data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
                {
                    selectDifficult = record.Value;
                }

                // 難易度に合わせたランキング名と更新用のプレイヤーデータ名を設定する
                switch (selectDifficult)
                {
                    case DifficultName.EASY: rankingName = RankingName.EASY; updatePlayerDataName = PlayerDataName.HISCORE_EASY; break;
                    case DifficultName.NORMAL: rankingName = RankingName.NORMAL; updatePlayerDataName = PlayerDataName.HISCORE_NORMAL; break;
                    case DifficultName.HARD: rankingName = RankingName.HARD; updatePlayerDataName = PlayerDataName.HISCORE_HARD; break;
                    case DifficultName.VERYHARD: rankingName = RankingName.VERYHARD; updatePlayerDataName = PlayerDataName.HISCORE_VERYHARD; break;
                    default: break;
                }

                // 難易度選択が正しかった場合
                if (selectDifficult != default)
                {
                    // 統計情報のスコアを取得
                    int staValue = statistics.GetStatisticValue(rankingName);

                    // ハイスコア更新で、統計情報が見つからなかった場合は既定値が返るので多分OK
                    if (staValue < scoreManager.GameScore)
                    {
                        Debug.Log("UpdateStatistics");

                        // ハイスコアを更新する
                        statistics.UpdatePlayerStatistics(rankingName, scoreManager.GameScore);

                        // 実績参照用のスコアをプレイヤーデータへ保存しておく
                        playerData.SetPlayerData(updatePlayerDataName, scoreManager.GameScore.ToString());

                        // 実績通知を要求する
                        noticeAchievement.RequestNotice();

                        // ハイスコア更新済みフラグ
                        isUpdate = true;
                    }
                }
                isPlayfabConnectEnd = true;
            }
        }
    }
}
