using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    protected int Score = 1;
    protected float MoveSpeed = 1.0f;
    private bool MainCameraIn = false;

    public int score
    {
    get { return Score; }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Score = 1;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector3 pos = transform.position;
        pos.x -= MoveSpeed;
        transform.position = pos;

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
