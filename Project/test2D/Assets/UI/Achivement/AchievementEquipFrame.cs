using UnityEngine;

public class AchievementEquipFrame : MonoBehaviour
{
    [SerializeField] float AnimationTime = 0.3f;
    private float AnimationCount = 0f;

    enum STATE
    {
        APPEAR,
        WAIT,
        VANISH,
    }

    STATE state = STATE.WAIT;

    void Start()
    {
        // 中心に配置して隠しておく
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = Vector3.zero;
    }


    void Update()
    {
        switch(state)
        {
            case STATE.APPEAR: Appear(); break;
            case STATE.WAIT: break;
            case STATE.VANISH: Vanish(); break;
        }
    }

    /// <summary>
    /// 出現
    /// </summary>
    private void Appear()
    {
        AnimationCount += Time.deltaTime;
        if (AnimationCount >= AnimationTime)
        {
            this.transform.localScale = new Vector3(1.0f, 1.0f);
            state = STATE.WAIT;
            AnimationCount = 0f;
        }
        else
        {
            float scale = Easing.Linear(AnimationCount, AnimationTime, 1.0f, 0.0f);
            this.transform.localScale = new Vector3(scale,scale);
        }
    }

    /// <summary>
    /// 消失
    /// </summary>
    private void Vanish()
    {
        AnimationCount += Time.deltaTime;
        if (AnimationCount >= AnimationTime)
        {
            this.transform.localScale = Vector3.zero;
            state = STATE.WAIT;
            AnimationCount = 0f;
        }
        else
        {
            float scale = Easing.Linear(AnimationCount, AnimationTime, 0.0f, 1.0f);
            this.transform.localScale = new Vector3(scale, scale);
        }
    }

    public void StartAppear()
    {
        state = STATE.APPEAR;
        AnimationCount = 0f;
    }

    public void StartVanish()
    {
        if (state != STATE.VANISH)
        {
            state = STATE.VANISH;
            AnimationCount = 0f;
        }
    }
}
