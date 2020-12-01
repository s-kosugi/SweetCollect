﻿using UnityEngine;
using UnityEngine.UI;

public class TitleManager : BaseScene
{
    [SerializeField] GameObject textBox = null;
    private PlayFabPlayerData m_PlayFabEClothesData = null;
    private PlayFabUserProfiel m_PlayFabUserProfiel = null;
    private PlayFabPlayerData m_PlayFabTutorialData = null;
    private PlayFabWaitConnect m_WaitConnect = null;

    private enum STATE
    {
        PREPARATION,
        FADEIN,
        MAIN,
        FADEOUT
    }
    private STATE m_State = STATE.PREPARATION;

    override protected void Start()
    {
        SoundManager.Instance.PlayBGM("MainGame");

        GameObject PlayFabManager = GameObject.Find("PlayFabManager");
        m_PlayFabEClothesData = PlayFabManager.transform.Find("PlayFabEclothesData").GetComponent<PlayFabPlayerData>();
        m_PlayFabUserProfiel = PlayFabManager.transform.Find("PlayFabUserProfiel").GetComponent<PlayFabUserProfiel>();
        m_PlayFabTutorialData = PlayFabManager.transform.Find("PlayFabTutorialData").GetComponent<PlayFabPlayerData>();
        m_WaitConnect = PlayFabManager.GetComponent<PlayFabWaitConnect>();
        NextSceneName = "GameMainScene";

        base.Start();


        // 黒画面にする
        fadeState = FADE_STATE.BLACK;
    }

    override protected void Update()
    {
        switch (m_State)
        {
            case STATE.PREPARATION: Preparation(); break;
            case STATE.FADEIN: TitleFadeIn(); break;
            case STATE.FADEOUT: TitleFadeOut(); break;
            case STATE.MAIN: Main(); break;
        }
        base.Update();
    }

    // 準備
    private void Preparation()
    {
        if (!m_WaitConnect.IsWait())
        {
            m_State = STATE.FADEIN;
            fadeState = FADE_STATE.FADEIN;
        }
    }
    // フェードイン中
    private void TitleFadeIn()
    {
        if(IsFadeEnd())
        {
            m_State = STATE.MAIN;
        }
    }
    // タイトルメイン状態
    private void Main()
    {
    }
    // フェードアウト中
    private void TitleFadeOut()
    {
    }

    // 次のシーンへ
    public void NextScene()
    {
        // 通信待ちでない場合
        if (!m_WaitConnect.IsWait())
        {
            // テキストボックスが空白の場合は次のシーンへいかない
            if (textBox.GetComponent<InputField>().text != "" && fadeState != FADE_STATE.FADEOUT)
            {
                // ユーザー名の更新
                m_PlayFabUserProfiel.SetUserName(textBox.GetComponent<InputField>().text);

                // ユーザーデータを取得できていなかったらデフォルトデータを設定しておく
                if (!m_PlayFabEClothesData.m_isGet)
                {
                    m_PlayFabEClothesData.SetPlayerData("001_NORAML");
                }

                // フェードアウト状態にする
                fadeState = FADE_STATE.FADEOUT;
                m_State = STATE.FADEOUT;
            }
        }
    }
    // シーン名を指定してシーン変更
    public void NextScene(string sceneName)
    {
        NextSceneName = sceneName;
        // チュートリアル終了済みでなかったらチュートリアルへ飛ばす
        if (NextSceneName == "GameMainScene")
        {
            if (m_PlayFabTutorialData.m_Value != "End")
            {
                NextSceneName = "TutorialScene";
            }
        }
        NextScene();
    }
}
