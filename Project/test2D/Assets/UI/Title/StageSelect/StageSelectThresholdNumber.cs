using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageSelectThresholdNumber : MonoBehaviour
{
    [SerializeField] PlayFabTitleData titleData = default;
    [SerializeField] StageSelectParent parent = default;
    TextMeshProUGUI textmesh = default;
    string oldDifficultName = default;

    void Start()
    {
        textmesh = GetComponent<TextMeshProUGUI>();
        textmesh.text = StringWidthConverter.ConvertToFullWidth(string.Format("{0:0000}", 0).ToUpper());
    }


    void Update()
    {
        // プレイヤーデータ取得済み且つ難易度が変更されていた場合読み込む
        if (titleData.isGet && oldDifficultName != parent.difficultName)
        {
            string titleDataName = default;
            switch (parent.difficultName)
            {
                case DifficultName.EASY: titleDataName = TitleDataName.RELEASE_THRESHOLD_NORMAL; break;
                case DifficultName.NORMAL: titleDataName = TitleDataName.RELEASE_THRESHOLD_HARD; break;
                case DifficultName.HARD: titleDataName = TitleDataName.RELEASE_THRESHOLD_VERYHARD; break;
                default: break;
            }
            if (titleDataName == default)
            {
                textmesh.text = "";
                oldDifficultName = parent.difficultName;
            }
            else
            {
                // タイトルデータから解放閾値を読み込む
                int Threshold = int.Parse(titleData.titleData[titleDataName]);
                {
                    // 全角でテキストに入れる
                    textmesh.text = StringWidthConverter.ConvertToFullWidth(string.Format("{0:0000}", Threshold));

                    oldDifficultName = parent.difficultName;
                }
            }
        }
    }
}
