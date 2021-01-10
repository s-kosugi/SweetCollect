using TMPro;
using UnityEngine;

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
        if (!isSet && leaderBoard.isGet)
        {
            Debug.Log(leaderBoard.entries[rankingRecord.rankPosition].DisplayName);
            // プレイヤー名の取得
            textMesh.text = leaderBoard.entries[rankingRecord.rankPosition].DisplayName;

            isSet = true;
        }
    }
}
