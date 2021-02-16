using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpPower = 225;
    public bool jumpFlag { get; private set; }      // ジャンプ中かどうか
    private bool twoJumpFlag;   // 2段ジャンプ中かどうか
    private ScoreManager scoreManager = null;
    [SerializeField] GameMainManager gameMainManager = default;  // ゲームメインマネージャー
    [SerializeField] EffekseerEffectAsset jumpEffect = default;
    [SerializeField] EffekseerEffectAsset heartEffect = default;
    [SerializeField] EffekseerEffectAsset heartShineEffect = default;
    private Rigidbody2D rigidBody2D = default;
    private List<EffekseerHandle> heartEffectList = default;
    public int jumpCount { get; private set; } = 0;     // 実績カウント用
    public int sweetGetCount { get; private set; } = 0;  // 実績カウント用


    void Start()
    {
        jumpFlag = true;
        twoJumpFlag = true;
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        heartEffectList = new List<EffekseerHandle>();
    }

    void Update()
    {
        // マウスがクリックされたらプレイヤーをジャンプ状態にする。
        if (Input.GetMouseButtonDown(0))
        {
            if (gameMainManager.state == GameMainManager.STATE.MAIN)
            {
                if (jumpFlag == false)
                {
                    jumpFlag = true;
                    rigidBody2D.velocity = Vector2.zero;
                    rigidBody2D.AddForce(new Vector2(0.0f, jumpPower), ForceMode2D.Impulse);
                    jumpCount++;

                    // エフェクトの取得
                    EffekseerSystem.PlayEffect(jumpEffect, transform.position + new Vector3(0f, -10f));

                    // ジャンプ音を再生
                    SoundManager.Instance.PlaySE("Jump");
                }
                else if (twoJumpFlag == false)
                {
                    twoJumpFlag = true;
                    rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0.0f);
                    rigidBody2D.AddForce(new Vector2(0.0f, jumpPower), ForceMode2D.Impulse);
                    jumpCount++;

                    // エフェクトの取得
                    EffekseerSystem.PlayEffect(jumpEffect, transform.position + new Vector3(0f, -10f));

                    // ジャンプ音を再生
                    SoundManager.Instance.PlaySE("Jump");
                }
            }
        }
        // ハートエフェクトを追従させる
        foreach( EffekseerHandle handle in heartEffectList)
        {
            if (handle.enabled)
            {
                handle.SetLocation(this.transform.position);
            }
            else
            {
                // リストから除去
                heartEffectList.Remove(handle);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // アイテムを取ったらスコア加算をする
        if (collision.gameObject.tag == "Item")
        {
            ItemEffect item = collision.gameObject.GetComponent<ItemEffect>();

            scoreManager.AddScore(item.score);

            sweetGetCount++;

            // エフェクトの取得
            EffekseerHandle handle = default;

            // ボーナス中かどうかで取得した時のエフェクトを変更する
            if (gameMainManager.CoinGetRate > 1.0f)
            {
                handle = EffekseerSystem.PlayEffect(heartShineEffect, transform.position);
                SoundManager.Instance.PlaySE("Heart_Shine");
            } else {
                handle = EffekseerSystem.PlayEffect(heartEffect, transform.position);
                SoundManager.Instance.PlaySE("Heart");
            }

            // エフェクトを更新で追従させるためにリストにいれる
            heartEffectList.Add(handle);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面についたらジャンプ可能にする
        if (collision.gameObject.tag == "Ground")
        {
            jumpFlag = false;
            twoJumpFlag = false;
        }
    }
}
