using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float JumpPower = 250;
    private bool JumpFlag;      // ジャンプ中かどうか
    private bool TwoJumpFlag;   // 2段ジャンプ中かどうか
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
    private List<EffekseerHandle> m_HeartEffectList = default;


    void Start()
    {
        JumpFlag = true;
        TwoJumpFlag = true;
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
        m_HeartEffectList = new List<EffekseerHandle>();
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
                    m_RigidBody.AddForce(new Vector2(0.0f, JumpPower), ForceMode2D.Impulse);

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
                        m_RigidBody.AddForce(new Vector2(0.0f, JumpPower), ForceMode2D.Impulse);
                        // エフェクトの取得
                        EffekseerSystem.PlayEffect(m_JumpEffect, transform.position + new Vector3(0f, -10f));

                        // ジャンプ音を再生
                        SoundManager.Instance.PlaySE("Jump");
                    }
                }
            }
        }
        // ハートエフェクトを追従させる
        foreach( EffekseerHandle handle in m_HeartEffectList)
        {
            if (handle.enabled)
            {
                handle.SetLocation(this.transform.position);
            }
            else
            {
                // リストから除去
                m_HeartEffectList.Remove(handle);
            }
        }
        // 点滅アニメーションの停止
        if ( m_CalcDamage.state == CalcDamage.DAMAGE_STATE.NORMAL)
            m_Blink.ActiveFlag = false;
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
            //m_CalcDamage.Recovery(item.recoverValue);

            ScoreManager.AddScore(item.score);

            // エフェクトの取得
            EffekseerHandle handle = EffekseerSystem.PlayEffect(m_HeartEffect, transform.position);
            // エフェクトを更新で追従させるためにリストにいれる
            m_HeartEffectList.Add(handle);

            SoundManager.Instance.PlaySE("Heart");
        }

        if (collision.gameObject.tag == "Enemy")
        {
            // 敵に触ったらダメージ(旧仕様)
            //float oldHP = m_CalcDamage.hp;
            //m_CalcDamage.Damage(collision.gameObject.GetComponent<CalcDamage>());
            //if (m_CalcDamage.state == CalcDamage.DAMAGE_STATE.DEAD)
            //{
            //    //Destroy(this.gameObject);

            //    // エフェクトの再生
            //    //EffekseerSystem.PlayEffect(m_DeadEffect, transform.position);

            //    //SoundManager.Instance.PlaySE("Dead");

            //    // ゲームオーバー状態へ変更する
            //    //GameMainManager.state = GameMainManager.STATE.OVER;
            //}
            //else
            //{
            //    // ダメージ関数を通ってダメージ中
            //    if (m_CalcDamage.hp < oldHP)
            //    {
            //        // ダメージを食らったけど死んでない
            //        // ダメージエフェクト
            //        EffekseerSystem.PlayEffect(m_DamageEffect, transform.position);
            //        // 点滅アニメーションの開始
            //        m_Blink.ActiveFlag = true;

            //        // SEの再生
            //        SoundManager.Instance.PlaySE("Damage");
            //    }
            //}
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面についたらジャンプ可能にする
        if (collision.gameObject.tag == "Ground")
        {
            JumpFlag = false;
            TwoJumpFlag = false;
        }
    }
}
