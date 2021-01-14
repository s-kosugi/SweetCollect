using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputNameSceneManager : BaseScene
{
    [SerializeField] InputField nameInputField = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] PlayFabUserProfiel userProfiel = default;
    [SerializeField] TextMeshProUGUI errortextMesh = default;
    public enum STATE
    {
        FADEIN,
        MAIN,
        CHECKNAME,
        FADEOUT
    }
    private STATE m_State = STATE.FADEIN;

    override protected void Start()
    {
        base.Start();

        errortextMesh.enabled = false;

        // フェードインにする
        fadeState = FADE_STATE.FADEIN;
    }

    override protected void Update()
    {
        switch (m_State)
        {
            case STATE.FADEIN: SceneFadeIn(); break;
            case STATE.FADEOUT: SceneFadeOut(); break;
            case STATE.MAIN: SceneMain(); break;
            case STATE.CHECKNAME: CheckName(); break;
        }
        base.Update();
    }
    // フェードイン中
    private void SceneFadeIn()
    {
        if (IsFadeEnd())
        {
            m_State = STATE.MAIN;
        }
    }
    // メイン状態
    private void SceneMain()
    {


    }
    // フェードアウト中
    private void SceneFadeOut()
    {

    }
    /// <summary>
    /// 名前のチェック
    /// </summary>
    private void CheckName()
    {
        if (!waitConnect.IsWait() && userProfiel.setNameResult != PlayFabUserProfiel.SETNAME_RESULT.NONE)
        {
            if (userProfiel.setNameResult == PlayFabUserProfiel.SETNAME_RESULT.SUCCESS)
            {
                // フェードアウト状態にする
                fadeState = FADE_STATE.FADEOUT;
                m_State = STATE.FADEOUT;
            }
            // エラー表示をしてもう一度名前入力をさせる
            if (userProfiel.setNameResult == PlayFabUserProfiel.SETNAME_RESULT.ERROR)
            {
                m_State = STATE.MAIN;
                errortextMesh.enabled = true;
            }
        }
    }

    public void Push_NextButton()
    {
        // 名前チェックをする
        m_State = STATE.CHECKNAME;
        // 名前送信
        userProfiel.SetUserName(nameInputField.text);
    }
}
