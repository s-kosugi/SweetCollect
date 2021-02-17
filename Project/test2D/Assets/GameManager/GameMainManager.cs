using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームメインシーン
/// </summary>
public class GameMainManager : BaseScene
{
    [SerializeField] public float GameOverTime = 2.0f;
    [SerializeField] public float GameDengerTime = 10.0f;
    [SerializeField] GameObject Player = null;
    [SerializeField] GameObject StartUIObject = null;
    [SerializeField] GameObject ReStartUIObject = null;
    [SerializeField] public float GameTimer = 60.0f;
    [SerializeField] Ads AdsObject = null;
    private StartUI startUI = default;
    private StartUI restartUI = default;

    private float gameOverCount = 0.0f;
    public ScoreManager scoreManager { get; private set; }
    private JumpAnimation playerJump = default;
    private PlayFabWaitConnect waitConnect = null;

    [SerializeField] ArrangeManager arrangeManager = default;
    [SerializeField] GameMainAchievement achievement = default;

#if UNITY_WEBGL
    [SerializeField] float WebGLGameOverTime = 2.0f;
    private float WebGLEndCount = 0f;
#endif

    [SerializeField] public float AndroidAutoAdsTime { get; private set; } = 5.0f;
    public float AndroidAutoAdsCount { get; private set; } = 0f;
    Button adsButton = default;
    [SerializeField] public float AndroidAutoSceneMoveTime { get; private set; } = 2.0f;
    public float AndroidAutoSceneMoveCount { get; private set; } = 0f;

    public float CoinGetRate { get; private set; }
    [SerializeField] float RestartBonusRate = 2.0f;     // リスタート時のコイン取得倍率

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
        FADEOUT,
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
        gameOverCount = 0.0f;
        CoinGetRate = 1f;
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        GameObject playFabManager = GameObject.Find("PlayFabManager");
        waitConnect = playFabManager.GetComponent<PlayFabWaitConnect>();
        playerJump = Player.GetComponent<JumpAnimation>();

        startUI = StartUIObject.GetComponent<StartUI>();
        restartUI = ReStartUIObject.GetComponent<StartUI>();

        adsButton = GameObject.Find("AdsButton").GetComponent<Button>();

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
            case STATE.FADEOUT: GameFadeOut(); break;

        }

        base.Update();
    }

    // 準備状態
    void GamePreParation()
    {
        // 通信待ちが終わった
        if(!waitConnect.IsWait() )
        {

            // 配置を難易度にあわせて更新
            arrangeManager.ChangeDifficultPattern();

            state = STATE.FADEIN;
            fadeState = FADE_STATE.FADEIN;

            scoreManager.Reset();
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
        if (startUI.isEnd)
        {
            state = STATE.MAIN;
            // プレイヤーのジャンプアニメーションを開始する
            playerJump.StartJumpAnimation();
        }
    }
    // ゲームメイン状態
    void GameMain()
    {
        // ゲームプレイ時間を減らす
        GameTimer -= Time.deltaTime;
        if (GameTimer <= 0)
        {
            GameTimer = 0;
            // ゲームオーバーに移行
            state = STATE.OVER;
        }
    }
    // ゲームオーバー状態
    void GameOver()
    {

        if (!AdsObject.isShow)
        {
            // ゲームスコアを確定する
            scoreManager.ConfirmScore();
        }

        gameOverCount += Time.deltaTime;
        if (gameOverCount >= GameOverTime)
        {
            gameOverCount = 0f;
            state = STATE.NEXT;
        }
    }
    // 次のシーン受付状態
    void GameNext()
    {
        // WebGL版は広告ボタンを表示せずに時間経過で移動する
#if UNITY_WEBGL
        WebGLEndCount += Time.deltaTime;
        if( WebGLGameOverTime >= WebGLEndCount)
        {
            NextSceneButtonClick();
        }
#endif
#if UNITY_ANDROID
        if (!AdsObject.isShow)
        {
            AndroidAutoAdsCount += Time.deltaTime;
            if (AndroidAutoAdsCount >= AndroidAutoAdsTime)
            {
                AndroidAutoAdsCount = 0f;
                // ボタンをクリックしたことにする
                adsButton.onClick.Invoke();
            }
        }
        else
        {
            AndroidAutoSceneMoveCount += Time.deltaTime;
            if (AndroidAutoSceneMoveTime <= AndroidAutoSceneMoveCount)
            {
                // 次シーン遷移処理
                NextSceneButtonClick();
            }
        }
#endif
    }
    // ゲームリスタート準備
    void GamePreReStart()
    {
        // 広告表示中でない且つ表示済み
        if (!AdsObject.isPlaying() && AdsObject.isShow)
        {
            // 得点入手レートをボーナス用に変更する
            CoinGetRate = RestartBonusRate;
            ReStartUIObject.SetActive(true);
            state = STATE.RESTART;
        }
    }
    // ゲームリスタート状態
    void GameReStart()
    {
        // UIの動きが終わったらリスタートさせる
        if (restartUI.isEnd)
        {
            SoundManager.Instance.PlayBGM("BonusTime");

            state = STATE.MAIN;
            // プレイヤーのジャンプアニメーションを開始する
            playerJump.StartJumpAnimation();
        }
    }
    // ゲームフェードアウト中
    void GameFadeOut()
    {
        // 通信終了したらフェードアウト状態に変更する
        if(!waitConnect.IsWait())
        {
            fadeState = FADE_STATE.FADEOUT;
        }
    }

    public void NextSceneButtonClick()
    {
        // フェードアウト状態に変更する
        state = STATE.FADEOUT;

        // 現在のスコアを仮想通貨に追加する
        scoreManager.AddVirtualCurrency();

        // 実績カウントの送信
        achievement.SendAchievementCount();
    }

    public void AddGameTime(float time)
    {
        GameTimer += time;
        if (GameTimer < 0) GameTimer = 0f;
    }

    // リスタート状態へ変更する
    public void RestartGame()
    {
        state = STATE.PRERESTART;
    }

    public void ReturnTitle()
    {

        NextSceneName = "TitleScene";

        // フェードアウト状態に変更する
        state = STATE.FADEOUT;
    }
}
