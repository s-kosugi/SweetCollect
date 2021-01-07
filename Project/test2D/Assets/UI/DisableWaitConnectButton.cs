using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableWaitConnectButton : MonoBehaviour
{
    [SerializeField] PlayFabWaitConnect waitConnect = default;
    [SerializeField] public BaseScene scene = default;
    Button button = default;

    private void Awake()
    {
        button = GetComponent<Button>();

    }
    private void Start()
    {
        if (waitConnect == default) waitConnect = GameObject.Find("PlayFabManager").GetComponent<PlayFabWaitConnect>();
    }

    void Update()
    {
        // 通信中かどうかでボタンの有効化を制御する
        if (waitConnect.IsWait())
        {
            button.enabled = false;
        }
        else
        {
            // シーン状態が通常のときのみ有効化
            if (scene != default)
            {
                if (scene.fadeState == BaseScene.FADE_STATE.NONE)
                {
                    button.enabled = true;

                }
            }
        }
    }
}
