using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPlayerController : MonoBehaviour
{
    [SerializeField] float goalPoint = -127.0f;
    [SerializeField] float runTime = 2.0f;
    float startPoint = 0f;
    float runCount = 0f;
    public STATE state { get; private set; } = STATE.PREPARATION;
    public enum STATE
    {
        PREPARATION,
        RUN,
        JUMP,
        WAIT,
    }

    void Start()
    {
        startPoint = this.transform.position.x;
    }

    void Update()
    {
        switch (state)
        {
            case STATE.PREPARATION: Preparation(); break;
            case STATE.RUN: Run(); break;
            case STATE.JUMP: Jump(); break;
            case STATE.WAIT: Wait(); break;
        }
    }

    /// <summary>
    /// 準備中
    /// </summary>
    void Preparation()
    {
    }

    /// <summary>
    /// 画面外から走ってくる
    /// </summary>
    void Run()
    {
        runCount += Time.deltaTime;
        if (runCount >= runTime)
        {
            runCount = 0f;
            state = STATE.JUMP;
        }
        else
        {
            // イージングで移動させる
            float posX = Easing.OutCubic(runCount,runTime, goalPoint,startPoint);
            transform.position = new Vector3(posX,transform.position.y);
        }
    }

    /// <summary>
    /// その場でジャンプ
    /// </summary>
    void Jump()
    {
    }

    /// <summary>
    /// 待機中
    /// </summary>
    void Wait()
    {
    }

    /// <summary>
    /// 走るアニメーションの開始
    /// </summary>
    public void StartRun()
    {
        state = STATE.RUN;
        runCount = 0f;
    }
}
