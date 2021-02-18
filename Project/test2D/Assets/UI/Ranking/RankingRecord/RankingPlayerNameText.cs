using TMPro;
using UnityEngine;

/// <summary>
/// ランキングのプレイヤー名テキストクラス
/// </summary>
public class RankingPlayerNameText : MonoBehaviour
{
    [SerializeField] RankingRecord rankingRecord = default;
    [SerializeField] TextMeshProUGUI textMesh = default;
    PlayFabLeaderBoard leaderBoard = default;
    bool isSet = false;


    void Update()
    {
        // 親からランキングレコードをもらう
        leaderBoard = rankingRecord.leaderBoard;
        if (leaderBoard != default)
        {
            if (!isSet && leaderBoard.isGet && rankingRecord.rankPosition != -1)
            {
                int position = rankingRecord.rankPosition;

                // 自身から数えたランキングだったら0番目を参照する
                if (leaderBoard.GetSelfCount())
                    position = 0;

                // プレイヤー名の取得
                textMesh.text = leaderBoard.entries[position].DisplayName;

                isSet = true;
            }
        }
    }
}
