using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFontAppear : MonoBehaviour
{
    [SerializeField] TitleManager titleManager= default;
    [SerializeField] float StartHeight = 1000.0f;
    [SerializeField] float FallTime = 3.0f;
    [SerializeField] float WaitTime = 0f;
    [SerializeField] float GoalHeight = 0f;
    [SerializeField] float TimeCounter = 0f;
    private TitleFontJump fontJump = default;
    void Start()
    {
        GoalHeight = transform.localPosition.y;
        Vector3 vec = transform.localPosition;
        StartHeight += GoalHeight;
        vec.y = StartHeight;
        transform.localPosition = vec;
        TimeCounter = -WaitTime;
        fontJump = GetComponent<TitleFontJump>();
    }


    void Update()
    {
        // タイトル画面が通常状態のときに処理する
        if (titleManager.state == TitleManager.STATE.MAIN)
        {
            TimeCounter += Time.deltaTime;
            if ((TimeCounter >= FallTime))
            {
                Vector3 vec = transform.localPosition;
                vec.y = GoalHeight;
                transform.localPosition = vec;
                // ジャンプアニメーションを有効にする
                fontJump.enabled = true;

                // 自身の処理を無効化する
                this.enabled = false;
            }
            else if (TimeCounter >= 0)
            {
                Vector3 vec = transform.localPosition;
                vec.y = Easing.OutBounce(TimeCounter, FallTime, GoalHeight, StartHeight);
                transform.localPosition = vec;
            }
            // 待ち時間中は処理しない
        }
    }
}
