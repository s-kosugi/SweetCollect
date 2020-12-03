﻿using UnityEngine;

public class CreditSceneManager : BaseScene
{
    public enum STATE
    {
        FADEIN,
        MAIN,
        FADEOUT
    }
    private STATE m_State = STATE.FADEIN;

    override protected void Start()
    {
        base.Start();


        // フェードインにする
        fadeState = FADE_STATE.FADEIN;
    }

    override protected void Update()
    {
        switch (m_State)
        {
            case STATE.FADEIN: CreditFadeIn(); break;
            case STATE.FADEOUT: CreditFadeOut(); break;
            case STATE.MAIN: Main(); break;
        }
        base.Update();
    }
    // フェードイン中
    private void CreditFadeIn()
    {
        if (IsFadeEnd())
        {
            m_State = STATE.MAIN;
        }
    }
    // メイン状態
    private void Main()
    {
        
     
    }
    // フェードアウト中
    private void CreditFadeOut()
    {

    }

    public void Push_CreditButton()
    {
        // フェードアウト状態にする
        fadeState = FADE_STATE.FADEOUT;
        m_State = STATE.FADEOUT;
    }

    //状態の取得
    public STATE GetState()
    {
        return m_State;
    }
}
