using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFontJump : MonoBehaviour
{
    [SerializeField] float JumpPower = 1000f;
    [SerializeField] float Gravity = 50f;
    [SerializeField] float JumpInterval = 1.5f;
    [SerializeField] float JumpTimer = 0f;

    private float velocity = 0f;
    private float landingLine = 0f;

    void Awake()
    {
        landingLine = transform.localPosition.y;
    }

    void Update()
    {
        JumpTimer += Time.deltaTime;
        if (JumpTimer >= JumpInterval)
        {
            velocity = JumpPower * Time.deltaTime;
            JumpTimer = 0f;
        }
        // 移動処理
        Vector3 vec = transform.localPosition;
        vec.y += velocity;
        transform.localPosition = vec;

        // 重力加速処理
        velocity -= Gravity * Time.deltaTime;
        // 地面にうまった
        if (landingLine >= transform.localPosition.y)
        {
            vec.y = landingLine;
            transform.localPosition = vec;
            velocity = 0f;
        }
    }
}
