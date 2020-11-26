using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAnimeSpriteRenderer : MonoBehaviour
{
    [SerializeField] float Interval = 0.2f;
    float m_TimeCount = 0f;
    SpriteRenderer spriteRenderer = null;
    /// <summary>
    /// 点滅アニメーションをするかどうか
    /// </summary>
    private bool m_ActiveFlag = false;
    public bool ActiveFlag
    {
        get { return this.m_ActiveFlag; }
        set { 
                if (this.m_ActiveFlag != value)
                {
                    this.m_ActiveFlag = value;
                    m_TimeCount = 0f;
                    // 初期状態により色を設定しておく
                    if (this.m_ActiveFlag == false)　spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                    else spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
                }
            }
    }
    void Start()
    {
        m_TimeCount = 0f;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_ActiveFlag = false;
    }

    void Update()
    {
        if (m_ActiveFlag)
        {
            // 点滅処理
            m_TimeCount += Time.deltaTime;
            if (m_TimeCount >= Interval / 2.0f)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            }
            if (m_TimeCount >= Interval)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
                m_TimeCount = 0f;
            }
        }
    }
}
