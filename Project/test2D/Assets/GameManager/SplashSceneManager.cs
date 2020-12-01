using UnityEngine;

public class SplashSceneManager : BaseScene
{
    [SerializeField] float WaitScreenTime = 1.0f;
    private float WaitCounter = 0f;
    private enum STATE
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
            case STATE.FADEIN: SplashFadeIn(); break;
            case STATE.FADEOUT: SplashFadeOut(); break;
            case STATE.MAIN: Main(); break;
        }
        base.Update();
    }
    // フェードイン中
    private void SplashFadeIn()
    {
        if (IsFadeEnd())
        {
            m_State = STATE.MAIN;
        }
    }
    // メイン状態
    private void Main()
    {
        WaitCounter += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            WaitCounter = WaitScreenTime;
        }
        if (WaitCounter >= WaitScreenTime)
        {
            // フェードアウト状態にする
            fadeState = FADE_STATE.FADEOUT;
            m_State = STATE.FADEOUT;
        }
    }
    // フェードアウト中
    private void SplashFadeOut()
    {

    }
}
