using System.Collections.Generic;
using UnityEngine;
using PlayFab;


// PlayFab通信待ち管理クラス
public class PlayFabWaitConnect : MonoBehaviour
{
    private List<string> m_WaitList = default;

    void Start()
    {
        m_WaitList = new List<string>();
    }

    void Update()
    {
        
    }

    // 通信待ちを設定する
    public void AddWait(string name)
    {
        // 通信待ちに存在していなければ追加する
        if (!m_WaitList.Contains(name)) m_WaitList.Add(name);
        else Debug.LogError("既に通信待ちしているタスク : " + name);
    }
    // 通信待ちを解除する
    public void RemoveWait(string name)
    {
        if (m_WaitList.Contains(name)) m_WaitList.Remove(name);
        else Debug.LogError("通信待ちしていないタスク : " + name );
    }
    /// <summary>
    /// オブジェクトを指定して通信待ちしているかどうかを取得する
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool GetWait(string name)
    {
        if (m_WaitList.Contains(name) == false) return false;
        return true;
    }
    /// <summary>
    /// 通信待ちがあればtrue
    /// </summary>
    /// <returns></returns>
    public bool IsWait()
    {
        // 通信待ちがあれば通信待ちを返す
        if (m_WaitList.Count > 0 || !PlayFabClientAPI.IsClientLoggedIn()) return true;

        return false;
    }
}
