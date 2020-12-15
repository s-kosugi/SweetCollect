﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsBonus : MonoBehaviour
{
    private Ads m_Ads = null;
    [SerializeField] GameMainManager gameMainManager = null;
    [SerializeField] float AddTime = 30f;
    public bool isAdd { get; private set; } = false;     // ボーナス加算済みかどうか

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
            gameMainManager.AddGameTime(AddTime);
            isAdd = true;
        }
    }

}
