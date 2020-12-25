using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetPointNumber : MonoBehaviour
{
    private ScoreManager m_ScoreManager = null;
    private TextMeshProUGUI m_Text = null;

    void Start()
    {
        m_ScoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        m_Text = gameObject.GetComponent<TextMeshProUGUI>();
        m_Text.text = "    = " + string.Format("{0:00000}", m_ScoreManager.GetCoinScore());
        
    }

    void Update()
    {
        
    }
}
