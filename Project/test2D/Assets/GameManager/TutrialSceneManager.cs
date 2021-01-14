using TMPro;
using UnityEngine;

public class TutrialSceneManager : BaseScene
{
    [SerializeField] PlayFabWaitConnect WaitConnect = default;
    [SerializeField] PlayFabPlayerData PlayerData = default;
    public enum STATE
    {
        PREPARE,
        FADEIN,
        MAIN,
        FADEOUT
    }
    [SerializeField] private STATE m_State = STATE.PREPARE;

    public STATE state
    {
        set
        { this.m_State = value; }
        get
        { return this.m_State; }
    }

    public enum TUTRIAL
    {
        TUTRIAL_01,
        TUTRIAL_02,
        TUTRIAL_03,
        TUTRIAL_04,
        TUTRIAL_END
    }
    [SerializeField] private TUTRIAL m_Tutrial = TUTRIAL.TUTRIAL_01;
    [SerializeField] private TUTRIAL m_NextTutrial = TUTRIAL.TUTRIAL_01;


    float TutrialEndTimer = 0.0f;                          //チュートリアル終了時間 
    const float TUTRIAL_END_TIME = 2.0f;                   //チュートリアル終了時間 

    public TUTRIAL tutrial
    {
        set
        { this.m_Tutrial = value; }
        get
        { return this.m_Tutrial; }
    }
    override protected void Start()
    {
        base.Start();

        SoundManager.Instance.PlayBGM("MainGame");

        // 黒画面にする
        fadeState = FADE_STATE.BLACK;

        //チュートリアル
        m_Tutrial = TUTRIAL.TUTRIAL_01;

        TutrialEndTimer = 0.0f;
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
        switch (m_Tutrial)
        {
            case TUTRIAL.TUTRIAL_01: Tutrial_01(); break;
            case TUTRIAL.TUTRIAL_02: Tutrial_02(); break;
            case TUTRIAL.TUTRIAL_03: Tutrial_03(); break;
            case TUTRIAL.TUTRIAL_04: Tutrial_04(); break;
            case TUTRIAL.TUTRIAL_END: Tutrial_End(); break;
        }
    }
    // フェードアウト中
    private void TutrialFadeOut()
    {
        
    }

    // チュートリアル
    private void Tutrial_01()
    {
        if (m_NextTutrial == TUTRIAL.TUTRIAL_02)
        {
            m_Tutrial = m_NextTutrial;
        }
        else
            m_NextTutrial = TUTRIAL.TUTRIAL_01;
    } 
    // チュートリアル
    private void Tutrial_02()
    {
        if (m_NextTutrial == TUTRIAL.TUTRIAL_03)
        {
            m_Tutrial = m_NextTutrial;
        }
        else
            m_NextTutrial = TUTRIAL.TUTRIAL_02;
    }  
    // チュートリアル
    private void Tutrial_03()
    {
        if (m_NextTutrial == TUTRIAL.TUTRIAL_04)
        {
            m_Tutrial = m_NextTutrial;
        }
        else
            m_NextTutrial = TUTRIAL.TUTRIAL_03;

    }
    // チュートリアル
    private void Tutrial_04()
    {
        if (m_NextTutrial == TUTRIAL.TUTRIAL_END)
        {
            m_Tutrial = m_NextTutrial;
        }
        else
            m_NextTutrial = TUTRIAL.TUTRIAL_04;
    }

    private void Tutrial_End()
    {
        TutrialEndTimer += Time.deltaTime;
        if (TutrialEndTimer > TUTRIAL_END_TIME)
        {
            m_State = STATE.FADEOUT;
            fadeState = FADE_STATE.FADEOUT;
            PlayerData.SetPlayerData(PlayerDataName.TUTORIAL,"End");
        }
    }

    //チュートリアル
    public void TutrialChange(TUTRIAL Id)
    {
        m_NextTutrial = Id;
    }

}
