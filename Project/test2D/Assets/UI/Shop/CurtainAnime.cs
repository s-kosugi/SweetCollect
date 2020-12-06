using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainAnime : MonoBehaviour
{
    [SerializeField] float CloseTime = 0.1f;
    [SerializeField] float OpenTime = 0.2f;
    [SerializeField] float CloseWaitTime = 0.5f;

    private float AnimeCounter = 0f;

    enum STATE
    {
        CLOSE,
        OPEN,
        WAIT,
        CLOSE_WAIT
    }

    STATE state;
    STATE nextState;
    
    void Start()
    {
        state = STATE.WAIT;
        nextState = STATE.WAIT;
        transform.localScale = new Vector3(0f, transform.localScale.y, transform.localScale.z);
    }


    void Update()
    {
        switch(state)
        {
            case STATE.CLOSE: Close(); break;
            case STATE.OPEN: Open(); break;
            case STATE.WAIT: Wait(); break;
            case STATE.CLOSE_WAIT: CloseWait(); break;

        }
    }

    void Close()
    {
        AnimeCounter += Time.deltaTime;
        if (AnimeCounter < CloseTime)
        {
            transform.localScale = new Vector3(Easing.Linear(AnimeCounter, CloseTime, 1f, 0f), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // 次の状態がオープンだったら切り替える
            if (nextState == STATE.OPEN)
            {
                state = STATE.CLOSE_WAIT;
                AnimeCounter = 0f;
            }
        }
    }

    void Open()
    {
        AnimeCounter += Time.deltaTime;
        if (AnimeCounter < OpenTime)
        {
            transform.localScale = new Vector3(Easing.Linear(AnimeCounter, OpenTime, 0f, 1f), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // アニメーション終了時に自動的に待機状態にする
            transform.localScale = new Vector3(0f, transform.localScale.y, transform.localScale.z);
            state = STATE.WAIT;
            AnimeCounter = 0f;
        }
    }

    void Wait()
    {
        transform.localScale = new Vector3(0f,transform.localScale.y,transform.localScale.z);
        // 次の状態がクローズだったら切り替える
        if (nextState == STATE.CLOSE)
        {
            SoundManager.Instance.PlaySE("Curtain");
            state = STATE.CLOSE;
            AnimeCounter = 0f;
        }
    }
    void CloseWait()
    {
        // アニメーションは閉じたままにする
        transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);

        AnimeCounter += Time.deltaTime;
        if (AnimeCounter >= CloseWaitTime)
        {
            if (nextState == STATE.OPEN)
            {
                SoundManager.Instance.PlaySE("Curtain");
                AnimeCounter = 0f;
                state = STATE.OPEN;
            }
        }
    }

    public void ChangeOpen()
    {
        nextState = STATE.OPEN;
    }

    public void ChangeClose()
    {
        nextState = STATE.CLOSE;
    }
}
