using UnityEngine;
using UnityEngine.UI;

public class TitleManager : BaseScene
{
    [SerializeField] GameObject textBox = null;
    [SerializeField] string defaultPlayerName = "ななしさん";
    private PlayFabPlayerData m_PlayPlayerData = null;
    private PlayFabUserProfiel m_PlayFabUserProfiel = null;
    private PlayFabPlayerData m_PlayFabTutorialData = null;
    private PlayFabInventory m_PlayFabInventory = null;
    private PlayFabStore m_PlayFabStore = null;
    private PlayFabWaitConnect m_WaitConnect = null;

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
        SoundManager.Instance.PlayBGM("MainGame");

        GameObject PlayFabManager = GameObject.Find("PlayFabManager");
        m_PlayPlayerData = PlayFabManager.transform.Find("PlayFabPlayerData").GetComponent<PlayFabPlayerData>();
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
            case STATE.MAIN: Main(); break;
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
    private void Main()
    {
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
            // テキストボックスが空白の場合はデフォルトネームを入れる
            if (textBox.GetComponent<InputField>().text == "")
            {
                textBox.GetComponent<InputField>().text = defaultPlayerName;
            }
            if (fadeState != FADE_STATE.FADEOUT)
            {
                // ユーザー名の更新
                m_PlayFabUserProfiel.SetUserName(textBox.GetComponent<InputField>().text);

                // ユーザーデータを取得できていなかったらデフォルトデータを設定しておく
                if (!m_PlayPlayerData.m_isGet || !m_PlayPlayerData.m_Data.ContainsKey(PlayerDataName.ECLOTHES))
                {
                    m_PlayPlayerData.SetPlayerData(PlayerDataName.ECLOTHES, "001_NORAML");
                }
                // 通常の服を持っていなかったらストアから購入する
                if (!m_PlayFabInventory.IsHaveItem("001_NORMAL"))
                {
                    m_PlayFabStore.BuyItem("001_NORMAL", "HA");
                }

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
            if (m_PlayPlayerData.m_Data[PlayerDataName.TUTORIAL].Value != "End")
            {
                NextSceneName = "TutorialScene";
            }
        }
        NextScene();
    }
}
