using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseScene : MonoBehaviour
{
    private const float FADE_MAX = 1.0f;
    private const float FADE_MIN = 0.0f;
    protected GameObject fadeCanvas = null;
    private FadeImage m_FadeImage = null;
    [SerializeField] public float FadeSpeed = 0.02f;
    [SerializeField] public string NextSceneName = "";

    // シーンフェード状態
    public enum FADE_STATE
    {
        FADEIN,
        FADEOUT,
        NONE,
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
                    if (value == FADE_STATE.FADEIN)
                    {
                        m_FadeImage.Range = FADE_MAX;
                    }
                    if (value == FADE_STATE.FADEOUT)
                    {
                        m_FadeImage.Range = FADE_MIN;
                    }
                    if (value == FADE_STATE.NONE)
                    {
                        m_FadeImage.Range = FADE_MIN;
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
        m_FadeImage = fadeCanvas.GetComponent<FadeImage>();
        m_FadeImage.Range = FADE_MAX;
    }

    virtual protected void Update()
    {
        switch (FadeState)
        {
            case FADE_STATE.FADEIN: FadeIn(); break;
            case FADE_STATE.FADEOUT: FadeOut(); break;
            case FADE_STATE.NONE: break;
        }
    }

    // フェードイン状態
    void FadeIn()
    {
        m_FadeImage.Range -= FadeSpeed;
        if (m_FadeImage.Range <= FADE_MIN)
        {
            m_FadeImage.Range = FADE_MIN;
            fadeState = FADE_STATE.NONE;
        }
    }

    // フェードアウト状態
    void FadeOut()
    {
        m_FadeImage.Range += FadeSpeed;
        // フェードが終わったら次のシーンへ
        if (m_FadeImage.Range >= FADE_MAX)
        {
            m_FadeImage.Range = FADE_MAX;
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
