using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutrialBackGroundController : MonoBehaviour
{
    [SerializeField] GameObject TraceObject = null;
    [SerializeField] float MoveSpeed = 60.0f;
    [SerializeField] GameObject TutrialManagerObject = null;
    private bool MainCameraIn = false;
    private TutrialSceneManager m_TutrialManager = null;


    // Start is called before the first frame update
    void Start()
    {
        m_TutrialManager = TutrialManagerObject.GetComponent<TutrialSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TutrialManager.state == TutrialSceneManager.STATE.MAIN)
        {
            // 背景スクロール
            transform.position = new Vector3(transform.position.x - MoveSpeed * Time.deltaTime, transform.position.y, transform.position.z);

            // 左画面外に出た場合
            if (transform.position.x < -100 && !MainCameraIn)
            {
                // もう一つの背景オブジェクトの隣に移動する。
                if (TraceObject)
                {
                    transform.position = new Vector3(TraceObject.transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x - 2.0f, transform.position.y, transform.position.z);
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
