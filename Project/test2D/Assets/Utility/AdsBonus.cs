using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsBonus : MonoBehaviour
{
    private Ads m_Ads = null;
    [SerializeField] GameMainManager gameMainManager = null;
    [SerializeField] float AddTime = 30f;
    private bool isAdd = false;     // ボーナス加算済みかどうか

    void Start()
    {
        m_Ads = gameObject.GetComponent<Ads>();
        isAdd = false;
    }

    void Update()
    {
        // 広告表示済みなら時間を延長させる
        if (m_Ads && m_Ads.isShow  && !isAdd)
        {
            Debug.Log("Add:AdsBonus");
            gameMainManager.state = GameMainManager.STATE.PRERESTART;
            gameMainManager.GameTimer = AddTime;
            //if ( m_Ads.rate > 1.0f)
            //    // 仮想通貨を倍率文のみ加算する(ゲームオーバー時に等倍は加算済み)
            //    m_PlayFabVirtualCurrency.AddUserVirtualCurrency("HA", (int)((float)(m_ScoreManager.GetScore()) * m_Ads.rate - (float)(m_ScoreManager.GetScore())));
            isAdd = true;
        }
    }

}
