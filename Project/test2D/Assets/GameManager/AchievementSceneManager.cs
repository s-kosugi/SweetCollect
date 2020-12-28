using UnityEngine;

public class AchievementSceneManager : BaseScene
{
    [SerializeField] private PlayFabPlayerData playFabPlayerData = default;
    [SerializeField] PlayFabInventory playFabInventory = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;


    public enum STATE
    {
        PREPARATION,
        FADEIN,
        MAIN,
        FADEOUT
    }
    public STATE state { get; private set; }

    override protected void Start()
    {

        NextSceneName = "ShopScene";

        base.Start();

        state = STATE.PREPARATION;

        // 黒画面にする
        fadeState = FADE_STATE.BLACK;
    }

    override protected void Update()
    {
        switch (state)
        {
            case STATE.PREPARATION: Preparation(); break;
            case STATE.FADEIN: AchiveFadeIn(); break;
            case STATE.FADEOUT: AchiveFadeOut(); break;
            case STATE.MAIN: AchiveMain(); break;
        }
        base.Update();


    }

    // 準備
    private void Preparation()
    {
        if (!waitConnect.IsWait())
        {
            state = STATE.FADEIN;
            fadeState = FADE_STATE.FADEIN;

        }
    }
    // フェードイン中
    private void AchiveFadeIn()
    {
        if (IsFadeEnd())
        {
            state = STATE.MAIN;
        }
    }
    // メイン状態
    private void AchiveMain()
    {

    }
    // フェードアウト中
    private void AchiveFadeOut()
    {
        // 通信が終わったらフェードアウトさせる
        if (!waitConnect.IsWait())
        {
            fadeState = FADE_STATE.FADEOUT;
        }
    }

    // 次のシーンへ
    public void NextScene()
    {
        // 通信待ちでない場合
        if (!waitConnect.IsWait())
        {
            if (fadeState != FADE_STATE.FADEOUT)
            {
                // フェードアウト状態にする
                state = STATE.FADEOUT;
            }
        }
    }
    // シーン名を指定してシーン変更
    public void NextScene(string sceneName)
    {
        NextSceneName = sceneName;

        NextScene();
    }
}
