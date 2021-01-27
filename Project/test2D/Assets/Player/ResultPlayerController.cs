using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPlayerController : MonoBehaviour
{
    [SerializeField] float goalPoint = -127.0f;
    [SerializeField] float runTime = 2.0f;
    [SerializeField] float jumpInterval = 1.0f;
    [SerializeField] float hiJumpPower = 200;
    [SerializeField] GameObject itemManager = default;
    float startPoint = 0f;
    float animationCount = 0f;
    Rigidbody2D m_Rigidbody2D = null;
    [SerializeField] float AnimationJumpower = 50;
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
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
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
        animationCount += Time.deltaTime;
        if (animationCount >= runTime)
        {
            animationCount = 0f;
            state = STATE.JUMP;
            transform.position = new Vector3(goalPoint, transform.position.y);

            // アイテムマネージャーを起動する
            itemManager.SetActive(true);
        }
        else
        {
            // イージングで移動させる
            float posX = Easing.OutSine(animationCount,runTime, goalPoint,startPoint);
            transform.position = new Vector3(posX,transform.position.y);

            // 走る状態なのに動いていなかったらアニメーションさせる
            if (m_Rigidbody2D.velocity.y == 0f)
                StartJumpAnimation();
        }
    }

    /// <summary>
    /// その場でジャンプ
    /// </summary>
    void Jump()
    {
        // 接地から一定時間経過で再ジャンプ
        if (m_Rigidbody2D.velocity.y == 0f)
        {
            animationCount += Time.deltaTime;
            if (animationCount >= jumpInterval)
            {
                animationCount = 0f;
                Vector2 v = new Vector2(0.0f, hiJumpPower);
                m_Rigidbody2D.AddForce(v, ForceMode2D.Impulse);
            }
        }
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
        animationCount = 0f;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面についたらジャンプ可能にする
        if (collision.gameObject.tag == "Ground")
        {
            // 走っている時のみアニメーションさせる
            if (state == STATE.RUN)
                StartJumpAnimation();
        }
    }

    /// <summary>
    /// ジャンプアニメーションの開始
    /// </summary>
    public void StartJumpAnimation()
    {
        Vector2 v = new Vector2(0.0f, AnimationJumpower);
        m_Rigidbody2D.AddForce(v,ForceMode2D.Impulse);
    }
}
