using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    [SerializeField] GameObject TraceObject = null;
    [SerializeField] float MoveSpeed = 0.4f;
    [SerializeField] GameObject GameMainManagerObject = null;
    private bool MainCameraIn = false;
    private GameMainManager m_GamemainManager = null;


    // Start is called before the first frame update
    void Start()
    {
        m_GamemainManager = GameMainManagerObject.GetComponent<GameMainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GamemainManager.state == GameMainManager.STATE.MAIN)
        {
            // 背景スクロール
            transform.position = new Vector3(transform.position.x - MoveSpeed * Time.deltaTime, transform.position.y, transform.position.z);

            // 左画面外に出た場合
            if (transform.position.x < -100 && !MainCameraIn)
            {
                // もう一つの背景オブジェクトの隣に移動する。
                if (TraceObject)
                {
                    transform.position = new Vector3(TraceObject.transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x - 1.0f, transform.position.y, transform.position.z);
                }
            }
        }

        // フラグリセット
        MainCameraIn = false;
    }
    // メインカメラに写っているかどうかを判定する。
    void OnWillRenderObject()
    {
        // 映っているカメラがメインだったら
        if (Camera.current.name == "Main Camera")
        {
            MainCameraIn = true;
        }
    }
}
