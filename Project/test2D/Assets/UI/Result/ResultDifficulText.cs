using TMPro;
using UnityEngine;

public class ResultDifficulText : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] Color easyTextColor = default;
    [SerializeField] Color normalTextColor = default;
    [SerializeField] Color hardTextColor = default;
    [SerializeField] Color veryHardTextColor = default;
    TextMeshProUGUI textMesh = default;
    bool isSet = false;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSet && playerData.isGet)
        {
            // 難易度毎で色とテキストを変更
            switch (playerData.data[PlayerDataName.SELECTED_DIFFICULT].Value)
            {
                case DifficultName.EASY: textMesh.text = DifficultHiraganaName.EASY; textMesh.color = easyTextColor; break;
                case DifficultName.NORMAL: textMesh.text = DifficultHiraganaName.NORMAL; textMesh.color = normalTextColor; break;
                case DifficultName.HARD: textMesh.text = DifficultHiraganaName.HARD; textMesh.color = hardTextColor; break;
                case DifficultName.VERYHARD: textMesh.text = DifficultHiraganaName.VERYHARD; textMesh.color = veryHardTextColor; break;
                default: textMesh.text = "？？？？？"; break;
            }

            isSet = true;
        }
    }
}
