using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TitleManager : BaseScene
{
    [SerializeField] GameObject textBox = null;
    private PlayFabPlayerData m_PlayFabPlayerData = null;
    private PlayFabUserProfiel m_PlayFabUserProfiel = null;
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
        m_PlayFabPlayerData = PlayFabManager.transform.Find("PlayFabEclothesData").GetComponent<PlayFabPlayerData>();
        m_PlayFabUserProfiel = PlayFabManager.transform.Find("PlayFabUserProfiel").GetComponent<PlayFabUserProfiel>();
        m_WaitConnect = PlayFabManager.GetComponent<PlayFabWaitConnect>();
        NextSceneName = "GameMainScene";

        base.Start();


        // フェードイン状態にする
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
                if (!m_PlayFabPlayerData.m_isGet)
                {
                    m_PlayFabPlayerData.SetPlayerData("001_NORAML");
                }

                // フェードアウト状態にする
                fadeState = FADE_STATE.FADEOUT;
                m_State = STATE.FADEOUT;
            }
        }
    }
}
