using PlayFab.ClientModels;
using UnityEngine;

public class GameMainAchievement : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] PlayerController player = default;
    [SerializeField] NoticeAchievement notice = default;

    /// <summary>
    /// 実績カウント送信
    /// </summary>
    public void SendAchievementCount()
    {
        if (player != default)
        {
            // ジャンプ回数
            AddAchievementCount(PlayerDataName.COUNT_JUMP, player.jumpCount);
            // お菓子ゲット数
            AddAchievementCount(PlayerDataName.COUNT_GETSWEET, player.sweetGetCount);
        }
    }

    /// <summary>
    /// 実績カウントを足す
    /// </summary>
    private void AddAchievementCount(string achievementName, int count)
    {
        UserDataRecord record = default;
        int result = 0;
        if (playerData.m_Data.TryGetValue(achievementName, out record))
        {
            result += int.Parse(record.Value);
        }

        result += count;

        // 足した結果をPlayFabへ送信する
        playerData.SetPlayerData(achievementName, result.ToString());

        // 実績通知を要求する
        notice.RequestNotice();
    }
}
