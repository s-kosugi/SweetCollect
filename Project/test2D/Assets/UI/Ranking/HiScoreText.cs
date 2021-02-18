using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class HiScoreText : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    [SerializeField] TextMeshProUGUI text = default;
    [SerializeField] PlayFabStatistics statistics = default;
    public bool isSet { get; private set; } = false;
    public int hiScore { get; private set; }
    void Start()
    {
    }

    void Update()
    {
        if (statistics.isGet && !isSet && playerData.isGet)
        {
            string rankingName = default;
            UserDataRecord record = default;
            if (playerData.data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
            {
                switch (record.Value)
                {
                    case DifficultName.EASY: rankingName = RankingName.EASY; break;
                    case DifficultName.NORMAL: rankingName = RankingName.NORMAL; break;
                    case DifficultName.HARD: rankingName = RankingName.HARD; break;
                    case DifficultName.VERYHARD: rankingName = RankingName.VERYHARD; break;
                }
                hiScore = statistics.GetStatisticValue(rankingName);
                text.text = string.Format("{0:0000}", hiScore);
            }
            isSet = true;
        }
    }
}
