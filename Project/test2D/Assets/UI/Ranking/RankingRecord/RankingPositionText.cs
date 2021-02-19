using TMPro;
using UnityEngine;

/// <summary>
/// ランキング順位テキストクラス
/// </summary>
public class RankingPositionText : MonoBehaviour
{
    [SerializeField] RankingRecord rankingRecord = default;
    [SerializeField] TextMeshProUGUI textMesh = default;

    void Update()
    {
        // ランキング順位を表示する
        textMesh.text = (rankingRecord.rankPosition + 1).ToString();
    }

}
