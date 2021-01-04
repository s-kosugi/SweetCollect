using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableWaitConnectButton : MonoBehaviour
{
    [SerializeField] PlayFabWaitConnect waitConnect = default;
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
            button.enabled = true;
        }

    }
}
