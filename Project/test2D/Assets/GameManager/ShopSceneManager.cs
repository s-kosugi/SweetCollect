using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSceneManager : BaseScene
{

    private PlayFabWaitConnect m_WaitConnect = null;

    private enum STATE
    {
        PREPARATION,
        FADEIN,
        MAIN,
        FADEOUT
    }
    private STATE m_State = STATE.PREPARATION;

    // Start is called before the first frame update
    override protected void Start()
    {
        SoundManager.Instance.PlayBGM("MainGame");
        GameObject PlayFabManager = GameObject.Find("PlayFabManager");
        m_WaitConnect = PlayFabManager.GetComponent<PlayFabWaitConnect>();
        NextSceneName = "TitleScene";
        base.Start();


        // 黒画面にする
        fadeState = FADE_STATE.BLACK;
    }

    // Update is called once per frame
    override protected void Update()
    {
        switch (m_State)
        {
            case STATE.PREPARATION: Preparation(); break;
            case STATE.FADEIN: ShopFadeIn(); break;
            case STATE.FADEOUT: ShopFadeOut(); break;
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
    private void ShopFadeIn()
    {
        if (IsFadeEnd())
        {
            m_State = STATE.MAIN;
        }
    }
    // ショップメイン状態
    private void Main()
    {
    }
    // フェードアウト中
    private void ShopFadeOut()
    {
    }

    // 次のシーンへ
    public void NextScene()
    {
        // 通信待ちでない場合
        if (!m_WaitConnect.IsWait())
        {
            // フェードアウト状態にする
            fadeState = FADE_STATE.FADEOUT;
            m_State = STATE.FADEOUT;

        }
    }
}
