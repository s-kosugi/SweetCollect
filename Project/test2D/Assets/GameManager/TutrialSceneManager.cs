using UnityEngine;

public class TutrialSceneManager : BaseScene
{
    [SerializeField] PlayFabWaitConnect WaitConnect = default;
    [SerializeField] PlayFabPlayerData PlayerData = default;
    private enum STATE
    {
        PREPARE,
        FADEIN,
        MAIN,
        FADEOUT
    }
    private STATE m_State = STATE.PREPARE;

    override protected void Start()
    {
        base.Start();


        // 黒画面にする
        fadeState = FADE_STATE.BLACK;
    }

    override protected void Update()
    {
        switch (m_State)
        {
            case STATE.PREPARE: TutrialPrepare(); break;
            case STATE.FADEIN: TutrialFadeIn(); break;
            case STATE.FADEOUT: TutrialFadeOut(); break;
            case STATE.MAIN: Main(); break;
        }
        base.Update();
    }
    // シーン準備中
    private void TutrialPrepare()
    {
        // 通信待ちのあと
        if (!WaitConnect.IsWait())
        {
            // フェードインにする
            fadeState = FADE_STATE.FADEIN;
            m_State = STATE.MAIN;
        }
    }
    // フェードイン中
    private void TutrialFadeIn()
    {
        if (IsFadeEnd())
        {
            m_State = STATE.MAIN;
        }
    }
    // メイン状態
    private void Main()
    {
        // クリックでチュートリアル終了
        if (Input.GetMouseButtonDown(0))
        {
            m_State = STATE.FADEOUT;
            fadeState = FADE_STATE.FADEOUT;
            PlayerData.SetPlayerData("End");
        }
    }
    // フェードアウト中
    private void TutrialFadeOut()
    {

    }
}
