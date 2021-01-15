using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class HiScoreText : MonoBehaviour
{
    [SerializeField] PlayFabPlayerData playerData = default;
    private TextMeshProUGUI m_Text = null;
    private PlayFabStatistics statistics = null;
    private bool isSet = false;
    void Start()
    {
        statistics = GameObject.Find("PlayFabManager").transform.Find("PlayFabStatistics").GetComponent<PlayFabStatistics>();
        m_Text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (statistics.isGet && !isSet)
        {
            string rankingName = default;
            UserDataRecord record = default;
            if (playerData.m_Data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
            {
                switch (record.Value)
                {
                    case DifficultName.EASY: rankingName = RankingName.EASY; break;
                    case DifficultName.NORMAL: rankingName = RankingName.NORMAL; break;
                    case DifficultName.HARD: rankingName = RankingName.HARD; break;
                    case DifficultName.VERYHARD: rankingName = RankingName.VERYHARD; break;
                }
                m_Text.text = string.Format("{0:0000}", statistics.GetStatisticValue(rankingName));
            }
            isSet = true;
        }
    }
}
