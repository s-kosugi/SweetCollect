using UnityEngine;

/// <summary>
/// キャラクターのジャンプアニメーションスクリプト
/// </summary>
public class JumpAnimation : MonoBehaviour
{
    GameMainManager gameMainManager = default;
    Rigidbody2D rb2D = default;
    [SerializeField] float animationJumpower = 50;

    void Start()
    {
        gameMainManager = GameObject.Find("GameManager").GetComponent<GameMainManager>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // メイン状態なのに動いていなかったらアニメーションさせる
        if (gameMainManager.state == GameMainManager.STATE.MAIN)
        {
            if (rb2D.velocity == Vector2.zero)
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
            // ゲームメイン時のみアニメーションさせる
            if (gameMainManager.state == GameMainManager.STATE.MAIN)
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
        Vector2 v = new Vector2(0.0f, animationJumpower);
        rb2D.velocity = v;
    }
}
