using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HiScoreText : MonoBehaviour
{
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
            m_Text.text = string.Format("{0:0000}", statistics.GetStatisticValue("SweetsPoint"));
            isSet = true;
        }
    }
}
