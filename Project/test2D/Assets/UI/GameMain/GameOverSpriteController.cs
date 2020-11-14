using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSpriteController : MonoBehaviour
{

    [SerializeField] GameMainManager GameMainManager = null;  // ゲームメインマネージャー
    private bool Enable = false;
    private float StartTime = 0;
    [SerializeField] float AnimationTime = 2.0f; // アニメーション時間
    [SerializeField] float StartPositionY = 160.0f;
    [SerializeField] float GoalPositionY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        GameMainManager = GameObject.Find("GameManager").GetComponent<GameMainManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Enable)
        {
            if (GameMainManager.state == GameMainManager.STATE.OVER)
            {
                Enable = true;
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
        }
    }
}
