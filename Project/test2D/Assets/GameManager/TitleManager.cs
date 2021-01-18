using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : BaseScene
{
    [SerializeField] InputField inputField = null;
    [SerializeField] PlayFabPlayerData m_PlayFabPlayerData = default;
    [SerializeField] PlayFabUserProfiel m_PlayFabUserProfiel = default;
    [SerializeField] PlayFabInventory m_PlayFabInventory = default;
    [SerializeField] PlayFabStore m_PlayFabStore = null;
    [SerializeField] PlayFabWaitConnect m_WaitConnect = null;
    private bool isSetDefault = false;
    [SerializeField] BGMSlider bgmSlider= default;
    [SerializeField] SESlider seSlider = default;
    [SerializeField] DifficultGroupe difficultGroupe = default;
    [SerializeField] TitlePlayerController titlePlayer = default;

    private bool optionButtonTap = false;


    public enum STATE
    {
        PREPARATION,
        FADEIN,
        MAIN,
        SIGNBOARD_CONTROL,
        OPTION_CONTROL,
        FADEOUT
    }
    public STATE state { get; private set; }

    override protected void Start()
    {
        NextSceneName = "GameMainScene";

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
            case STATE.FADEIN: TitleFadeIn(); break;
            case STATE.FADEOUT: TitleFadeOut(); break;
            case STATE.MAIN: TitleMain(); break;
            case STATE.SIGNBOARD_CONTROL: SignBoardControl(); break;
            case STATE.OPTION_CONTROL: OptionControl(); break;
        }
        base.Update();

        
    }

    // 準備
    private void Preparation()
    {
        if (!m_WaitConnect.IsWait())
        {
            state = STATE.FADEIN;
            fadeState = FADE_STATE.FADEIN;

            StartSound();
        }
    }
    // フェードイン中
    private void TitleFadeIn()
    {
        if(IsFadeEnd())
        {
            state = STATE.MAIN;
        }
    }
    // タイトルメイン状態
    private void TitleMain()
    {
        // デフォルト服データのセット
        SetDefaultEClothesData();
    }
    // フェードアウト中
    private void TitleFadeOut()
    {
        // 通信が終わった且つプレイヤーアニメが終わったらフェードアウトさせる
        if (!m_WaitConnect.IsWait() &&
            (titlePlayer.state == TitlePlayerController.STATE.ENDANIME) ||
            ((NextSceneName != "InputNameScene") && (NextSceneName != "TutorialScene") && (NextSceneName != "GameMainScene")))
        {
            fadeState = FADE_STATE.FADEOUT;
        }
    }

    /// <summary>
    /// 立て看板の操作状態
    /// </summary>
    private void SignBoardControl()
    {
    }

    /// <summary>
    /// オプション画面操作状態
    /// </summary>
    private void OptionControl()
    {
    }

    // 次のシーンへ
    public void NextScene()
    {
        // 通信待ちでない場合
        if (!m_WaitConnect.IsWait())
        {
            // テキストボックスが空白でない時にユーザー名を更新する
            if (inputField.text != "")
            {
                // ユーザー名の更新
                m_PlayFabUserProfiel.SetUserName(inputField.text);
            }
            // 音量設定を更新する
            m_PlayFabPlayerData.SetPlayerData(PlayerDataName.BGMVOLUME,SoundManager.Instance.m_BGMVolume.ToString());
            m_PlayFabPlayerData.SetPlayerData(PlayerDataName.SEVOLUME, SoundManager.Instance.m_SEVolume.ToString());

            // 選択難易度の更新
            m_PlayFabPlayerData.SetPlayerData(PlayerDataName.SELECTED_DIFFICULT,difficultGroupe.selectedDifficult);

            // オプションボタンがタップされていたらオプション通知処理を非表示にする
            if (optionButtonTap)
            {
                m_PlayFabPlayerData.SetPlayerData(PlayerDataName.NOTICE_OPTION, "FALSE");
            }

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
        // チュートリアル終了済みでなかったら名前入力へ飛ばす
        if (NextSceneName == "GameMainScene")
        {
            if (!m_PlayFabPlayerData.m_Data.ContainsKey(PlayerDataName.TUTORIAL) || m_PlayFabPlayerData.m_Data[PlayerDataName.TUTORIAL].Value != "End")
            {
                NextSceneName = "InputNameScene";
            }
        }
        NextScene();
    }
    /// <summary>
    /// デフォルト服データのセット
    /// </summary>
    private void SetDefaultEClothesData()
    {
        if (!isSetDefault)
        {
            if (!m_WaitConnect.IsWait())
            {
                // ユーザーデータを取得できていなかったらデフォルトデータを設定しておく
                if (!m_PlayFabPlayerData.m_Data.ContainsKey(PlayerDataName.ECLOTHES))
                {
                    m_PlayFabPlayerData.SetPlayerData(PlayerDataName.ECLOTHES, "001_NORMAL");
                }
                // 通常の服を持っていなかったらストアから購入する
                if (!m_PlayFabInventory.IsHaveItem("001_NORMAL"))
                {
                    m_PlayFabStore.BuyItem("001_NORMAL", "HA");
                }
                isSetDefault = true;
            }
        }
    }

    // サウンド開始処理
    private void StartSound()
    {
        UserDataRecord record = default;
        // キーがない場合の事を考慮する
        if (m_PlayFabPlayerData.m_Data.TryGetValue(PlayerDataName.BGMVOLUME, out record))
        {
            SoundManager.Instance.SetBGMVolume(float.Parse(record.Value));
        }
        if (m_PlayFabPlayerData.m_Data.TryGetValue(PlayerDataName.SEVOLUME, out record))
        {
            SoundManager.Instance.SetSEVolume(float.Parse(record.Value));
        }
        // スライダーオブジェクトの初期化
        bgmSlider.InitializeSlider();
        seSlider.InitializeSlider();

        // BGM再生開始
        SoundManager.Instance.PlayBGM("MainGame");
    }

    /// <summary>
    /// オプションボタンタップ時の処理
    /// </summary>
    public void TapOptionButton()
    {
        optionButtonTap = true;
        state = STATE.OPTION_CONTROL;
    }

    /// <summary>
    /// 立て看板タップ時の処理
    /// </summary>
    public void TapSignBoard()
    {
        state = STATE.SIGNBOARD_CONTROL;
    }

    /// <summary>
    /// メイン状態に戻るボタンを押した時の処理
    /// </summary>
    public void TapMainBackButton()
    {
        state = STATE.MAIN;
    }
}
