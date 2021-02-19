using TMPro;
using UnityEngine;

public class TutrialSceneManager : BaseScene
{
    [SerializeField] PlayFabWaitConnect WaitConnect = default;     //通信
    [SerializeField] PlayFabPlayerData PlayerData = default;       //プレイヤーデータ

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


    [SerializeField] Tutrial_EquipFrame Discription_Start = null;   //開始時説明
    [SerializeField] Tutrial_EquipFrame Discription_Play = null;    //説明
    [SerializeField] Tutrial_EquipFrame Discription_End = null;     //終了後説明
    [SerializeField] Tutorial_Playing_Text Playing_Test = null;     //プレイ中テキスト

    public enum TUTRIAL
    {
        NONE = -1,
        TUTRIAL_DESCRIPTION = 0, //説明
        TUTRIAL_JUMP,            //ジャンプ
        TUTRIAL_DOUBLEJUMP,      //空中ジャンプ
        TUTRIAL_SYOKUDAI,        //燭台
        TUTRIAL_CHEF,            //シェフ
        TUTRIAL_FINISHDESCRIPTION,//終了
        TUTRIAL_END,//終了
    }
    [SerializeField] private TUTRIAL m_Tutrial = TUTRIAL.NONE;
    [SerializeField] private TUTRIAL m_NextTutrial = TUTRIAL.TUTRIAL_DESCRIPTION;


    float TutrialFinishMarginTimer = 0.0f;                                      //チュートリアル終了後の余白時間 
    [SerializeField] private float TUTRIAL_FINISH_MARGIN_TIME = 2.0f;           //チュートリアル終了後の余白時間 
    private bool FinishMargin = false;

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
        m_Tutrial = TUTRIAL.TUTRIAL_DESCRIPTION;

        TutrialFinishMarginTimer = 0.0f;
        FinishMargin = false;
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
            m_State = STATE.FADEIN;
        }
    }
    // フェードイン中
    private void TutrialFadeIn()
    {
        if (IsFadeEnd())
        {
            m_State = STATE.MAIN;
            Discription_Start.StartAppear();
        }
    }
    // メイン状態
    private void Main()
    {
        switch (m_Tutrial)
        {
            case TUTRIAL.TUTRIAL_DESCRIPTION: Tutrial_Description(); break;
            case TUTRIAL.TUTRIAL_JUMP: Tutrial_Jump(); break;
            case TUTRIAL.TUTRIAL_DOUBLEJUMP: Tutrial_DoubleJump(); break;
            case TUTRIAL.TUTRIAL_SYOKUDAI: Tutrial_Syokudai(); break;
            case TUTRIAL.TUTRIAL_CHEF: Tutrial_Chef(); break;
            case TUTRIAL.TUTRIAL_FINISHDESCRIPTION: Tutrial_FinishDescription(); break;
            case TUTRIAL.TUTRIAL_END: End(); break;
        }
    }
    // フェードアウト中
    private void TutrialFadeOut()
    {
        
    }

    //説明
    private void Tutrial_Description()
    {
        if (m_NextTutrial == TUTRIAL.TUTRIAL_JUMP)
        {
            m_Tutrial = m_NextTutrial;
            Discription_Play.StartAppear();
        }
        else
            m_NextTutrial = TUTRIAL.TUTRIAL_DESCRIPTION;
    } 
    // ジャンプ説明
    private void Tutrial_Jump()
    {
        if (m_NextTutrial == TUTRIAL.TUTRIAL_DOUBLEJUMP)
        {
            m_Tutrial = m_NextTutrial;
            Playing_Test.StartFade();
        }
        else
            m_NextTutrial = TUTRIAL.TUTRIAL_JUMP;
    } 
    // 二回ジャンプ説明
    private void Tutrial_DoubleJump()
    {
        if (m_NextTutrial == TUTRIAL.TUTRIAL_SYOKUDAI)
        {
            m_Tutrial = m_NextTutrial;
            Playing_Test.StartFade();
        }
        else
            m_NextTutrial = TUTRIAL.TUTRIAL_DOUBLEJUMP;
    }  
    // 燭台説明
    private void Tutrial_Syokudai()
    {
        if (m_NextTutrial == TUTRIAL.TUTRIAL_CHEF)
        {
            m_Tutrial = m_NextTutrial;
            Playing_Test.StartFade();
        }
        else
            m_NextTutrial = TUTRIAL.TUTRIAL_SYOKUDAI;

    }
    //店員説明
    private void Tutrial_Chef()
    {
        if (m_NextTutrial == TUTRIAL.TUTRIAL_FINISHDESCRIPTION)
        {
            m_Tutrial = m_NextTutrial;
            Discription_Play.StartVanish();
        }
        else
            m_NextTutrial = TUTRIAL.TUTRIAL_CHEF;
    }
    //説明終了
    private void Tutrial_FinishDescription()
    {
        if(!FinishMargin)
        {
            TutrialFinishMarginTimer += Time.deltaTime;
            if (TutrialFinishMarginTimer > TUTRIAL_FINISH_MARGIN_TIME)
            {
                FinishMargin = true;
                Discription_End.StartAppear();
            }
        }

        if(m_NextTutrial == TUTRIAL.TUTRIAL_END)
            m_Tutrial = m_NextTutrial;
    }
    //チュートリアル終了
    private void End()
    {
        m_State = STATE.FADEOUT;
        fadeState = FADE_STATE.FADEOUT;
        PlayerData.SetPlayerData(PlayerDataName.TUTORIAL, "End");
    }

    //状態変更
    public void TutrialChange(TUTRIAL Id)
    {
        m_NextTutrial = Id;
    }

}
