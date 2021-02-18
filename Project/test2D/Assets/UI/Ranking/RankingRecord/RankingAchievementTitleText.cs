using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class RankingAchievementTitleText : MonoBehaviour
{
    [SerializeField] RankingRecord rankingRecord = default;
    [SerializeField] TextMeshProUGUI textMesh = default;
    bool isSet = false;

    void Update()
    {
        if (!isSet)
        {
            // データ取得完了していたら実績名を取得する
            if (rankingRecord.playerData != default && rankingRecord.playerData.isGet)
            {
                UserDataRecord userData;

                if (rankingRecord.store.m_isCatalogGet)
                {
                    if (rankingRecord.playerData.data.TryGetValue(PlayerDataName.ACHIEVEMENT_SELECT, out userData))
                    {
                        var catalogItem = rankingRecord.store.CatalogItems.Find(x => x.ItemId == userData.Value);
                        if (catalogItem != null)
                            textMesh.text = catalogItem.DisplayName;
                        else
                            textMesh.text = default;
                    }
                    else
                    {
                        textMesh.text = default;
                    }

                    isSet = true;
                }
            }
        }
    }
}
