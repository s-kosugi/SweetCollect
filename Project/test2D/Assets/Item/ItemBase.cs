using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [SerializeField] protected float MoveSpeed = 10.0f;
    [SerializeField] protected int Score = 1;
    [SerializeField] protected float RecoverValue = 10f;
    private bool MainCameraIn = false;
    // ゲームメインはアイテム管理で設定する
    public GameMainManager m_GameMainManager = null;

    public int score
    { get { return Score; } }
    public float recoverValue { get { return this.RecoverValue; } }

    protected virtual void Start()
    {
        Score = 1;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (m_GameMainManager.state == GameMainManager.STATE.MAIN)
        {
            Vector3 pos = transform.position;
            pos.x -= MoveSpeed * Time.deltaTime;
            transform.position = pos;
        }
        CheckAreaOut();
    }

    /// <summary>
    /// 範囲外に出た場合の処理
    /// </summary>
    protected void CheckAreaOut()
    {
        // 左画面外に出た場合消去する
        if (transform.position.x < -100 && !MainCameraIn)
        {
            Destroy(gameObject);
        }
        // フラグリセット
        MainCameraIn = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// メインカメラに写っているかどうかを判定する。
    /// </summary>
    private void OnWillRenderObject()
    {
        // 映っているカメラがメインだったら
        if (Camera.current.name == "Main Camera")
        {
            MainCameraIn = true;
        }
    }
}
