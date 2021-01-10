using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingPositionText : MonoBehaviour
{
    [SerializeField] RankingRecord rankingRecord = default;
    [SerializeField] TextMeshProUGUI textMesh = default;

    void Start()
    {
        // ランキング順位を表示する
        textMesh.text = (rankingRecord.rankPosition + 1).ToString();
    }

}
