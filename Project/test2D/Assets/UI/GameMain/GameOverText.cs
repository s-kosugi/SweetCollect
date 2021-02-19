using UnityEngine;

/// <summary>
/// ゲームオーバー文字クラス
/// </summary>
public class GameOverText : MonoBehaviour
{

    [SerializeField] GameMainManager GameMainManager = default;  // ゲームメインマネージャー
    private bool isStart = false;               // クラスが開始されているかどうか
    private float startTime = 0;
    [SerializeField] float AnimationTime = 1.5f; // アニメーション時間
    [SerializeField] float StartPositionY = 300.0f;
    [SerializeField] float GoalPositionY = 90.0f;

    void Start()
    {
        Reset();
    }

    void FixedUpdate()
    {
        if (!isStart)
        {
            // ゲームオーバーになったら開始する
            if (GameMainManager.state == GameMainManager.STATE.OVER)
            {
                isStart = true;
                startTime = Time.time;
                SoundManager.Instance.PlaySE("Bell");
            }
        }
        else
        {
            // イージングでアニメーションさせる
            Vector3 vec = this.transform.localPosition;
            if (Time.time - startTime >= AnimationTime)
            {
                vec.y = GoalPositionY;
            }
            else
            {
                vec.y = Easing.OutBounce(Time.time - startTime, AnimationTime, GoalPositionY, StartPositionY);
            }
            this.transform.localPosition = vec;
            // リスタートされたらリセットする
            if (GameMainManager.state == GameMainManager.STATE.PRERESTART ||
                GameMainManager.state == GameMainManager.STATE.RESTART) Reset();
        }
    }
    /// <summary>
    /// リセットして再度使えるようにする
    /// </summary>
    private void Reset()
    {
        Vector3 vec = this.transform.localPosition;
        vec.y = StartPositionY;
        this.transform.localPosition = vec;
        isStart = false;
    }
}
