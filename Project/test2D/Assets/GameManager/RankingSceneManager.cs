using UnityEngine;

public class RankingSceneManager : BaseScene
{
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] PlayFabLeaderBoard leaderBoard = default;
    [SerializeField] PlayFabLeaderBoard selfLeaderBoard = default;
    [SerializeField] RankingRecordParent recordParent = default;
    [SerializeField] RankingSelfRecord recordSelf = default;
    [SerializeField] CanvasGroup recordParentGroup = default;
    [SerializeField] CanvasGroup recordSelfGroup = default;
    [SerializeField] float recordFadeTime = 0.5f;
    private SELECT_DIFFICULT SelectDifficult = SELECT_DIFFICULT.HARD;
    [SerializeField] float connectWaitFrame = 5f;
    float connectWaitCount = 0f;
    float recordFadeCount = 0.0f;

    /// <summary>
    /// 選択難易度
    /// </summary>
    public enum SELECT_DIFFICULT
    {
        MIN,

        EASY = MIN,
        NORMAL,
        HARD,
        //VERYHARD,

        MAX
    }

    // ランキングシーン状態
    public enum STATE
    {
        PREPRATION,
        FADEIN,
        MAIN,
        VANISH_DIFFICULT,
        LOAD_DIFFICULT,
        APPEAR_DIFFICULT,
        FADEOUT,
    };

    private STATE State;

    public STATE state
    {
        set
        { this.State = value; }
        get
        { return this.State; }
    }


    override protected void Start()
    {
        SoundManager.Instance.PlayBGM("MainGame");

        base.Start();
        fadeState = FADE_STATE.BLACK;
        state = STATE.PREPRATION;


        // 最初に選択されている難易度をリーダーボード基準で変更する
        switch(leaderBoard.GetRankingName())
        {
            case RankingName.EASY: SelectDifficult = SELECT_DIFFICULT.EASY; break;
            case RankingName.NORMAL: SelectDifficult = SELECT_DIFFICULT.NORMAL; break;
            case RankingName.HARD: SelectDifficult = SELECT_DIFFICULT.HARD; break;
            //case RankingName.VERYHARD: SelectDifficult = SELECT_DIFFICULT.VERYHARD; break;
        }
    }


    override protected void Update()
    {

        switch (state)
        {
            case STATE.PREPRATION: Preparation(); break;
            case STATE.FADEIN: GameFadeIn(); break;
            case STATE.MAIN: GameMain(); break;
            case STATE.VANISH_DIFFICULT: VanishDifficult(); break;
            case STATE.LOAD_DIFFICULT: LoadDifficult(); break;
            case STATE.APPEAR_DIFFICULT: AppearDifficult(); break;
            case STATE.FADEOUT: GameFadeOut(); break;
        }

        base.Update();
    }
    // 準備
    void Preparation()
    {
        // 規定フレームの間通信待ちをしていなかったら出現状態へ移行する(子オブジェクトの作成待ち）
        if (!waitConnect.IsWait())
        {
            connectWaitCount++;
            if (connectWaitCount > connectWaitFrame)
            {
                fadeState = FADE_STATE.FADEIN;
                state = STATE.FADEIN;
                connectWaitCount = 0;
            }
        }
        else
        {
            connectWaitCount = 0;
        }

    }

    // フェードイン状態
    void GameFadeIn()
    {
        // フェードインが終わったら状態を遷移させる
        if (IsFadeEnd())
        {
            state = STATE.MAIN;
        }
    }

    // メイン状態
    void GameMain()
    {
    }

    /// <summary>
    /// 難易度切り替えによる消失処理
    /// </summary>
    void VanishDifficult()
    {
        // 消失演出
        recordFadeCount += Time.deltaTime;
        if (recordFadeCount < recordFadeTime)
        {
            recordSelfGroup.alpha = recordParentGroup.alpha = Easing.Linear(recordFadeCount, recordFadeTime, 0.0f, 1.0f);
        }
        else
        {
            recordSelfGroup.alpha = recordParentGroup.alpha = 0f;
            recordFadeCount = 0f;

            // 完全に消えたので新しい難易度をロードする
            string rankingName = rankingName = RankingName.EASY;
            switch (SelectDifficult)
            {
                case SELECT_DIFFICULT.EASY: rankingName = RankingName.EASY; break;
                case SELECT_DIFFICULT.NORMAL: rankingName = RankingName.NORMAL; break;
                case SELECT_DIFFICULT.HARD: rankingName = RankingName.HARD; break;
                    //case SELECT_DIFFICULT.VERYHARD: rankingName = RankingName.VERYHARD; break;
            }
            // リーダーボードの再取得要求
            leaderBoard.RequestGetLeaderBoard(rankingName);
            selfLeaderBoard.RequestGetLeaderBoard(rankingName);

            // レコードの子の再ロード
            recordParent.ReloadChild();
            recordSelf.ReloadRecord();


            state = STATE.LOAD_DIFFICULT;
        }

    }

    /// <summary>
    /// 難易度読み込み処理
    /// </summary>
    void LoadDifficult()
    {
        // 規定フレームの間通信待ちをしていなかったら出現状態へ移行する(子オブジェクトの作成待ち）
        if (!waitConnect.IsWait())
        {
            connectWaitCount++;
            if (connectWaitCount > connectWaitFrame)
            {
                if (leaderBoard.isGet /*&& selfLeaderBoard.isGet*/)
                {
                    connectWaitCount = 0;
                    // ロードが終わったら出現させる
                    state = STATE.APPEAR_DIFFICULT;
                }
            }
        }else
        {
            connectWaitCount = 0;
        }
    }

    /// <summary>
    /// 難易度出現処理
    /// </summary>
    void AppearDifficult()
    {
        // 出現演出
        recordFadeCount += Time.deltaTime;
        if (recordFadeCount < recordFadeTime)
        {
            recordSelfGroup.alpha = recordParentGroup.alpha = Easing.Linear(recordFadeCount, recordFadeTime, 1.0f, 0.0f);
        }
        else
        {
            recordSelfGroup.alpha = recordParentGroup.alpha = 1f;

            recordFadeCount = 0f;
            // 出現処理が終わったら操作可能にする
            state = STATE.MAIN;

        }
    }

    // フェードアウト状態
    void GameFadeOut()
    {
        if (!waitConnect.IsWait())
        {
            // フェードアウト状態に変更する
            fadeState = FADE_STATE.FADEOUT;
        }
    }

    // 次のシーンへ
    public void NextScene()
    {
        if (fadeState != FADE_STATE.FADEOUT)
        {
            // フェードアウト状態にする
            state = STATE.FADEOUT;
        }
    }
    // シーン名を指定してシーン変更
    public void NextScene(string sceneName)
    {
        NextSceneName = sceneName;

        NextScene();
    }

    /// <summary>
    /// 難易度切り替えボタン押下処理
    /// </summary>
    /// <param name="value"></param>
    public void MoveDifficult(int value)
    {
        // 通常状態以外では受け付けない
        if (state != STATE.MAIN) return;

        SelectDifficult += value;

        // 範囲を超えた場合の処理
        if (SelectDifficult < SELECT_DIFFICULT.MIN)
        {
            SelectDifficult = SELECT_DIFFICULT.MAX-1;
        }
        if (SelectDifficult >= SELECT_DIFFICULT.MAX)
        {
            SelectDifficult = SELECT_DIFFICULT.MIN;
        }

        state = STATE.VANISH_DIFFICULT;
    }
}
