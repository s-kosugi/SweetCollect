using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 名前入力シーン
/// </summary>
public class InputNameSceneManager : BaseScene
{
    [SerializeField] InputField nameInputField = default;
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] PlayFabUserProfiel userProfiel = default;
    [SerializeField] TextMeshProUGUI errortextMesh = default;

    /// <summary>
    /// シーン状態
    /// </summary>
    public enum STATE
    {
        FADEIN,
        MAIN,
        CHECKNAME,
        FADEOUT
    }
    private STATE state = STATE.FADEIN;

    override protected void Start()
    {
        base.Start();

        errortextMesh.enabled = false;

        // フェードインにする
        fadeState = FADE_STATE.FADEIN;
    }

    override protected void Update()
    {
        switch (state)
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
            state = STATE.MAIN;
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
                state = STATE.FADEOUT;
            }
            // エラー表示をしてもう一度名前入力をさせる
            if (userProfiel.setNameResult == PlayFabUserProfiel.SETNAME_RESULT.ERROR)
            {
                state = STATE.MAIN;
                errortextMesh.enabled = true;
            }
        }
    }

    public void Push_NextButton()
    {
        // 名前チェックをする
        state = STATE.CHECKNAME;
        // 名前送信
        userProfiel.SetUserName(nameInputField.text);
    }
}
