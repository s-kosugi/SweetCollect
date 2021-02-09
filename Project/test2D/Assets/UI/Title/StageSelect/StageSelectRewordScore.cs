using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageSelectRewordScore : MonoBehaviour
{
    [SerializeField] StageSelectParent parent = default;
    [SerializeField] PlayFabStore achievementStore = default;
    string oldDifficutName = default;
    TextMeshProUGUI textMesh = default;


    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        // 難易度が変更された時に得点を変更する
        if (parent.difficultName != oldDifficutName)
        {
            if (achievementStore.m_isCatalogGet)
            {
                string achievementName = default;

                // 難易度によってどの実績データを読み込むかを変更する
                switch (parent.difficultName)
                {
                    case DifficultName.EASY: achievementName = AchievementItemName.EASY_HISCORE; break;
                    case DifficultName.NORMAL: achievementName = AchievementItemName.NORMAL_HISCORE; break;
                    case DifficultName.HARD: achievementName = AchievementItemName.HARD_HISCORE; break;
                    case DifficultName.VERYHARD: achievementName = AchievementItemName.VERYHARD_HISCORE; break;
                }

                var catalogItem = achievementStore.CatalogItems.Find(x => x.ItemId == achievementName);

                // LitJsonを使ってJsonを連想配列化する
                var achievementDictionary = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(catalogItem.CustomData);
                // 報酬のアイテムの得点を読み込む
                int score = int.Parse(achievementDictionary[AchievementItemCustomDataKey.PROGRESS_MAX]);
                textMesh.text = StringWidthConverter.ConvertToFullWidth(string.Format("{0:000}", score));

                oldDifficutName = parent.difficultName;
            }
        }
    }
}
