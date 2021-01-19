using UnityEngine;

public class MoveJumpTaiyaki : MonoBehaviour
{
    GameMainManager gameMain = default;
    [SerializeField] float hiJumpLine = 120.0f;
    [SerializeField] float jumpPower = 50.0f;
    [SerializeField] float hiJumpPowerRate = 3.0f;
    Rigidbody2D rb = default;
    int jumpCount = 0;
    bool jumpFlag = true;
    float gravityScale = 0f;


    void Start()
    {
        gameMain = transform.root.GetComponent<GameMainManager>();
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
    }


    void Update()
    {
        // ゲームメインが通常状態の時のみ動かす
        if (gameMain.state == GameMainManager.STATE.MAIN)
        {
            rb.gravityScale = gravityScale;
            // ジャンプしてなければジャンプさせる
            if (jumpFlag == false)
            {
                // ハイジャンプするラインを越えていたらハイジャンプする
                if (transform.position.x <= hiJumpLine)
                {
                    rb.AddForce(new Vector2(0.0f, jumpPower* hiJumpPowerRate), ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(new Vector2(0.0f, jumpPower), ForceMode2D.Impulse);
                }

                jumpFlag = true;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0.0f;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面についたらジャンプ可能にする
        if (collision.gameObject.tag == "Ground")
        {
            jumpFlag = false;
        }
    }
}
