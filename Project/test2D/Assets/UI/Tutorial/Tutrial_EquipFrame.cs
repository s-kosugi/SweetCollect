using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutrial_EquipFrame : MonoBehaviour
{
    [SerializeField] float AnimationTime = 0.3f;
    private float AnimationCount = 0f;
    public bool DisplayFlag { get; private set; }

    [SerializeField] Vector3 position = Vector2.zero; //初期位置の移動をした際に使用

    public enum STATE
    {
        APPEAR,
        WAIT,
        VANISH,
    }

    STATE state = STATE.WAIT;

    void Start()
    {
        DisplayFlag = false;

        // 中心に配置して隠しておく
        this.transform.localPosition = Vector3.zero + position;
        this.transform.localScale = Vector3.zero;
    }


    void Update()
    {
        switch (state)
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
            DisplayFlag = true;
        }
        else
        {
            float scale = Easing.Linear(AnimationCount, AnimationTime, 1.0f, 0.0f);
            this.transform.localScale = new Vector3(scale, scale);
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
            DisplayFlag = false;
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
