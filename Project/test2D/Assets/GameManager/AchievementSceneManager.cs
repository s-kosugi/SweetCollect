using PlayFab.ClientModels;
using UnityEngine;

public class AchievementSceneManager : BaseScene
{
    [SerializeField] private PlayFabPlayerData playFabPlayerData = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] AchievementParent achivementParent = default;
    [SerializeField] AchievementEquipFrame frame = default;

    /// <summary>
    /// シーン状態
    /// </summary>
    public enum STATE
    {
        PREPARATION,
        FADEIN,
        MAIN,
        POPUP,
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
            case STATE.POPUP: AchievePopUp(); break;
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

            // シーン開始時に現在つけている称号を選択済みにする
            UserDataRecord item = default;
            if (playFabPlayerData.m_Data.TryGetValue(PlayerDataName.ACHIEVEMENT_SELECT, out item))
            {
                achivementParent.SelectedAchievement(item.Value);
            }
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

    /// <summary>
    /// ポップアップ表示状態
    /// </summary>
    private void AchievePopUp()
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
                // 選択された称号を送信する
                playFabPlayerData.SetPlayerData(PlayerDataName.ACHIEVEMENT_SELECT,achivementParent.selectAchievementID);

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

    /// <summary>
    /// ポップアップ状態に変更する
    /// </summary>
    public void StartPopUp()
    {
        // 開放済みの称号だった場合ポップアップ状態に移行する
        if (achivementParent.isNowAchievementReach)
        {
            frame.StartAppear();
            state = STATE.POPUP;
        }
    }

    /// <summary>
    /// メイン状態に変更する
    /// </summary>
    public void StartMain()
    {
        state = STATE.MAIN;
    }
}
