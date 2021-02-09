using PlayFab.ClientModels;
using UnityEngine;

public class StageSelectParent : MonoBehaviour
{
    [SerializeField] float appearAnimationTime = 1.0f;
    [SerializeField] float goalHeight = 0.0f;
    [SerializeField] float vanishAnimationTime = 1.0f;
    [SerializeField] PlayFabPlayerData playerData = default;
    CanvasGroup canvasGroup = default;
    public string difficultName { get;private set; }
    float animationCount = 0f;
    float startHeight = default;

    /// <summary>
    /// 最初に選択されている難易度をセットしたかどうか
    /// </summary>
    bool isDifficultSet = false;


    STATE state = default;

    enum STATE
    {
        APPEAR,
        WAIT,
        VANISH,
        FADEOUT,
    }

    void Start()
    {
        animationCount = 0f;
        state = STATE.WAIT;

        startHeight = transform.localPosition.y;

        difficultName = DifficultName.EASY;

        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE.APPEAR: Appear(); break;
            case STATE.WAIT: Wait(); break;
            case STATE.VANISH: Vanish(); break;
            case STATE.FADEOUT: FadeOut(); break;
        }

        if (playerData.m_isGet && !isDifficultSet)
        {
            UserDataRecord record;
            if (playerData.m_Data.TryGetValue(PlayerDataName.SELECTED_DIFFICULT, out record))
            {
                difficultName = record.Value;
            }
            isDifficultSet = true;
        }
    }

    /// <summary>
    /// 出現アニメーション
    /// </summary>
    private void Appear()
    {
        animationCount += Time.deltaTime;
        float height = goalHeight;
        if (animationCount >= appearAnimationTime)
        {
            state = STATE.WAIT;
            animationCount = 0f;
        }
        else
        {
            height = Easing.OutBounce(animationCount, appearAnimationTime, goalHeight, startHeight);
        }

        transform.localPosition = new Vector3(transform.localPosition.x, height);
    }

    /// <summary>
    /// 待機
    /// </summary>
    private void Wait()
    {
    }

    /// <summary>
    /// 退場アニメーション
    /// </summary>
    private void Vanish()
    {
        animationCount += Time.deltaTime;
        float height = startHeight;
        if (animationCount >= vanishAnimationTime)
        {
            state = STATE.WAIT;
            animationCount = 0f;
        }
        else
        {
            height = Easing.InBack(animationCount, vanishAnimationTime, startHeight, goalHeight,0.5f);
        }

        transform.localPosition = new Vector3(transform.localPosition.x, height);
    }

    /// <summary>
    /// フェードアウトアニメーション
    /// </summary>
    private void FadeOut()
    {
        animationCount += Time.deltaTime;
        float alpha = 0.0f;
        if (animationCount >= vanishAnimationTime)
        {
            state = STATE.WAIT;
            animationCount = 0f;
        }
        else
        {
            alpha = Easing.OutCubic(animationCount, vanishAnimationTime, 0.0f, 1.0f);
        }

        canvasGroup.alpha = alpha;
    }

    /// <summary>
    ///  出現処理の開始
    /// </summary>
    public void StartAppear()
    {
        animationCount = 0f;
        state = STATE.APPEAR;
        transform.localPosition = new Vector3(transform.localPosition.x,startHeight);
    }

    /// <summary>
    ///  退場処理の開始
    /// </summary>
    public void StartVanish()
    {
        animationCount = 0f;
        state = STATE.VANISH;
        transform.localPosition = new Vector3(transform.localPosition.x, goalHeight);
    }

    /// <summary>
    /// フェードアウト状態の開始
    /// </summary>
    public void StartFadeOut()
    {
        state = STATE.FADEOUT;
        canvasGroup.alpha = 1.0f;
    }

    /// <summary>
    /// ステージの設定
    /// </summary>
    public void SelectDifficult(string DifficultName)
    {
        difficultName = DifficultName;
    }
}
