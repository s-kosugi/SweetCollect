using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabAutoRequest : MonoBehaviour
{
    /// <summary>
    /// 問い合わせ間隔
    /// </summary>
    [SerializeField] float RequestInterval = 1.0f;
    /// <summary>
    ///  問い合わせ用タイマー
    /// </summary>
    private float m_RequestTimer = 1.0f;


    private void Update()
    {
        m_RequestTimer += Time.deltaTime;
    }
    /// <summary>
    /// リクエストできるかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsRequest()
    {
        // Playfabにログイン済みかを確認する
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            // 問い合わせタイマーを満たしていたら問い合わせる
            if (m_RequestTimer >= RequestInterval)
            {
                m_RequestTimer = 0.0f;
                return true;
            }
        }
        return false;
    }
}
