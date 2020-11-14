using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetPointNumber : MonoBehaviour
{
    private ScoreManager m_ScoreManager = null;
    private Ads m_Ads = null;
    private TextMeshProUGUI m_Text = null;
    // Start is called before the first frame update
    void Start()
    {
        m_ScoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        m_Ads = GameObject.Find("AdsManager").GetComponent<Ads>();
        m_Text = gameObject.GetComponent<TextMeshProUGUI>();
        // 広告が表示済みなら3倍表示
        if (m_Ads.isShow)
        {
            m_Text.text = "×" + m_Ads.rate + " = " + string.Format("{0:00000}", m_ScoreManager.GetScore() * m_Ads.rate);
        }
        else
        {
            m_Text.text = "    = " + string.Format("{0:00000}", m_ScoreManager.GetScore());
        }
        
    }

    void Update()
    {
        
    }
}
