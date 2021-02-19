using UnityEngine;

/// <summary>
/// 横移動スクリプト
/// </summary>
public class MoveHorizontal : MonoBehaviour
{
    GameMainManager gameMainManager = default;
    [SerializeField] protected float MoveSpeed = 80.0f;


    void Start()
    {
        gameMainManager = transform.root.GetComponent<GameMainManager>();
    }


    void Update()
    {
        if (gameMainManager.state == GameMainManager.STATE.MAIN)
        {
            // 移動
            Vector3 pos = transform.position;
            pos.x -= MoveSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
