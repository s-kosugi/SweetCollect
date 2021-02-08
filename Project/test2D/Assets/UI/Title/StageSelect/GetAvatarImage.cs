using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAvatarImage : MonoBehaviour
{
    [SerializeField] StageSelectParent parent = default;
    [SerializeField] PlayFabStore achievementStore = default;
    string oldDifficutName = default;
    Image image = default;


    void Start()
    {
        image = GetComponent<Image>();
    }


    void Update()
    {
        // 難易度が変更された時に画像を変更する
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
                // 報酬のアイテムの画像を読み込む
                image.sprite = Resources.Load<Sprite>("Player\\" + achievementDictionary[AchievementItemCustomDataKey.REWORD]);

                oldDifficutName = parent.difficultName;
            }
        }
    }
}
