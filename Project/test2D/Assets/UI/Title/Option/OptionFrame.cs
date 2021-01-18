using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionFrame : MonoBehaviour
{
    [SerializeField] Vector3 disablePos = default;
    [SerializeField] Vector3 enablePos = default;
    [SerializeField] float animationTime = 1.5f;
    float animationCount = 0f;
    public STATE state { get; private set; } 

    public enum STATE
    {
        WAIT,
        APPEAR,
        DISAPPEAR,

    }

    void Start()
    {
        transform.localPosition = disablePos;
    }

    private void Update()
    {
        switch (state)
        {
            case STATE.WAIT: Wait(); break;
            case STATE.APPEAR: Appear(); break;
            case STATE.DISAPPEAR: Disappear(); break;
        }
    }
    /// <summary>
    /// 消失開始
    /// </summary>
    public void DisableFrame()
    {
        state = STATE.DISAPPEAR;

        transform.localPosition = disablePos;
    }

    /// <summary>
    /// 出現開始
    /// </summary>
    public void EnableFrame()
    {
        state = STATE.APPEAR;

        transform.localPosition = enablePos;
    }
    /// <summary>
    /// 待機処理
    /// </summary>
    private void Wait()
    {
        animationCount = 0f;
    }
    /// <summary>
    /// 出現処理
    /// </summary>
    private void Appear()
    {
        animationCount += Time.deltaTime;
        float posX;
        if (animationCount >= animationTime)
        {
            // 移動終了
            state = STATE.WAIT;
            animationCount = 0f;
            posX = enablePos.x;
        }
        else
        {
            // イージングで動かす
            posX = Easing.OutBack(animationCount, animationTime, enablePos.x, disablePos.x, 0.5f);
        }
        transform.localPosition = new Vector3(posX, transform.localPosition.y);
    }
    /// <summary>
    /// 消失処理
    /// </summary>
    private void Disappear()
    {
        animationCount += Time.deltaTime;
        float posX;
        if (animationCount >= animationTime)
        {
            // 移動終了
            state = STATE.WAIT;
            animationCount = 0f;
            posX = disablePos.x;
        }
        else
        {
            // イージングで動かす
            posX = Easing.InBack(animationCount, animationTime, disablePos.x, enablePos.x, 0.5f);
        }
        transform.localPosition = new Vector3(posX, transform.localPosition.y);
    }
}
