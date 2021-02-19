using UnityEngine;

/// <summary>
/// 看板（大）操作クラス
/// </summary>
public class BigSignBoardControl : MonoBehaviour
{
    [SerializeField] float GoalMovePosY = 500f;
    [SerializeField] float MoveTime = 0.4f;
    [SerializeField] SignBoardControl partnerBoard = default;
    float moveCount = 0f;
    Vector3 oldPos = Vector3.zero;

    STATE state = STATE.WAIT;

    enum STATE
    {
        WAIT,
        VANISH,
        APPEAR,
    }

    void Start()
    {
        // 最初の座標を保存しておく
        oldPos = transform.localPosition;
    }


    void Update()
    {
        switch (state)
        {
            case STATE.VANISH: Vanish(); break;
            case STATE.WAIT: break;
            case STATE.APPEAR: Appear(); break;
        }
    }

    /// <summary>
    /// 消失処理
    /// </summary>
    private void Vanish()
    {
        // 画面外に消えていく
        moveCount += Time.deltaTime;
        if (moveCount >= MoveTime)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, oldPos.y );
            state = STATE.WAIT;
            moveCount = 0f;

            // 画面外に消えたらもう片方の看板の出現処理を行う
            partnerBoard.StartAppear();
        }
        else
        {
            float posY = Easing.InBack(moveCount, MoveTime, oldPos.y , oldPos.y + GoalMovePosY, 1.0f);
            transform.localPosition = new Vector3(transform.localPosition.x, posY);
        }

    }
    /// <summary>
    /// 出現処理
    /// </summary>
    private void Appear()
    {
        // 画面外から出てくる
        moveCount += Time.deltaTime;
        if (moveCount >= MoveTime)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, oldPos.y + GoalMovePosY);
            state = STATE.WAIT;
            moveCount = 0f;
        }
        else
        {
            float posY = Easing.OutBack(moveCount, MoveTime, oldPos.y + GoalMovePosY, oldPos.y, 0.5f);
            transform.localPosition = new Vector3(transform.localPosition.x, posY);
        }
    }


    /// <summary>
    /// 消失開始
    /// </summary>
    public void StartVanish()
    {
        state = STATE.VANISH;
        moveCount = 0f;
    }

    /// <summary>
    /// 出現開始
    /// </summary>
    public void StartAppear()
    {
        state = STATE.APPEAR;
        moveCount = 0f;
    }
}
