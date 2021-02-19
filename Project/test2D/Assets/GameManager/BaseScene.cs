using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseScene : MonoBehaviour
{
    private const float FADE_MAX = 1.0f;
    private const float FADE_MIN = 0.0f;
    protected GameObject fadeCanvas = null;
    private FadeImage fadeImage = null;
    [SerializeField] public float FadeSpeed = 0.02f;
    [SerializeField] public string NextSceneName = "";

    // シーンフェード状態
    public enum FADE_STATE
    {
        FADEIN,     // 明るくなる
        FADEOUT,    // 暗くなる
        NONE,       // フェードキャンバス無し
        BLACK,      // 真っ暗
    };

    private FADE_STATE FadeState;

    public FADE_STATE fadeState
    {
        set
        {
            if (this.FadeState != value)
            {
                if (fadeCanvas)
                {
                    // 指定されたフェード状態に応じて初期値を代入する
                    switch( value )
                    {
                        case FADE_STATE.FADEIN: fadeImage.Range = FADE_MAX; break;
                        case FADE_STATE.FADEOUT: fadeImage.Range = FADE_MIN; break;
                        case FADE_STATE.NONE: fadeImage.Range = FADE_MIN; break;
                        case FADE_STATE.BLACK: fadeImage.Range = FADE_MAX; break;
                    }
                }
                    
                this.FadeState = value;
            }
        }
        get
        { return this.FadeState; }
    }

    virtual protected void Start()
    {
        fadeCanvas = GameObject.Find("FadeCanvas");
        fadeState = FADE_STATE.FADEIN;
        fadeImage = fadeCanvas.GetComponent<FadeImage>();
        fadeImage.Range = FADE_MAX;
    }

    virtual protected void Update()
    {
        switch (FadeState)
        {
            case FADE_STATE.FADEIN: FadeIn(); break;
            case FADE_STATE.FADEOUT: FadeOut(); break;
            case FADE_STATE.NONE: break;
            case FADE_STATE.BLACK: break;
        }
    }

    // フェードイン状態
    void FadeIn()
    {
        fadeImage.Range -= FadeSpeed;
        if (fadeImage.Range <= FADE_MIN)
        {
            fadeImage.Range = FADE_MIN;
            fadeState = FADE_STATE.NONE;
        }
    }

    // フェードアウト状態
    void FadeOut()
    {
        fadeImage.Range += FadeSpeed;
        // フェードが終わったら次のシーンへ
        if (fadeImage.Range >= FADE_MAX)
        {
            fadeImage.Range = FADE_MAX;
            SceneManager.LoadScene(NextSceneName);
        }
    }

    // フェード状態が終わっているかどうか
    public bool IsFadeEnd()
    {
        if (fadeState != FADE_STATE.NONE) return false;

        return true;
    }
}
