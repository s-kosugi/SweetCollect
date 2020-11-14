using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private bool MainCameraIn = false;
    [SerializeField] protected float MoveSpeed = 2.0f;
    [SerializeField] protected float RotateSpeed = 4.0f;
    [SerializeField] protected float MaxRotateAngle = 10.0f;
    protected float SinAngle = 0f;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        SinAngle = 0f;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // 移動
        Vector3 pos = transform.position;
        pos.x -= MoveSpeed;
        transform.position = pos;


        // 回転値を指定して回転させる
        SinAngle += RotateSpeed;
        transform.eulerAngles = new Vector3(0.0f,0.0f, Mathf.Sin(SinAngle * Mathf.Deg2Rad) * MaxRotateAngle);

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
