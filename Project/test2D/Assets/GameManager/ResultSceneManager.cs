using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneManager : BaseScene
{
    private PlayFabWaitConnect m_WaitConnect = null;
    private float m_AppearTimer = 0.0f;
    [SerializeField] float AppearPos = -1000;
    [SerializeField] float AppearEndTime = 2.0f;
    [SerializeField] float AppearSubTime = 0.2f;
    [SerializeField] GameObject AppearGroup1 = null;
    [SerializeField] GameObject AppearGroup2 = null;
    [SerializeField] GameObject AppearGroup3 = null;
    [SerializeField] GameObject AppearGroup4 = null;

    [SerializeField] ReleaseDifficult releaseDifficult = default;
    
    private CanvasGroup m_CanvasGroup = null;

    // リザルトシーン状態
    public enum STATE
    {
        PREPRATION,
        FADEIN,
        APPEAR,
        UNLOCK,
        SHOW_MESSAGE,
        MAIN,
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
        GameObject PlayFabManager = GameObject.Find("PlayFabManager");
        m_WaitConnect = PlayFabManager.GetComponent<PlayFabWaitConnect>();

        // 出現前にUIを画面外に配置しておく
        AppearGroup1.transform.localPosition = new Vector3(AppearPos, 0);
        AppearGroup2.transform.localPosition = new Vector3(AppearPos, 0);
        AppearGroup3.transform.localPosition = new Vector3(AppearPos, 0);
        AppearGroup4.transform.localPosition = new Vector3(AppearPos, 0);

        m_CanvasGroup = GameObject.Find("Canvas").GetComponent<CanvasGroup>();

        SoundManager.Instance.PlayBGM("MainGame");


        base.Start();

        fadeState = FADE_STATE.BLACK;
    }

    // Update is called once per frame
    override protected void Update()
    {

        switch (state)
        {
            case STATE.PREPRATION: Preparation(); break;
            case STATE.FADEIN: GameFadeIn(); break;
            case STATE.APPEAR: Appear(); break;
            case STATE.UNLOCK: Unlock();break;
            case STATE.SHOW_MESSAGE: ShowMessage(); break;
            case STATE.MAIN: GameMain(); break;
            case STATE.FADEOUT: GameFadeOut(); break;
        }

        base.Update();
    }
    // 準備
    void Preparation()
    {
        // 通信が終了したらフェードインへ移行する
        if (!m_WaitConnect.IsWait())
        {
            fadeState = FADE_STATE.FADEIN;
            state = STATE.FADEIN;
        }
    }

    // フェードイン状態
    void GameFadeIn()
    {
        // フェードインが終わったら状態を遷移させる
        if (IsFadeEnd())
        {
            state = STATE.APPEAR;
            m_AppearTimer = 0.0f;
        }
    }

    // UIの出現状態
    void Appear()
    {
        m_AppearTimer += Time.deltaTime;

        // 出現タイマー制御用のリスト
        List<GameObject> list = new List<GameObject>();
        list.Add(AppearGroup1);
        list.Add(AppearGroup2);
        list.Add(AppearGroup3);
        list.Add(AppearGroup4);

        // 位置を初期化
        foreach (GameObject item in list)
        {
            item.transform.localPosition = new Vector3(0f, 0f);
        }

        // タップしたらアニメーションをスキップ
        if (Input.GetMouseButtonDown(0))
            m_AppearTimer = AppearEndTime;

        if (AppearEndTime > m_AppearTimer)
        {
            for (int i = 0; i < list.Count; i++)
            {
                // UIを上段から少しずつずらして出現させる
                float posX = Easing.OutSine(m_AppearTimer, AppearEndTime - (float)(list.Count - i) * AppearSubTime, 0, -AppearPos);
                if (AppearEndTime - (float)(list.Count - i) * AppearSubTime <= m_AppearTimer)
                {
                    list[i].transform.localPosition = new Vector3(0, 0);
                }
                else
                {
                    list[i].transform.localPosition = new Vector3(posX, 0);
                }
            }
        }
        else
        {
            // メインに移行する
            state = STATE.MAIN;

            // アンロックメッセージを表示するかどうか
            if (releaseDifficult.isShowUnlockMessage())
            {
                state = STATE.UNLOCK;
                // エフェクト再生開始
                releaseDifficult.StartReleaseEffect();
            }
        }
    }
    /// <summary>
    /// アンロック演出状態
    /// </summary>
    void Unlock()
    {
        // エフェクト再生が終わったらメッセージ表示に移行する
        if (releaseDifficult.IsReleaseEffectEnd())
        {
            state = STATE.SHOW_MESSAGE;
        }
    }

    /// <summary>
    /// メッセージ表示状態
    /// </summary>
    void ShowMessage()
    {
        // 画面上のボタンクリックで状態移行させる
    }

    // メイン状態
    void GameMain()
    {

    }
    // フェードアウト状態
    void GameFadeOut()
    {
        // 通信が終わったらシーンをフェードアウトに変更する
        if( !m_WaitConnect.IsWait())
        {
            fadeState = FADE_STATE.FADEOUT;
        }
    }

    // 次のシーンへ進む
    public void StepNextScene()
    {
        if (state != STATE.FADEOUT)
        {
            // フェードアウト状態に変更する
            state = STATE.FADEOUT;
        }
    }
    // 次のシーンへ進む(シーン名指定)
    public void StepNextScene(string sceneName)
    {
        NextSceneName = sceneName;
        StepNextScene();
    }

    /// <summary>
    /// 状態を通常状態に移行する
    /// </summary>
    public void SwitchStateNormal()
    {
        state = STATE.MAIN;
    }
}
