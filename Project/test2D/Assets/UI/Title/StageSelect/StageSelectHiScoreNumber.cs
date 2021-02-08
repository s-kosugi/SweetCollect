using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageSelectHiScoreNumber : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
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
        if (playerData.m_isGet && oldDifficultName != parent.difficultName)
        {
            UserDataRecord record = default;
            // 各難易度を読み込む
            if (playerData.m_Data.TryGetValue("HISCORE_" + parent.difficultName, out record))
            {
                int hiScore = int.Parse(record.Value);
                // 全角でテキストに入れる
                textmesh.text = StringWidthConverter.ConvertToFullWidth(string.Format("{0:0000}", hiScore));

                oldDifficultName = parent.difficultName;
            }
        }
    }
}
