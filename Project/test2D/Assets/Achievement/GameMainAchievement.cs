using PlayFab.ClientModels;
using UnityEngine;

/// <summary>
/// ゲームメイン実績送信クラス
/// </summary>
public class GameMainAchievement : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] PlayerController player = default;
    [SerializeField] NoticeAchievement notice = default;
    [SerializeField] ScoreManager scoreManager = default;

    /// <summary>
    /// 実績カウント送信
    /// </summary>
    public void SendAchievementCount()
    {
        // ジャンプ回数
        AddAchievementCount(PlayerDataName.COUNT_JUMP, player.jumpCount);
        // お菓子ゲット数
        AddAchievementCount(PlayerDataName.COUNT_GETSWEET, player.sweetGetCount);
        // プレイ回数
        AddAchievementCount(PlayerDataName.COUNT_PLAYED, 1);
        // コイン入手総数
        AddAchievementCount(PlayerDataName.COUNT_GETCOIN, scoreManager.GetCoinScore());

        // 実績通知を要求する
        notice.RequestNotice();
    }

    /// <summary>
    /// 実績カウントを足す
    /// </summary>
    private void AddAchievementCount(string achievementName, int count)
    {
        UserDataRecord record = default;
        int result = 0;
        if (playerData.data.TryGetValue(achievementName, out record))
        {
            result += int.Parse(record.Value);
        }

        result += count;

        // 足した結果をPlayFabへ送信する
        playerData.SetPlayerData(achievementName, result.ToString());
    }
}
