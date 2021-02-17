using UnityEngine;

// 範囲外チェック
public class CheckAreaOut : MonoBehaviour
{
    [SerializeField] DIRECTION direction = DIRECTION.LEFT;
    private bool mainCameraIn = false;

    enum DIRECTION
    {
        LEFT,
        DOWN,
        TOP,
        BOTTOM
    }

    void Update()
    {
        bool areaOut = false;
        switch (direction)
        {
            case DIRECTION.LEFT: if (transform.position.x < -100) areaOut = true; break;
            case DIRECTION.DOWN: if (transform.position.y < -100) areaOut = true; break;
            case DIRECTION.TOP: if (transform.position.x > 100) areaOut = true; break;
            case DIRECTION.BOTTOM: if (transform.position.y> 100) areaOut = true; break;
        }
        // 画面外に出た場合消去する
        if (areaOut && !mainCameraIn)
        {
            Destroy(gameObject);
        }
        // フラグリセット
        mainCameraIn = false;
    }

    /// <summary>
    /// メインカメラに写っているかどうかを判定する。
    /// </summary>
    private void OnWillRenderObject()
    {
        // 映っているカメラがメインだったら
        if (Camera.current.name == "Main Camera")
        {
            mainCameraIn = true;
        }
    }
}
