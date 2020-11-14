using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverButton : MonoBehaviour
{
    private float m_AppearTimer = 0f;
    [SerializeField] float AppearEndTime = 1.0f;
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
    }

    // Update is called once per frame
    void Update()
    {
        switch( m_State )
        {
            case STATE.HIDE: Hide(); break;
            case STATE.APPEAR: Appear(); break;
            case STATE.WAIT: Wait(); break;
        }
    }
    void Hide()
    {
        gameObject.transform.localScale = Vector3.zero;
    }
    void Appear()
    {
        m_AppearTimer += Time.deltaTime;
        if( m_AppearTimer >= AppearEndTime)
        {
            gameObject.transform.localScale = new Vector3(1f,1f);
            gameObject.transform.localEulerAngles = Vector3.zero;
            m_State = STATE.WAIT;
            m_AppearTimer = 0f;
        }
        else
        {
            float scale = Easing.Linear(m_AppearTimer,AppearEndTime,1.0f,0.0f);
            float angle = Easing.Linear(m_AppearTimer, AppearEndTime, 0.0f, 180.0f);
            gameObject.transform.localScale = new Vector3(scale, scale);
            gameObject.transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }
    void Wait()
    {

    }

    // クリックされた時
    public void Click()
    {
        SoundManager.Instance.PlaySE("Tap");
    }
}
