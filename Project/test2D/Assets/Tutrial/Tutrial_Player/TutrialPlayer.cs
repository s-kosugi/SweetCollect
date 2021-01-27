using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class TutrialPlayer : MonoBehaviour
{
    [SerializeField] float JumpPower = 250;
    public bool JumpFlag { get; private set; }      // ジャンプ中かどうか
    private bool TwoJumpFlag;   // 2段ジャンプ中かどうか
    private TutrialSceneManager m_TutrialManager = null;
    private EffekseerEffectAsset m_JumpEffect = null;
    private EffekseerEffectAsset m_HeartEffect = null;
    private EffekseerEffectAsset m_HeartShineEffect = null;
    private EffekseerEffectAsset m_CoinGetEffect = null;
    private Rigidbody2D m_RigidBody = null;
    private CalcDamage m_CalcDamage = null;
    private BlinkAnimeSpriteRenderer m_Blink = null;
    private List<EffekseerHandle> m_HeartEffectList = default;


    void Start()
    {
        JumpFlag = true;
        TwoJumpFlag = true;
        m_TutrialManager = GameObject.Find("TutrialSceneManager").GetComponent<TutrialSceneManager>();
        m_JumpEffect = Resources.Load<EffekseerEffectAsset>("Effect\\jump");
        m_HeartEffect = Resources.Load<EffekseerEffectAsset>("Effect\\heart");
        m_HeartShineEffect = Resources.Load<EffekseerEffectAsset>("Effect\\heart_shine");
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
            if (m_TutrialManager.state == TutrialSceneManager.STATE.MAIN 
                && m_TutrialManager.tutrial != TutrialSceneManager.TUTRIAL.TUTRIAL_CHEF 
                && m_TutrialManager.tutrial != TutrialSceneManager.TUTRIAL.TUTRIAL_DESCRIPTION
                && m_TutrialManager.tutrial != TutrialSceneManager.TUTRIAL.TUTRIAL_FINISHDESCRIPTION)
            {
                if (JumpFlag == false)
                {
                    JumpFlag = true;
                    m_RigidBody.velocity = Vector2.zero;
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
        foreach (EffekseerHandle handle in m_HeartEffectList)
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // アイテムを取ったら
        if (collision.gameObject.tag == "Item")
        {
            ItemEffect item = collision.gameObject.GetComponent<ItemEffect>();



            // エフェクトの取得
            EffekseerHandle handle = EffekseerSystem.PlayEffect(m_HeartEffect, transform.position);
            SoundManager.Instance.PlaySE("Heart");

            // エフェクトを更新で追従させるためにリストにいれる
            m_HeartEffectList.Add(handle);

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
