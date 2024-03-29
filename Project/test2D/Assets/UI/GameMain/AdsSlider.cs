﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 広告用スライダー制御クラス
/// </summary>
public class AdsSlider : MonoBehaviour
{
    [SerializeField] GameMainManager mainManager = default;
    [SerializeField] Ads ads = default;
    Slider slider = default;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!ads.isShow)
        {
            slider.value = 1.0f - mainManager.AndroidAutoAdsCount / mainManager.AndroidAutoAdsTime;
        }
        else
        {
            slider.value = 1.0f - mainManager.AndroidAutoSceneMoveCount / mainManager.AndroidAutoSceneMoveTime;
        }
    }
}
