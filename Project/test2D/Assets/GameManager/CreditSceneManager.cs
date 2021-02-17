/// <summary>
/// クレジットシーン
/// </summary>
public class CreditSceneManager : BaseScene
{
    /// <summary>
    /// シーン状態
    /// </summary>
    public enum STATE
    {
        FADEIN,
        MAIN,
        FADEOUT
    }
    private STATE state = STATE.FADEIN;

    override protected void Start()
    {
        base.Start();


        // フェードインにする
        fadeState = FADE_STATE.FADEIN;
    }

    override protected void Update()
    {
        switch (state)
        {
            case STATE.FADEIN: CreditFadeIn(); break;
            case STATE.FADEOUT: CreditFadeOut(); break;
            case STATE.MAIN: Main(); break;
        }
        base.Update();
    }
    // フェードイン中
    private void CreditFadeIn()
    {
        if (IsFadeEnd())
        {
            state = STATE.MAIN;
        }
    }
    // メイン状態
    private void Main()
    {
        
     
    }
    // フェードアウト中
    private void CreditFadeOut()
    {

    }

    public void Push_CreditButton()
    {
        // フェードアウト状態にする
        fadeState = FADE_STATE.FADEOUT;
        state = STATE.FADEOUT;
    }

    //状態の取得
    public STATE GetState()
    {
        return state;
    }
}
