﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementName : MonoBehaviour
{
    [SerializeField] AchievementEquipFrame achievementequipframe = null;        //表示フレーム
    [SerializeField] TextMeshProUGUI textmesh = default;                        //テキスト

    private void Awake()
    {
        textmesh.text = "?????????????????????????????";
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    //===========================================================================================================
    //設定関連
    //実績名称の取得
    //name : 実績名称の名前
    public void GetAchievementName(string name)
    {
        textmesh.text = name;
        achievementequipframe.StartAppear();
    }
    //===========================================================================================================
    //===========================================================================================================

}
