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
    [SerializeField] PlayFabWaitConnect waitConnect = default;

    // ランキングシーン状態
    public enum STATE
    {
        PREPRATION,
        FADEIN,
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
        SoundManager.Instance.PlayBGM("MainGame");

        base.Start();
        fadeState = FADE_STATE.BLACK;
        state = STATE.PREPRATION;
    }


    override protected void Update()
    {

        switch (state)
        {
            case STATE.PREPRATION: Preparation(); break;
            case STATE.FADEIN: GameFadeIn(); break;
            case STATE.MAIN: GameMain(); break;
            case STATE.FADEOUT: GameFadeOut(); break;
        }

        base.Update();
    }
    // 準備
    void Preparation()
    {
        // 通信待ちをしていなかったら出現状態へ移行する
        if (!waitConnect.IsWait())
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
            state = STATE.MAIN;
        }
    }

    // メイン状態
    void GameMain()
    {
        // クリックして状態移行
        //if (Input.GetMouseButtonDown(0))
        //{
        //    state = STATE.FADEOUT;
        //}
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
}
