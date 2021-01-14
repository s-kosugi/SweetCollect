using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingDifficultText : MonoBehaviour
{
    [SerializeField] PlayFabLeaderBoard leaderBoard = default;
    [SerializeField] string easyText = "かんたん";
    [SerializeField] Color easyTextColor = default;
    [SerializeField] string normalText = "ふつう";
    [SerializeField] Color normalTextColor = default;
    [SerializeField] string hardTexr = "むずかしい";
    [SerializeField] Color hardTextColor = default;
    [SerializeField] string veryHardText = "えくすとら";
    [SerializeField] Color veryHardTextColor = default;
    TextMeshProUGUI textMesh = default;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // 難易度毎で色とテキストを変更
        switch (leaderBoard.GetRankingName())
        {
            case RankingName.EASY: textMesh.text = easyText;  textMesh.color = easyTextColor; break;
            case RankingName.NORMAL: textMesh.text = normalText; textMesh.color = normalTextColor; break;
            case RankingName.HARD: textMesh.text = hardTexr; textMesh.color = hardTextColor; break;
            case RankingName.VERYHARD: textMesh.text = veryHardText; textMesh.color = veryHardTextColor; break;
            default: textMesh.text = "？？？？？"; break;
        }
    }
}
