using TMPro;
using UnityEngine;

/// <summary>
/// 実績名テキストクラス
/// </summary>
public class AchievementTitleText : MonoBehaviour
{
    [SerializeField] AchievementParent achievementParent = default;
    [SerializeField] TextMeshProUGUI textmesh = default;


    void Update()
    {
        // 開放済みかどうかで表示名を変更する
        if (achievementParent.isNowAchievementReach)
            textmesh.text = achievementParent.descriptAchievementName;
        else
            textmesh.text = "？？？？？";
    }
}
