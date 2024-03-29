﻿using UnityEngine;

// 広告
using UnityEngine.Advertisements;

/// <summary>
/// UnityAds広告表示クラス
/// </summary>
public class Ads : MonoBehaviour
{
    // 表示済みフラグ
    private bool IsShow = false;
    public bool isShow
    {
        get { return this.IsShow; }
    }

    void Start()
    {
        IsShow = false;
#if UNITY_ANDROID
        // 広告の初期化
        Advertisement.Initialize("3890947", true);
#endif
    }

    // 広告の表示
    public void ShowAds()
    {
#if UNITY_ANDROID
        if (Advertisement.IsReady())
        {
            //広告の表示
            Advertisement.Show();

            // 広告表示済みフラグ
            IsShow = true;
        }
        else
        {
            Debug.Log("Ads shows Failed");
        }
#endif
    }

    /// <summary>
    /// 広告表示中かどうか
    /// </summary>
    public bool isPlaying()
    {
#if UNITY_ANDROID
        return Advertisement.isShowing;
#else
        return false;
#endif
    }
}
