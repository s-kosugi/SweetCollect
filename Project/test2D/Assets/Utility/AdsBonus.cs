using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsBonus : MonoBehaviour
{
    private Ads m_Ads = null;
    private PlayFabVirtualCurrency m_PlayFabVirtualCurrency = null;
    private ScoreManager m_ScoreManager = null;
    private bool isAdd = false;     // ボーナス加算済みかどうか

    void Start()
    {
        m_Ads = gameObject.GetComponent<Ads>();
        m_PlayFabVirtualCurrency = GameObject.Find("PlayFabVirtualCurrency").GetComponent<PlayFabVirtualCurrency>();
        m_ScoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        isAdd = false;
    }

    void Update()
    {
        // 広告表示済みなら倍率をかける
        if (m_Ads && m_Ads.isShow && m_ScoreManager && !isAdd)
        {
            Debug.Log("Add:AdsBonus");
            if ( m_Ads.rate > 1.0f)
                // 仮想通貨を倍率文のみ加算する(ゲームオーバー時に等倍は加算済み)
                m_PlayFabVirtualCurrency.AddUserVirtualCurrency("HA", (int)((float)m_ScoreManager.GetScore() * m_Ads.rate - m_ScoreManager.GetScore()));
            isAdd = true;
        }
    }

}
