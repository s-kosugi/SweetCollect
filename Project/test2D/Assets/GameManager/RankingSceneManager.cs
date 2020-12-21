using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading;
using UnityEngine.Assertions.Must;

public class RankingSceneManager : BaseScene
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

    // ランキングシーン状態
    public enum STATE
    {
        PREPRATION,
        FADEIN,
        APPEAR,
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

        SoundManager.Instance.PlayBGM("MainGame");

        base.Start();
        fadeState = FADE_STATE.NONE;

    }

    // Update is called once per frame
    override protected void Update()
    {

        switch (state)
        {
            case STATE.PREPRATION: Preparation(); break;
            case STATE.FADEIN: GameFadeIn(); break;
            case STATE.APPEAR: Appear(); break;
            case STATE.MAIN: GameMain(); break;
            case STATE.FADEOUT: GameFadeOut(); break;
        }

        base.Update();
    }
    // 準備
    void Preparation()
    {
        // 通信待ちをしていなかったら出現状態へ移行する
        if (!m_WaitConnect.IsWait())
            state = STATE.FADEIN;

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
                if (AppearEndTime - (float)(list.Count - i) * AppearSubTime <= m_AppearTimer )
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
            state = STATE.MAIN;
        }


    }

    // メイン状態
    void GameMain()
    {
        // クリックして状態移行
        if (Input.GetMouseButtonDown(0))
        {
            state = STATE.FADEOUT;
        }
    }
    // フェードアウト状態
    void GameFadeOut()
    {
        if (!m_WaitConnect.IsWait())
        {
            // フェードアウト状態に変更する
            fadeState = FADE_STATE.FADEOUT;
        }
    }
}
