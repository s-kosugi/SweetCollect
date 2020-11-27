using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSpriteController : MonoBehaviour
{

    [SerializeField] GameMainManager GameMainManager = null;  // ゲームメインマネージャー
    private bool m_Enable = false;
    private float StartTime = 0;
    [SerializeField] float AnimationTime = 2.0f; // アニメーション時間
    [SerializeField] float StartPositionY = 160.0f;
    [SerializeField] float GoalPositionY = 0.0f;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (!m_Enable)
        {
            if (GameMainManager.state == GameMainManager.STATE.OVER)
            {
                m_Enable = true;
                StartTime = Time.time;
            }
        }
        else
        {
            // イージングでアニメーションさせる
            Vector3 vec = this.transform.position;
            if (Time.time - StartTime >= AnimationTime)
            {
                vec.y = GoalPositionY;
            }
            else
            {
                vec.y = Easing.OutBounce(Time.time - StartTime, AnimationTime, GoalPositionY, StartPositionY);
            }
            this.transform.position = vec;
            // リスタートされたらリセットする
            if (GameMainManager.state == GameMainManager.STATE.PRERESTART) Reset();
        }
    }
    private void Reset()
    {
        Vector3 vec = this.transform.position;
        vec.y = StartPositionY;
        this.transform.position = vec;
        m_Enable = false;
    }
}
