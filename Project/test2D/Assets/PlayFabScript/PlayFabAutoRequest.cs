using UnityEngine;
using PlayFab;

/// <summary>
/// 自動リクエストクラス
/// </summary>
public class PlayFabAutoRequest : MonoBehaviour
{
    /// <summary>
    /// 問い合わせ間隔
    /// </summary>
    [SerializeField] float requestInterval = 1.0f;
    /// <summary>
    ///  問い合わせ用タイマー
    /// </summary>
    private float requestTimer = 1.0f;


    private void Update()
    {
        requestTimer += Time.deltaTime;
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
            if (requestTimer >= requestInterval)
            {
                requestTimer = 0.0f;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// タイマーを強制的に終了状態にする
    /// </summary>
    public void FinishTimer()
    {
        requestTimer = requestInterval;
    }
}
