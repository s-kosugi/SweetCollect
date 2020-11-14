using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class PlayerController : MonoBehaviour
{
    private bool JumpFlag;      // ジャンプ中かどうか
    private bool TwoJumpFlag;   // 2段ジャンプ中かどうか
    private GameMainManager GameMainManager = null;  // ゲームメインマネージャー
    ScoreManager ScoreManager = null;
    EffekseerEffectAsset m_JumpEffect = null;
    EffekseerEffectAsset m_HeartEffect = null;
    EffekseerEffectAsset m_DeadEffect = null;
    Rigidbody2D m_RigidBody = null;

    // Start is called before the first frame update
    void Start()
    {
        JumpFlag = true;
        TwoJumpFlag = true;
        GameMainManager = GameObject.Find("GameManager").GetComponent<GameMainManager>();
        ScoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        m_JumpEffect = Resources.Load<EffekseerEffectAsset>("Effect\\jump");
        m_HeartEffect = Resources.Load<EffekseerEffectAsset>("Effect\\heart");
        m_DeadEffect = Resources.Load<EffekseerEffectAsset>("Effect\\deadeffect");
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // マウスがクリックされたらプレイヤーをジャンプ状態にする。
        if (Input.GetMouseButtonDown(0))
        {
            if (JumpFlag == false)
            {
                JumpFlag = true;
                m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, 0.0f);
                m_RigidBody.AddForce(new Vector2(0.0f, 250.0f), ForceMode2D.Impulse);

                // エフェクトの取得
                EffekseerSystem.PlayEffect(m_JumpEffect, transform.position+new Vector3(0f,-10f));

                // ジャンプ音を再生
                SoundManager.Instance.PlaySE("Jump");
            }
            else if(TwoJumpFlag == false)
            {
                {
                    TwoJumpFlag = true;
                    m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x,0.0f);
                    m_RigidBody.AddForce( new Vector2(0.0f,250.0f),ForceMode2D.Impulse);
                    // エフェクトの取得
                    EffekseerSystem.PlayEffect(m_JumpEffect, transform.position + new Vector3(0f, -10f));

                    // ジャンプ音を再生
                    SoundManager.Instance.PlaySE("Jump");
                }

            }
            
        }

        //// マウスのボタンが押されたらプレイヤーをマウスの場所へ移動させる
        //if (Input.GetMouseButton(0))
        //{
        //    Vector3 v = Input.mousePosition;
        //    v.z = 10.0f;
        //    //v - gameObject.transform.position;
        //    // スクリーン座標をワールド座標に変換
        //    gameObject.transform.position = Camera.main.ScreenToWorldPoint(v);
        //}
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    // リソースからロード
        //    GameObject obj = (GameObject)Resources.Load("Effect\\FireEffect");
        //    Instantiate(obj, transform);
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // アイテムを取ったらスコア加算をする
        if (collision.gameObject.tag == "Item")
        {
            ItemBase item = collision.gameObject.GetComponent<ItemBase>();
            if (item == null)
            {
                // 取得できなかった場合Puddingのように親が持っている可能性があるので探す
                item = collision.gameObject.transform.parent.gameObject.GetComponent<ItemBase>();
                // それでもなければ処理しない
                if (item == null) return;
            }
            ScoreManager.AddScore(item.score);

            // エフェクトの取得
            EffekseerSystem.PlayEffect(m_HeartEffect, transform.position);
            SoundManager.Instance.PlaySE("Heart");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // 地面についたらジャンプ可能にする
        if (collision.gameObject.tag == "Ground")
        {
            JumpFlag = false;
            TwoJumpFlag = false;

            // 歩きアニメーションをする。
            Vector2 v = new Vector2(0.0f, 2000.0f);
            m_RigidBody.AddForce(v);
        }

        // 敵に触ったら消える
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);

            // エフェクトの再生
            EffekseerSystem.PlayEffect(m_DeadEffect, transform.position);

            SoundManager.Instance.PlaySE("Dead");

            // ゲームオーバー状態へ変更する
            GameMainManager.state = GameMainManager.STATE.OVER;
        }
    }
}
