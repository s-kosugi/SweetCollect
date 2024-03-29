﻿using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// ツイッターボタンクラス
/// </summary>
public class TwitterButton : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    // Assets/Plugins/WebGLにあるOpenNewTabPluginをインポートする
    [System.Runtime.InteropServices.DllImport("__Internal")] private static extern void OpenNewTab(string URL);
#endif

    private ScoreManager scoreManager = default;
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

    }
    /// <summary>
    /// ボタン押下処理
    /// </summary>
    public void PushButton()
    {
        //urlの作成
        string esctext = UnityWebRequest.EscapeURL("今回のスコアは\n"+scoreManager.GetCoinScore()+" だよ\nhttps://twitter.com/SweetCollectDev");
        string esctag = UnityWebRequest.EscapeURL("スイートコレクト");
        string url = "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;

        // Twitter投稿画面の起動
#if UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_WEBGL
        OpenNewTab(url);
#else
        Application.OpenURL(url);
#endif
    }
}
