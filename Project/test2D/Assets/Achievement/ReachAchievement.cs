using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 実績達成確認クラス
/// </summary>
public class ReachAchievement : MonoBehaviour
{
    [SerializeField] PlayFabStore achievementStore = default;
    [SerializeField] PlayFabPlayerData playerData = default;
    private Dictionary<string, ReachAchievementInfo> reachDictionary = new Dictionary<string, ReachAchievementInfo>();
    public bool isSet { get; private set; } = false;

    /// <summary>
    /// 実績情報
    /// </summary>
    public struct ReachAchievementInfo
    {
        public bool reach;
        public int progressMax;
        public int progressValue;
    }
    void Update()
    {
        // 実績を達成したかどうか一度だけチェックする
        if (!isSet)
        {
            CheckReachAchievement();
        }
    }

    /// <summary>
    /// ストアから条件を満たしているかをチェックする
    /// </summary>
    private void CheckReachAchievement()
    {
        // 条件に関するデータが取得済みかどうか
        if (achievementStore.m_isCatalogGet && achievementStore.m_isStoreGet && playerData.m_isGet)
        {
            // チェック前に一度クリアする
            reachDictionary.Clear();

            for (int i = 0; i < achievementStore.StoreItems.Count; i++)
            {
                // カタログと一致するアイテムの取得
                var catalogItem = achievementStore.CatalogItems.Find(x => x.ItemId == achievementStore.StoreItems[i].ItemId);

                // LitJsonを使ってJsonを連想配列化する
                var jsonDic = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(catalogItem.CustomData);

                //--------------------------------------------------------------------------------
                // 進捗度をセット
                UserDataRecord playerRecord;
                string progressString = "0";
                // 実績内のカスタムデータからキーを取得してプレイヤーデータにアクセスする
                if (playerData.m_Data.TryGetValue(jsonDic[AchievementDataName.PROGRESS_KEY], out playerRecord))
                {
                    double num;
                    // 進捗度が数値ではなかった場合は実績内の該当キーと一致しているかで判断をする
                    if (!double.TryParse(playerRecord.Value, out num))
                    {
                        string achievementValue;
                        // 実績内のプレイヤーデータを持つキーとプレイヤーデータが一致したら達成済み(1)とする
                        if (jsonDic.TryGetValue(jsonDic[AchievementDataName.PROGRESS_KEY], out achievementValue))
                        {
                            if (playerRecord.Value == achievementValue) progressString = "1";
                        }
                    }
                    else
                    {
                        // 数値データだったのでそのまま格納する
                        progressString = playerRecord.Value;
                    }
                }

                // 実績情報の作成
                ReachAchievementInfo info = default;
                info.progressMax = int.Parse(jsonDic[AchievementDataName.PROGRESS_MAX]);
                info.progressValue = int.Parse(progressString);

                // 実績達成済みならtrueを入れる
                if (info.progressValue >= info.progressMax) info.reach = true;
                else info.reach = false;

                reachDictionary.Add(achievementStore.StoreItems[i].ItemId,info);
            }
            isSet = true;
        }
    }

    /// <summary>
    /// 実績達成済みかどうか
    /// </summary>
    /// <param name="achievementID">実績アイテムID</param>
    /// <returns>true 条件達成済み　false 条件未達成又は未取得</returns>
    public bool IsReachAchievement(string achievementID)
    {
        bool ret = false;
        if (isSet)
        {
            ret = reachDictionary[achievementID].reach;
        }

        return ret;
    }

    /// <summary>
    /// 実績解放情報の取得
    /// </summary>
    /// <param name="achievementID">実績アイテムID</param>
    /// <returns>見つからない又は未取得はdefault</returns>
    public ReachAchievementInfo GetInfo(string achievementID)
    {
        ReachAchievementInfo info = default;
        if (isSet)
        {
            if (!reachDictionary.TryGetValue(achievementID, out info))
            {
                info = default;
            }
        }

        return info;
    }

    /// <summary>
    /// 実績達成したかどうかを更新しなおす
    /// </summary>
    public void RequestUpdate()
    {
        isSet = false;
    }
}
