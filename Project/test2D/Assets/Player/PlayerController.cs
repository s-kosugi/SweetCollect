using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float AnimationJumpPower = 2000.0f;
    [SerializeField] float HpDownIngerval = 0.2f;
    private bool JumpFlag;      // ジャンプ中かどうか
    private bool TwoJumpFlag;   // 2段ジャンプ中かどうか
    private float HpDownTimerCount = 0;
    private GameMainManager GameMainManager = null;  // ゲームメインマネージャー
    private ScoreManager ScoreManager = null;
    private EffekseerEffectAsset m_JumpEffect = null;
    private EffekseerEffectAsset m_HeartEffect = null;
    private EffekseerEffectAsset m_DeadEffect = null;
    private EffekseerEffectAsset m_DamageEffect = null;
    private EffekseerEffectAsset m_CoinGetEffect = null;
    private Rigidbody2D m_RigidBody = null;
    private CalcDamage m_CalcDamage = null;
    private BlinkAnimeSpriteRenderer m_Blink = null;


    void Start()
    {
        JumpFlag = true;
        TwoJumpFlag = true;
        HpDownTimerCount = 0;
        GameMainManager = GameObject.Find("GameManager").GetComponent<GameMainManager>();
        ScoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        m_JumpEffect = Resources.Load<EffekseerEffectAsset>("Effect\\jump");
        m_HeartEffect = Resources.Load<EffekseerEffectAsset>("Effect\\heart");
        m_DeadEffect = Resources.Load<EffekseerEffectAsset>("Effect\\deadeffect");
        m_DamageEffect = Resources.Load<EffekseerEffectAsset>("Effect\\damage");
        m_CoinGetEffect = Resources.Load<EffekseerEffectAsset>("Effect\\CoinGet");
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_CalcDamage = GetComponent<CalcDamage>();
        m_Blink = GetComponent<BlinkAnimeSpriteRenderer>();
    }

    void Update()
    {
        // マウスがクリックされたらプレイヤーをジャンプ状態にする。
        if (Input.GetMouseButtonDown(0))
        {
            if (GameMainManager.state == GameMainManager.STATE.MAIN)
            {
                if (JumpFlag == false)
                {
                    JumpFlag = true;
                    m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, 0.0f);
                    m_RigidBody.AddForce(new Vector2(0.0f, 250.0f), ForceMode2D.Impulse);

                    // エフェクトの取得
                    EffekseerSystem.PlayEffect(m_JumpEffect, transform.position + new Vector3(0f, -10f));

                    // ジャンプ音を再生
                    SoundManager.Instance.PlaySE("Jump");
                }
                else if (TwoJumpFlag == false)
                {
                    {
                        TwoJumpFlag = true;
                        m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, 0.0f);
                        m_RigidBody.AddForce(new Vector2(0.0f, 250.0f), ForceMode2D.Impulse);
                        // エフェクトの取得
                        EffekseerSystem.PlayEffect(m_JumpEffect, transform.position + new Vector3(0f, -10f));

                        // ジャンプ音を再生
                        SoundManager.Instance.PlaySE("Jump");
                    }
                }
            }
        }
        // 時間経過でのダメージ処理
        DamageTimeElapsed();
        // 点滅アニメーションの停止
        if ( m_CalcDamage.state == CalcDamage.DAMAGE_STATE.NORMAL)
            m_Blink.ActiveFlag = false;
    }
    // 歩きアニメーション
    public void StartJumpAnimation()
    {
        // 歩きアニメーションをする。
        Vector2 v = new Vector2(0.0f, AnimationJumpPower);
        m_RigidBody.AddForce(v);
    }
    // 時間経過でのダメージ処理
    void DamageTimeElapsed()
    {
        HpDownTimerCount += Time.deltaTime;
        if (HpDownIngerval <= HpDownTimerCount)
        {
            HpDownTimerCount = 0;
            m_CalcDamage.DamageValue(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // コインを取ったらスコア加算をする
        if (collision.gameObject.tag == "Coin")
        {
            CoinEffect item = collision.gameObject.GetComponent<CoinEffect>();

            ScoreManager.AddScore(item.score);

            // エフェクトの取得
            EffekseerSystem.PlayEffect(m_CoinGetEffect, transform.position);
            SoundManager.Instance.PlaySE("Coin");
        }

        // アイテムを取ったらスコア加算をする
        if (collision.gameObject.tag == "Item")
        {
            ItemEffect item = collision.gameObject.GetComponent<ItemEffect>();
            // 回復処理
            m_CalcDamage.Recovery(item.recoverValue);

            ScoreManager.AddScore(item.score);

            // エフェクトの取得
            EffekseerSystem.PlayEffect(m_HeartEffect, transform.position);
            SoundManager.Instance.PlaySE("Heart");
        }

        // 敵に触ったらダメージ
        if (collision.gameObject.tag == "Enemy")
        {
            float oldHP = m_CalcDamage.hp;
            m_CalcDamage.Damage(collision.gameObject.GetComponent<CalcDamage>());
            if (m_CalcDamage.state == CalcDamage.DAMAGE_STATE.DEAD)
            {
                //Destroy(this.gameObject);

                // エフェクトの再生
                //EffekseerSystem.PlayEffect(m_DeadEffect, transform.position);

                //SoundManager.Instance.PlaySE("Dead");

                // ゲームオーバー状態へ変更する
                //GameMainManager.state = GameMainManager.STATE.OVER;
            }
            else
            {
                // ダメージ関数を通ってダメージ中
                if (m_CalcDamage.hp < oldHP)
                {
                    // ダメージを食らったけど死んでない
                    // ダメージエフェクト
                    EffekseerSystem.PlayEffect(m_DamageEffect, transform.position);
                    // 点滅アニメーションの開始
                    m_Blink.ActiveFlag = true;

                    // SEの再生
                    SoundManager.Instance.PlaySE("Damage");
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面についたらジャンプ可能にする
        if (collision.gameObject.tag == "Ground")
        {
            JumpFlag = false;
            TwoJumpFlag = false;
                
            // ゲームメイン時のみアニメーションさせる
            if (GameMainManager.state == GameMainManager.STATE.MAIN)
            {
                // 歩きアニメーションをする。
                Vector2 v = new Vector2(0.0f, AnimationJumpPower);
                m_RigidBody.AddForce(v);
            }
        }
    }
}
