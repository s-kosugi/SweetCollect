using UnityEngine;

/// <summary>
/// 看板(小)操作クラス
/// </summary>
public class SignBoardControl : MonoBehaviour
{
    [SerializeField] float GoalMovePosY = -500f;
    [SerializeField] float MoveTime = 0.3f;
    [SerializeField] BigSignBoardControl partnerBoard = default;
    float MoveCount = 0f;
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
        MoveCount += Time.deltaTime;
        if (MoveCount >= MoveTime)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, oldPos.y + GoalMovePosY);
            state = STATE.WAIT;
            MoveCount = 0f;

            // 画面外に消えたらもう片方の看板の出現処理を行う
            partnerBoard.StartAppear();
        }
        else
        {
            float posY = Easing.InBack(MoveCount, MoveTime, oldPos.y + GoalMovePosY, oldPos.y,1.0f);
            transform.localPosition = new Vector3(transform.localPosition.x, posY);
        }

    }
    /// <summary>
    /// 出現処理
    /// </summary>
    private void Appear()
    {
        // 画面外から出てくる
        MoveCount += Time.deltaTime;
        if (MoveCount >= MoveTime)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, oldPos.y);
            state = STATE.WAIT;
            MoveCount = 0f;
        }
        else
        {
            float posY = Easing.OutBack(MoveCount, MoveTime, oldPos.y, oldPos.y + GoalMovePosY,0.5f);
            transform.localPosition = new Vector3(transform.localPosition.x, posY);
        }
    }


    /// <summary>
    /// 消失開始
    /// </summary>
    public void StartVanish()
    {
        state = STATE.VANISH;
        MoveCount = 0f;
    }

    /// <summary>
    /// 出現開始
    /// </summary>
    public void StartAppear()
    {
        state = STATE.APPEAR;
        MoveCount = 0f;
    }
}
