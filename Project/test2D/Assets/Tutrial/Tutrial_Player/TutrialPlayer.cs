using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class TutrialPlayer : MonoBehaviour
{
    [SerializeField] float JumpPower = 250;
    public bool JumpFlag { get; private set; }      // ジャンプ中かどうか
    private bool TwoJumpFlag;   // 2段ジャンプ中かどうか
    [SerializeField] TutrialSceneManager tutrialManager = default;
    [SerializeField] EffekseerEffectAsset jumpEffect = default;
    [SerializeField] EffekseerEffectAsset heartEffect = default;
    private Rigidbody2D rigidBody = default;
    private List<EffekseerHandle> heartEffectList = default;


    void Start()
    {
        JumpFlag = true;
        TwoJumpFlag = true;
        rigidBody = GetComponent<Rigidbody2D>();
        heartEffectList = new List<EffekseerHandle>();
    }

    void Update()
    {
        // マウスがクリックされたらプレイヤーをジャンプ状態にする。
        if (Input.GetMouseButtonDown(0))
        {
            if (tutrialManager.state == TutrialSceneManager.STATE.MAIN 
                && tutrialManager.tutrial != TutrialSceneManager.TUTRIAL.TUTRIAL_CHEF 
                && tutrialManager.tutrial != TutrialSceneManager.TUTRIAL.TUTRIAL_DESCRIPTION
                && tutrialManager.tutrial != TutrialSceneManager.TUTRIAL.TUTRIAL_FINISHDESCRIPTION)
            {
                if (JumpFlag == false)
                {
                    JumpFlag = true;
                    rigidBody.velocity = Vector2.zero;
                    rigidBody.AddForce(new Vector2(0.0f, JumpPower), ForceMode2D.Impulse);

                    // エフェクトの取得
                    EffekseerSystem.PlayEffect(jumpEffect, transform.position + new Vector3(0f, -10f));

                    // ジャンプ音を再生
                    SoundManager.Instance.PlaySE("Jump");
                }
                else if (TwoJumpFlag == false)
                {
                    {
                        TwoJumpFlag = true;
                        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0.0f);
                        rigidBody.AddForce(new Vector2(0.0f, JumpPower), ForceMode2D.Impulse);
                        // エフェクトの取得
                        EffekseerSystem.PlayEffect(jumpEffect, transform.position + new Vector3(0f, -10f));

                        // ジャンプ音を再生
                        SoundManager.Instance.PlaySE("Jump");
                    }
                }
            }
        }
        // ハートエフェクトを追従させる
        foreach (EffekseerHandle handle in heartEffectList)
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
        // アイテムを取ったら
        if (collision.gameObject.tag == "Item")
        {
            ItemEffect item = collision.gameObject.GetComponent<ItemEffect>();



            // エフェクトの取得
            EffekseerHandle handle = EffekseerSystem.PlayEffect(heartEffect, transform.position);
            SoundManager.Instance.PlaySE("Heart");

            // エフェクトを更新で追従させるためにリストにいれる
            heartEffectList.Add(handle);

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
