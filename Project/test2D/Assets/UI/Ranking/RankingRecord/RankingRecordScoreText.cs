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
        // 親からリーダーボードをもらう
        leaderBoard = rankingRecord.leaderBoard;
        if (!isSet && leaderBoard.isGet && rankingRecord.rankPosition != -1)
        {
            int position = rankingRecord.rankPosition;

            // 自身から数えたランキングだったら0番目を参照する
            if (leaderBoard.GetSelfCount())
                position = 0;

            // スコアの取得
            textMesh.text = string.Format("{0:0000}", leaderBoard.entries[position].StatValue);

            isSet = true;
        }
        else if( !isSet )
        {
            // とりあえず取得できていないので表示を0にする
            textMesh.text = string.Format("{0:0000}", 0);
        }
    }
}
