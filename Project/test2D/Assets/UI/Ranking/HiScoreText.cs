using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HiScoreText : MonoBehaviour
{
    private TextMeshProUGUI m_Text = null;
    private PlayFabStatistics m_Statistics = null;
    void Start()
    {
        m_Statistics = GameObject.Find("PlayFabManager").GetComponent<PlayFabStatistics>();
        m_Text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (m_Statistics)
        {
            m_Text.text = string.Format("{0:0000}", m_Statistics.GetStatisticValue("SweetsPoint"));
        }
    }
}
