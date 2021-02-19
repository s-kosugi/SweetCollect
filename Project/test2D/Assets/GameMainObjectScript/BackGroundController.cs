using UnityEngine;

/// <summary>
/// 背景操作クラス
/// </summary>
public class BackGroundController : MonoBehaviour
{
    [SerializeField] GameObject traceObject = null;
    [SerializeField] float moveSpeed = 60f;
    [SerializeField] GameMainManager gameMainManager = default;
    private bool mainCameraIn = false;
    private SpriteRenderer backSprite = default;



    void Start()
    {
        backSprite = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (gameMainManager.state == GameMainManager.STATE.MAIN)
        {
            // 背景スクロール
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);

            // 左画面外に出た場合
            if (transform.position.x < -100 && !mainCameraIn)
            {
                // もう一つの背景オブジェクトの隣に移動する。
                if (traceObject)
                {
                    transform.position = new Vector3(traceObject.transform.position.x + backSprite.bounds.size.x - 2.0f, transform.position.y, transform.position.z);
                }
            }
        }

        // フラグリセット
        mainCameraIn = false;
    }
    private void LateUpdate()
    {
        // 追従処理
        if (traceObject.transform.position.x < this.transform.position.x)
        {
            float pos = traceObject.transform.position.x + backSprite.bounds.size.x;
            transform.position = new Vector3(pos, transform.position.y, transform.position.z);
        }
    }
    // メインカメラに写っているかどうかを判定する。
    void OnWillRenderObject()
    {
        // 映っているカメラがメインだったら
        if (Camera.current.name == "Main Camera")
        {
            mainCameraIn = true;
        }
    }
}
