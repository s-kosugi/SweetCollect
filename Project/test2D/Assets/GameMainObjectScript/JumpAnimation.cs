using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAnimation : MonoBehaviour
{
    GameMainManager m_GameMainManager = null;
    Rigidbody2D m_Rigidbody2D = null;
    [SerializeField] float AnimationJumpower = 50;

    void Start()
    {
        m_GameMainManager = GameObject.Find("GameManager").GetComponent<GameMainManager>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面についたらジャンプ可能にする
        if (collision.gameObject.tag == "Ground")
        {
            // ゲームメイン時のみアニメーションさせる
            if (m_GameMainManager.state == GameMainManager.STATE.MAIN)
            {
                // 歩きアニメーションをする。
                StartJumpAnimation();
            }
        }
    }

    /// <summary>
    /// 歩きアニメーションの開始
    /// </summary>
    public void StartJumpAnimation()
    {
        Vector2 v = new Vector2(0.0f, AnimationJumpower);
        m_Rigidbody2D.velocity = v;
    }
}
