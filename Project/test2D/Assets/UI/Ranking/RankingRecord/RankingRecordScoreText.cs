using TMPro;
using UnityEngine;

public class RankingRecordScoreText : MonoBehaviour
{
    [SerializeField] RankingRecord rankingRecord = default;
    [SerializeField] TextMeshProUGUI textMesh = default;
    PlayFabLeaderBoard leaderBoard = default;
    bool isSet = false;


    void Update()
    {
        // 親からランキングレコードをもらう
        leaderBoard = rankingRecord.leaderBoard;
        if (!isSet && leaderBoard.isGet)
        {
            // スコアの取得
            textMesh.text = string.Format("{0:0000}",leaderBoard.entries[rankingRecord.rankPosition].StatValue);

            isSet = true;
        }
    }
}
