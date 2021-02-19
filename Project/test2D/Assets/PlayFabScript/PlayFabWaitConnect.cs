using System.Collections.Generic;
using UnityEngine;
using PlayFab;


/// <summary>
/// PlayFab通信待ち管理クラス
/// </summary>
public class PlayFabWaitConnect : MonoBehaviour
{
    /// <summary>
    /// 通信待ちリスト
    /// </summary>
    private List<string> waitList = default;

    void Start()
    {
        waitList = new List<string>();
    }

    /// <summary>
    /// 通信待ちを設定する
    /// </summary>
    /// <param name="name">通信待ちにするタスク名</param>
    public void AddWait(string name)
    {
        // 通信待ちに存在していなければ追加する
        if (!waitList.Contains(name)) waitList.Add(name);
        else Debug.LogError("既に通信待ちしているタスク : " + name);
    }

    /// <summary>
    /// 通信待ちを解除する
    /// </summary>
    /// <param name="name">通信待ちを解除するタスク名</param>
    public void RemoveWait(string name)
    {
        if (waitList.Contains(name)) waitList.Remove(name);
        else Debug.LogError("通信待ちしていないタスク : " + name );
    }

    /// <summary>
    /// 通信待ちをしているかを確認する
    /// </summary>
    /// <param name="name">通信待ちを確認するタスク名</param>
    /// <returns>true:通信待ちしている false:通信待ちしていない</returns>
    public bool GetWait(string name)
    {
        if (waitList.Contains(name) == false) return false;
        return true;
    }
    /// <summary>
    /// 通信待ちをしているかの確認
    /// </summary>
    /// <returns>true:通信待ちをしている false:通信待ち無し</returns>
    public bool IsWait()
    {
        // 通信待ちがあれば通信待ちを返す
        if (waitList.Count > 0 || !PlayFabClientAPI.IsClientLoggedIn()) return true;

        return false;
    }
}
