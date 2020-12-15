using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutrialJumpAnimation : MonoBehaviour
{
    TutrialSceneManager m_TutrialManager = null;
    Rigidbody2D m_Rigidbody2D = null;
    [SerializeField] float AnimationJumpower = 50;

    void Start()
    {
        m_TutrialManager = GameObject.Find("TutrialSceneManager").GetComponent<TutrialSceneManager>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // メイン状態なのに動いていなかったらアニメーションさせる
        if (m_TutrialManager.state == TutrialSceneManager.STATE.MAIN)
        {
            if(m_Rigidbody2D.velocity == Vector2.zero)
            {
                StartJumpAnimation();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面についたらジャンプ可能にする
        if (collision.gameObject.tag == "Ground")
        {
            if (m_TutrialManager)
            {
                // ゲームメイン時のみアニメーションさせる
                if (m_TutrialManager.state == TutrialSceneManager.STATE.MAIN)
                {
                    // 歩きアニメーションをする。
                    StartJumpAnimation();
                }
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
