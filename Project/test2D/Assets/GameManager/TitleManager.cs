using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : BaseScene
{
    [SerializeField] InputField inputField = null;
    [SerializeField]private PlayFabPlayerData m_PlayFabPlayerData = null;
    private PlayFabUserProfiel m_PlayFabUserProfiel = null;
    private PlayFabInventory m_PlayFabInventory = null;
    private PlayFabStore m_PlayFabStore = null;
    private PlayFabWaitConnect m_WaitConnect = null;
    private bool isSetDefault = false;
    [SerializeField] BGMSlider bgmSlider= default;
    [SerializeField] SESlider seSlider = default;


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
        GameObject PlayFabManager = GameObject.Find("PlayFabManager");
        m_PlayFabPlayerData = PlayFabManager.transform.Find("PlayFabPlayerData").GetComponent<PlayFabPlayerData>();
        m_PlayFabUserProfiel = PlayFabManager.transform.Find("PlayFabUserProfiel").GetComponent<PlayFabUserProfiel>();
        m_PlayFabInventory = PlayFabManager.transform.Find("PlayFabInventory").GetComponent<PlayFabInventory>();
        m_PlayFabStore = PlayFabManager.transform.Find("PlayFabStore").GetComponent<PlayFabStore>();
        m_WaitConnect = PlayFabManager.GetComponent<PlayFabWaitConnect>();
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
        // 通信が終わったらフェードアウトさせる
        if (!m_WaitConnect.IsWait())
        {
            fadeState = FADE_STATE.FADEOUT;
        }
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
        // チュートリアル終了済みでなかったらチュートリアルへ飛ばす
        if (NextSceneName == "GameMainScene")
        {
            if (!m_PlayFabPlayerData.m_Data.ContainsKey(PlayerDataName.TUTORIAL) || m_PlayFabPlayerData.m_Data[PlayerDataName.TUTORIAL].Value != "End")
            {
                NextSceneName = "TutorialScene";
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
                    m_PlayFabPlayerData.SetPlayerData(PlayerDataName.ECLOTHES, "001_NORAML");
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
}
