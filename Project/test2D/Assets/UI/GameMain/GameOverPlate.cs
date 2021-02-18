using UnityEngine;

/// <summary>
/// ゲームオーバーのプレート表示
/// </summary>
public class GameOverPlate : MonoBehaviour
{
    private float appearTimer = 0f;
    [SerializeField] float AppearEndTime = 1.0f;
    [SerializeField] GameMainManager gameMainManager = default;
    public enum STATE
    {
        HIDE,
        APPEAR,
        WAIT,
    };
    private STATE m_State = STATE.HIDE;
    public STATE state
    {
        get { return this.m_State; }
        set { this.m_State = value; }
    }
    void Start()
    {
        gameObject.transform.localScale = Vector3.zero;
        if( gameMainManager == default)
            gameMainManager = GameObject.Find("GameManager").GetComponent<GameMainManager>();
    }


    void Update()
    {
#if UNITY_ANDROID
        switch (m_State)
        {
            case STATE.HIDE: Hide(); break;
            case STATE.APPEAR: Appear(); break;
            case STATE.WAIT: Wait(); break;
        }
#endif
    }
    void Hide()
    {
        gameObject.transform.localScale = Vector3.zero;
        if (gameMainManager.state == GameMainManager.STATE.OVER)
        {
            m_State = STATE.APPEAR;
        }
    }
    void Appear()
    {
        appearTimer += Time.deltaTime;
        if (appearTimer >= AppearEndTime)
        {
            gameObject.transform.localScale = new Vector3(1f, 1f);
            gameObject.transform.localEulerAngles = Vector3.zero;
            m_State = STATE.WAIT;
            appearTimer = 0f;
        }
        else
        {
            float scale = Easing.Linear(appearTimer, AppearEndTime, 1.0f, 0.0f);
            float angle = Easing.Linear(appearTimer, AppearEndTime, 0.0f, 180.0f);
            gameObject.transform.localScale = new Vector3(scale, scale);
            gameObject.transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }
    void Wait()
    {
        if(gameMainManager.state == GameMainManager.STATE.PRERESTART ||
            gameMainManager.state == GameMainManager.STATE.RESTART)
        {
            m_State = STATE.HIDE;
        }
    }
}
