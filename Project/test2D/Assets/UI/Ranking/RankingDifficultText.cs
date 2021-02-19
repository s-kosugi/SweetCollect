using TMPro;
using UnityEngine;

/// <summary>
/// ランキング名テキストクラス
/// </summary>
public class RankingDifficultText : MonoBehaviour
{
    [SerializeField] PlayFabLeaderBoard leaderBoard = default;
    [SerializeField] Color easyTextColor = default;
    [SerializeField] Color normalTextColor = default;
    [SerializeField] Color hardTextColor = default;
    [SerializeField] Color veryHardTextColor = default;
    TextMeshProUGUI textMesh = default;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        // 難易度毎で色とテキストを変更
        switch (leaderBoard.GetRankingName())
        {
            case RankingName.EASY: textMesh.text = DifficultHiraganaName.EASY;  textMesh.color = easyTextColor; break;
            case RankingName.NORMAL: textMesh.text = DifficultHiraganaName.NORMAL; textMesh.color = normalTextColor; break;
            case RankingName.HARD: textMesh.text = DifficultHiraganaName.HARD; textMesh.color = hardTextColor; break;
            case RankingName.VERYHARD: textMesh.text = DifficultHiraganaName.VERYHARD; textMesh.color = veryHardTextColor; break;
            default: textMesh.text = "？？？？？"; break;
        }
    }
}
