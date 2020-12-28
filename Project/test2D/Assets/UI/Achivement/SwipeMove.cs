using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スワイプ移動
/// </summary>
public class SwipeMove : MonoBehaviour
{
    [SerializeField] float horizontalRate = 1.0f;
    [SerializeField] float verticalRate = 1.0f;
    [SerializeField] float friction = 0.9f;
    [SerializeField] float stopThreshold = 0.01f;
    [SerializeField] public Rect moveLimitRect = default;

    private bool touchFlag = false;
    private Vector3 oldTouchPos = Vector3.zero;
    private Vector3 inertiaMove = Vector3.zero;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // 押された瞬間だった場合は旧座標を保存する
            if (!touchFlag)
            {
                oldTouchPos = Input.mousePosition;
                touchFlag = true;
            }
            Vector3 velocity = Input.mousePosition - oldTouchPos;

            // 上下左右それぞれの移動倍率をかける
            velocity = new Vector3(velocity.x * horizontalRate, velocity.y * verticalRate, velocity.z);
            // 慣性移動用の値を保存
            inertiaMove = velocity;

            // オブジェクトを移動させる
            MoveObject(velocity);

            // 移動が終わったので旧座標を保存する
            oldTouchPos = Input.mousePosition;
        }
        else
        {
            touchFlag = false;


            // 摩擦率をかける
            inertiaMove *= friction;

            // スピードが遅くなったら完全停止させる
            if (Mathf.Abs(inertiaMove.x) <= stopThreshold && Mathf.Abs(inertiaMove.y) <= stopThreshold)
            {
                inertiaMove = Vector3.zero;
            }

            // 摩擦計算込みの移動をする
            MoveObject(inertiaMove);

        }
        
    }

    /// <summary>
    /// オブジェクトを移動させて移動限界にきたら強制的に座標をもどす。
    /// </summary>
    /// <param name="velocity"></param>
    private void MoveObject(Vector3 velocity)
    {
        Vector3 checkPos = gameObject.transform.localPosition + velocity;

        // 左側チェック
        if (checkPos.x < moveLimitRect.xMin)
        {
            checkPos.x = moveLimitRect.xMin;
        }
        // 右側チェック
        if (checkPos.x > moveLimitRect.xMax)
        {
            checkPos.x = moveLimitRect.xMax;
        }
        // 上側チェック
        if (checkPos.y < moveLimitRect.yMin)
        {
            checkPos.y = moveLimitRect.yMin;
        }
        // 下側チェック
        if (checkPos.y > moveLimitRect.yMax)
        {
            checkPos.y = moveLimitRect.yMax;
        }
        // 移動させる
        gameObject.transform.localPosition = checkPos;
    }
}
