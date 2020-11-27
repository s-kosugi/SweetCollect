using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 範囲外チェック
public class CheckAreaOut : MonoBehaviour
{
    private bool MainCameraIn = false;


    void Update()
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
