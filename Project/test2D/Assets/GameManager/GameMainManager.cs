using UnityEngine;

public class GameMainManager : BaseScene
{
    [SerializeField] float GameOverTime = 2.0f;
    [SerializeField] GameObject Player = null;
    [SerializeField] GameObject StartUIObject = null;
    [SerializeField] GameObject ReStartUIObject = null;
    [SerializeField] public float GameTimer = 60f;
    [SerializeField] GameOverButton AdsButton = null;
    [SerializeField] GameOverButton ResultButton = null;
    [SerializeField] Ads AdsObject = null;
    private StartUI m_StartUI = null;
    private StartUI m_ReStartUI = null;

    private float GameOverCount = 0.0f;
    public ScoreManager m_ScoreManager { get; private set; }
    private bool isScoreSend = false;
    private PlayFabStatistics m_PlayFabStatistics = null;
    private JumpAnimation m_PlayerJump = null;
    private PlayFabWaitConnect m_WaitConnect = null;

    // ゲームメイン状態
    public enum STATE
    {
        PREPRATION,
        FADEIN,
        START,
        MAIN,
        OVER,
        NEXT,
        PRERESTART,
        RESTART,
    };

    private STATE State;

    public STATE state{
        set
        { this.State = value; }
        get
        { return this.State; }
    }


    override protected void Start()
    {
        GameOverCount = 0.0f;
        m_ScoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        GameObject playFabManager = GameObject.Find("PlayFabManager");
        m_PlayFabStatistics = playFabManager.transform.Find("PlayFabStatistics").GetComponent<PlayFabStatistics>();
        m_WaitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        m_PlayerJump = Player.GetComponent<JumpAnimation>();
        SoundManager.Instance.PlayBGM("MainGame");

        m_StartUI = StartUIObject.GetComponent<StartUI>();
        m_ReStartUI = ReStartUIObject.GetComponent<StartUI>();

        base.Start();

        state = STATE.PREPRATION;
        fadeState = FADE_STATE.BLACK;
    }

    override protected void Update()
    {
        switch( state )
        {
            case STATE.PREPRATION: GamePreParation(); break;
            case STATE.FADEIN: GameFadeIn(); break;
            case STATE.START: GameStart(); break;
            case STATE.MAIN: GameMain(); break;
            case STATE.OVER: GameOver(); break;
            case STATE.NEXT: GameNext(); break;
            case STATE.PRERESTART: GamePreReStart(); break;
            case STATE.RESTART: GameReStart(); break;
        }

        base.Update();
    }

    // 準備状態
    void GamePreParation()
    {
        // 通信待ちが終わった
        if(!m_WaitConnect.IsWait() )
        {
            state = STATE.FADEIN;
            fadeState = FADE_STATE.FADEIN;
        }
    }

    // フェードイン状態
    void GameFadeIn()
    {
        // フェードインが終わったら状態を遷移させる
        if (IsFadeEnd())
        {
            state = STATE.START;
            // 開始演出UIを有効化する
            StartUIObject.SetActive(true);
        }
    }
    // ゲーム開始状態
    void GameStart()
    {
        // UI演出が終了したらメイン状態に移る
        if (m_StartUI.isEnd)
        {
            state = STATE.MAIN;
            // プレイヤーのジャンプアニメーションを開始する
            m_PlayerJump.StartJumpAnimation();
        }
    }
    // ゲームメイン状態
    void GameMain()
    {
        // ゲームプレイ時間を減らす
        GameTimer -= Time.deltaTime;
        if (GameTimer <= 0)
        {
            // ゲームオーバーに移行
            state = STATE.OVER;
        }
    }
    // ゲームオーバー状態
    void GameOver()
    {
        if (!isScoreSend)
        {
            if (m_PlayFabStatistics && m_ScoreManager)
            {
                int staValue = m_PlayFabStatistics.GetStatisticValue("SweetsPoint");
                // ハイスコア更新で、統計情報が見つからなかった場合は既定値が返るので多分OK
                if ( staValue < m_ScoreManager.GetScore())
                {
                    Debug.Log("UpdateStatistics");
                    // ハイスコアを更新する
                    m_PlayFabStatistics.UpdatePlayerStatistics("SweetsPoint", m_ScoreManager.GetScore());
                }
                isScoreSend = true;

            }
        }
        GameOverCount += Time.deltaTime;
        if (GameOverCount >= GameOverTime)
        {
            GameOverCount = 0f;
#if UNITY_ANDROID
            // ボタンUIを出現させる
            AdsButton.state = GameOverButton.STATE.APPEAR;
#else
            // リザルトボタンの移動処理を書く
#endif
            ResultButton.state = GameOverButton.STATE.APPEAR;


            state = STATE.NEXT;
        }
    }
    // 次のシーン受付状態
    void GameNext()
    {
        // タップされたらフェードアウト状態にして次のシーンへ
        //if (Input.GetMouseButton(0))
        //{
        //    // フェードアウト状態に変更する
        //    fadeState = FADE_STATE.FADEOUT;
        //}
    }
    // ゲームリスタート準備
    void GamePreReStart()
    {
        if (!AdsObject.isPlaying())
        {
            ReStartUIObject.SetActive(true);
            state = STATE.RESTART;

#if UNITY_ANDROID
            // ボタンUIを隠す
            AdsButton.state = GameOverButton.STATE.HIDE;
#endif
            ResultButton.state = GameOverButton.STATE.HIDE;
        }
    }
    // ゲームリスタート状態
    void GameReStart()
    {
        // UIの動きが終わったらリスタートさせる
        if (m_ReStartUI.isEnd)
        {
            state = STATE.MAIN;
            // プレイヤーのジャンプアニメーションを開始する
            m_PlayerJump.StartJumpAnimation();
        }
    }

    public void NextSceneButtonClick()
    {
        // フェードアウト状態に変更する
        fadeState = FADE_STATE.FADEOUT;

        // 現在のスコアを仮想通貨に追加する
        m_ScoreManager.AddVirtualCurrency();
    }


}
