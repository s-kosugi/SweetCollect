using UnityEngine;

public class GameMainManager : BaseScene
{
    [SerializeField] float GameOverTime = 2.0f;
    [SerializeField] GameObject Player = null;
    [SerializeField] GameObject StartUIObject = null;
    private StartUI m_StartUI = null;

    private float GameOverCount = 0.0f;
    private ScoreManager m_ScoreManager = null;
    private bool isScoreSend = false;
    private PlayFabStatistics m_PlayFabStatistics = null;
    private PlayFabVirtualCurrency m_PlayFabVirtualCurrency = null;
    private PlayerController m_PlayerCotroller = null;
    private PlayFabWaitConnect m_WaitConnect = null;
    private PlayerAvatar m_Avatar = null;

    // ゲームメイン状態
    public enum STATE
    {
        PREPRATION,
        FADEIN,
        START,
        MAIN,
        OVER,
        NEXT,
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
        m_PlayFabVirtualCurrency = playFabManager.GetComponent<PlayFabVirtualCurrency>();
        m_WaitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        m_PlayerCotroller = Player.GetComponent<PlayerController>();
        m_Avatar = Player.GetComponent<PlayerAvatar>();
        SoundManager.Instance.PlayBGM("MainGame");

        m_StartUI = StartUIObject.GetComponent<StartUI>();

        base.Start();

        state = STATE.PREPRATION;
        fadeState = FADE_STATE.BLACK;
    }

    // Update is called once per frame
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
        }

        base.Update();
    }

    // 準備状態
    void GamePreParation()
    {
        // 通信待ちが終わった
        if(!m_WaitConnect.IsWait() && m_Avatar.m_isAvatarChange)
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
            m_PlayerCotroller.StartJumpAnimation();
        }
    }
    // ゲームメイン状態
    void GameMain()
    {
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

                // 仮想通貨の加算
                if (m_PlayFabVirtualCurrency)
                {
                    Debug.Log("AddVirtualCurrency");

                    // 仮想通貨を加算する
                    m_PlayFabVirtualCurrency.AddUserVirtualCurrency("HA", m_ScoreManager.GetScore());
                }
            }
        }
        GameOverCount += Time.deltaTime;
        if (GameOverCount >= GameOverTime)
        {
            GameObject resultButton = GameObject.Find("ResultButton");
#if UNITY_ANDROID
            // ボタンUIを出現させる
            GameObject.Find("AdsButton").GetComponent<GameOverButton>().state = GameOverButton.STATE.APPEAR;
#else
            // リザルトボタンの移動処理を書く
#endif
            resultButton.GetComponent<GameOverButton>().state = GameOverButton.STATE.APPEAR;


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

    public void NextSceneButtonClick()
    {
        // フェードアウト状態に変更する
        fadeState = FADE_STATE.FADEOUT;
    }


}
