using UnityEngine;

public class TitlePlayerController : MonoBehaviour
{
    [SerializeField] float walkDistance = 30.0f;
    [SerializeField] float walkTime = 1.0f;
    [SerializeField] float waitTime = 1.0f;
    [SerializeField] float jumpPower = 100.0f;
    [SerializeField] int jumpNum = 2;
    [SerializeField] float runDistance = 600f;
    [SerializeField] float runTime = 3.0f;
    private float animeCount = 0f;
    private float startPosX = 0f;
    private bool leftWalkFlag = true;
    Rigidbody2D rb = default;
    bool jumpFlag = false;
    int jumpCount = 0;
    float runStartPosX = 0f;

    public STATE state { get; private set; } = STATE.WAIT;
    public enum STATE
    {
        LEFT_WALK,
        RIGHT_WALK,
        WAIT,
        JUMP,
        RUN,
        ENDANIME,
    }

    void Start()
    {
        startPosX = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE.LEFT_WALK: LeftWalk(); break;
            case STATE.RIGHT_WALK: RightWalk(); break;
            case STATE.WAIT: Wait(); break;
            case STATE.JUMP: Jump(); break;
            case STATE.RUN: Run(); break;
            case STATE.ENDANIME: EndAnime();break;
        }
    }
    /// <summary>
    /// 左に歩く処理
    /// </summary>
    private void LeftWalk()
    {
        // 左を向かせる
        transform.localScale = new Vector3(-1f, 1f);
        float posX;
        animeCount += Time.deltaTime;
        if (animeCount >= walkTime)
        {
            // 左に歩き切った
            animeCount = 0;
            posX = startPosX - walkDistance;
            state = STATE.WAIT;
            // 次は右に歩く
            leftWalkFlag = false;
        }
        else
        {
            posX = Easing.Linear(animeCount, walkTime, startPosX - walkDistance, startPosX);
        }
        transform.position = new Vector3(posX, transform.position.y);
    }
    /// <summary>
    /// 右に歩く処理
    /// </summary>
    private void RightWalk()
    {
        // 右を向かせる
        transform.localScale = new Vector3(1f, 1f);

        float posX;
        animeCount += Time.deltaTime;
        if (animeCount >= walkTime)
        {
            // 右に歩き切った
            animeCount = 0;
            posX = startPosX;
            state = STATE.WAIT;
            // 次は左に歩く
            leftWalkFlag = true;
        }
        else
        {
            posX = Easing.Linear(animeCount, walkTime, startPosX, startPosX - walkDistance);
        }
        transform.position = new Vector3(posX, transform.position.y);
    }
    /// <summary>
    /// 待機処理
    /// </summary>
    private void Wait()
    {
        animeCount += Time.deltaTime;
        if(animeCount >= waitTime)
        {
            animeCount = 0;

            // どちらに歩くかを決める
            if (leftWalkFlag) state = STATE.LEFT_WALK;
            else state = STATE.RIGHT_WALK;
        }
    }

    /// <summary>
    /// 走る前の喜びジャンプ処理
    /// </summary>
    private void Jump()
    {
        // 右を向かせる
        transform.localScale = new Vector3(1f, 1f);
        if (jumpFlag == false)
        {
            if (jumpCount >= jumpNum)
            {
                // 規定回数ジャンプしたので走るアニメーション
                state = STATE.RUN;

                animeCount = 0f;
                runStartPosX = transform.position.x;
            }
            else
            {
                rb.AddForce(new Vector2(0.0f, jumpPower), ForceMode2D.Impulse);
                jumpFlag = true;
                jumpCount++;
            }
        }
    }
    /// <summary>
    /// 走って画面外へ行く処理
    /// </summary>
    private void Run()
    {
        // 右を向かせる
        transform.localScale = new Vector3(1f, 1f);

        float posX;
        animeCount += Time.deltaTime;
        if (animeCount >= runTime)
        {
            // 右に走り切った
            posX = runStartPosX + runDistance;
            state = STATE.ENDANIME;
        }
        else
        {
            posX = Easing.Linear(animeCount, runTime, runStartPosX + runDistance, runStartPosX);
        }
        //移動
        transform.position = new Vector3(posX, transform.position.y);
    }
    /// <summary>
    /// アニメーション終了
    /// </summary>
    private void EndAnime()
    {

    }

    /// <summary>
    /// 接触処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面についたらジャンプ可能にする
        if (collision.gameObject.tag == "Ground")
        {
            jumpFlag = false;
        }
    }

    /// <summary>
    /// ジャンプアニメーション開始
    /// </summary>
    public void StartJump()
    {
        state = STATE.JUMP;
        jumpFlag = false;
    }
}
